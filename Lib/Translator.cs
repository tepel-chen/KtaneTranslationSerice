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
        private Dictionary<string, string>? _moduleDict;


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

        public Dictionary<string, string>? LoadTranslations()
        {
            return _moduleDict ??= loader.GetTranslation(langCode);
        }

        public string? GetTranslation(string from, string moduleName, List<string>? categories = null)
        {
            var moduleDict = LoadTranslations();
            if (moduleDict is null) return null;
            var trimmed = from.Trim();
            var lower = trimmed.ToLowerInvariant();
            var upper = trimmed.ToUpperInvariant();
            if (moduleDict!.TryGetValue(moduleName + ":" +lower, out string to) || moduleDict!.TryGetValue(lower, out to))
            {
                if (trimmed == upper) return to.ToUpperInvariant();
                else if (trimmed[0] == upper[0]) return to.Substring(0, 1).ToUpperInvariant() + to.Substring(1).ToLowerInvariant();
                else return to.ToLowerInvariant();
            }
            var key = categories?.Find(c => moduleDict!.ContainsKey($"<{c}>:{lower}")) ?? "";
            if (key.Length > 0 && moduleDict!.TryGetValue($"<{key}>:{lower}", out to))
            {
                if (trimmed == upper) return to.ToUpperInvariant();
                else if (trimmed[0] == upper[0]) return to.Substring(0, 1).ToUpperInvariant() + to.Substring(1).ToLowerInvariant();
                else return to.ToLowerInvariant();
            }
            return null;
        }

        public void SetTranslationToMeshes(TextMesh[] textmeshes, KMBombModule module, List<string>? categories = null)
        {
            SetTranslationToMeshes(textmeshes, module, Magnifier.Default, categories);
        }
        public void SetTranslationToMeshes(TextMesh[] textmeshes, KMBombModule module, Magnifier magnifier, List<string>? categories = null)
        {
            foreach (var textMesh in textmeshes)
            {
                SetTranslationToMesh(textMesh, module, magnifier, categories);
            }
        }
        public void SetTranslationToMesh(TextMesh textmesh, KMBombModule module, Magnifier magnifier, List<string>? categories = null)
        {
            MeshRenderer renderer = textmesh.GetComponent<MeshRenderer>();
            Transform transform = textmesh.GetComponent<Transform>();
            string moduleName = module.ModuleDisplayName;

            if (GetTranslation(textmesh.text, moduleName, categories) is string translated)
            {

                // Changing alignment to center
                transform.position = renderer.bounds.center;
                textmesh.anchor = TextAnchor.MiddleCenter;

                var parent = transform.parent;
                var rotation = transform.localRotation;
                var scale = transform.localScale;
                var position = transform.localPosition;

                transform.parent = null;
                transform.rotation = Quaternion.identity;
                transform.position = new Vector3(0, 0, 0);
                transform.localScale = new Vector3(scale.x, scale.y, 1);
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
                logger.Log($"Found text \"{beforeTranslation}\" in module {moduleName}. Translating to \"{translated}\"");
                transform.parent = parent;
                transform.localPosition = position;
                transform.localScale = scale * m;
                transform.localRotation = rotation;

            }
            else if (settings.EnableSuggestionLog && new Regex(@"^[A-Za-z]{2,}$").IsMatch(textmesh.text))
            {
                logger.Log($"[Suggest] Found {textmesh.text} in {moduleName} but translation not found.");
            }
        }
    }
}
