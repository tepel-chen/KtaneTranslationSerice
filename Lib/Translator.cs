#nullable enable

using KeepCoding;
using System.Collections;
using System.Collections.Generic;
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

        public void SetTranslationToMeshes(TextMesh[] textmeshes, string moduleName)
        {
            SetTranslationToMeshes(textmeshes, moduleName, Magnifier.Default);
        }
        public void SetTranslationToMeshes(TextMesh[] textmeshes, string moduleName, Magnifier magnifier)
        {
            foreach (var textMesh in textmeshes)
            {
                SetTranslationToMesh(textMesh, moduleName, magnifier);
            }
        }
        public void SetTranslationToMesh(TextMesh textmesh, string moduleName, Magnifier magnifier)
        {
            MeshRenderer renderer = textmesh.GetComponent<MeshRenderer>();
            Transform transform = textmesh.GetComponent<Transform>();

            if (GetTranslation(textmesh.text, moduleName) is string translated)
            {

                // Changing alignment to center
                Bounds beforeBounds = renderer.bounds;
                textmesh.anchor = TextAnchor.MiddleCenter;
                transform.position = beforeBounds.center;

                string beforeTranslation = textmesh.text;
                if (font != null && fontMaterial != null)
                {
                    textmesh.font = font;
                    renderer.material = fontMaterial;
                }
                textmesh.text = translated;
                Bounds afterBounds = renderer.bounds;
                float m = magnifier.GetMagnifier(beforeBounds.size, afterBounds.size, beforeTranslation);
                if (m > 0.1) transform.localScale *= m;
                logger.Log($"Found text \"{beforeTranslation}\" in module {moduleName}. Translating to \"{translated}\"");

            }
            else if (settings.EnableSuggestionLog && new Regex(@"^[A-Za-z]{2,}$").IsMatch(textmesh.text))
            {
                logger.Log($"[Suggest] Found {textmesh.text} in {moduleName} but translation not found.");
            }
        }
    }
}
