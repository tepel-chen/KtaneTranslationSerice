#nullable enable

using KeepCoding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TranslationService.Loaders;
using UnityEngine;

namespace TranslationService
{
    public class Translator : CustomYieldInstruction
    {

        public ILog logger;
        public Font? font;
        public Material? fontMaterial;
        public Settings settings;
        public string langCode;

        private TranslationLoader loader;
        private Dictionary<string, string>? _dict;


        public override bool keepWaiting => loader.keepWaiting;

        public Translator(ILog logger, Font? font, Material? fontMaterial, Settings settings, string langCode)
        {
            this.logger = logger;
            this.font = font;
            this.fontMaterial = fontMaterial;
            this.settings = settings;
            this.langCode = langCode;

            loader = new TranslationLoader(logger);
        }

        private Dictionary<string, string>? LoadTranslations()
        {
            return _dict ??= loader.GetTranslation(langCode);
        }

        public string? GetTranslation(string from, string moduleName)
        {
            if (LoadTranslations() is not Dictionary<string, string> dict) return null;
            var trimmed = from.Trim();
            var lower = trimmed.ToLowerInvariant();
            var upper = trimmed.ToUpperInvariant();
            if (dict.TryGetValue(moduleName + ":" +lower, out string to) || dict.TryGetValue(lower, out to))
            {
                if (trimmed == upper) return to.ToUpperInvariant();
                else if (trimmed[0] == upper[0]) return to.Substring(0, 1).ToUpperInvariant() + to.Substring(1).ToLowerInvariant();
                else return to.ToLowerInvariant();
            }
            return null;
        }

        public void SetTranslationToMeshes(TextMesh[] textmeshes, KMBombModule module)
        {
            SetTranslationToMeshes(textmeshes, module, Magnifier.Default);
        }
        public void SetTranslationToMeshes(TextMesh[] textmeshes, KMBombModule module, Magnifier magnifier)
        {
            foreach (var textMesh in textmeshes)
            {
                SetTranslationToMesh(textMesh, module, magnifier);
            }
        }
        public void SetTranslationToMesh(TextMesh textmesh, KMBombModule module, Magnifier magnifier)
        {
            MeshRenderer renderer = textmesh.GetComponent<MeshRenderer>();
            Transform transform = textmesh.GetComponent<Transform>();
            string moduleName = module.ModuleDisplayName;

            if (GetTranslation(textmesh.text, moduleName) is string translated)
            {

                // Changing alignment to center
                textmesh.anchor = TextAnchor.MiddleCenter;
                transform.position = renderer.bounds.center;

                var rotation = transform.rotation;
                transform.rotation = Quaternion.identity;
                Vector2 beforeSize = renderer.bounds.size;

                string beforeTranslation = textmesh.text;
                if (font != null && fontMaterial != null)
                {
                    textmesh.font = font;
                    renderer.material = fontMaterial;
                }
                textmesh.text = translated;

                Vector2 afterSize = renderer.bounds.size;
                float m = magnifier.GetMagnifier(beforeSize, afterSize, beforeTranslation, module);
                transform.localScale *= m;
                logger.Log((afterSize * 1/module.GetComponent<BombComponent>().Bomb.Scale).ToString("F6"));
                logger.Log($"Found text \"{beforeTranslation}\" in module {moduleName}. Translating to \"{translated}\"");

                transform.rotation = rotation;

            }
            else if (settings.EnableSuggestionLog && new Regex(@"^[A-Za-z]{2,}$").IsMatch(textmesh.text))
            {
                logger.Log($"[Suggest] Found {textmesh.text} in {moduleName} but translation not found.");
            }
        }
    }
}
