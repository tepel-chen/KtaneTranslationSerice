#nullable enable

using KeepCoding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace TranslationService
{
    public abstract class DefaultTranslator
    {
        public abstract Dictionary<string, string> GetDefaultDict();

        public ILog logger;
        public Font? font;
        public Material? fontMaterial;
        public Settings settings;

        public DefaultTranslator(ILog logger, Font? font, Material? fontmaterial, Settings settings)
        {
            this.logger = logger;
            this.font = font;
            this.fontMaterial = fontmaterial;
            this.settings = settings;
        }

        public string? GetTranslation(string from, Dictionary<string, string> dict, string moduleName)
        {
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

        public IEnumerator SetTranslationToMeshes(TextMesh[] textmeshes, string moduleName)
        {
            return SetTranslationToMeshes(textmeshes, moduleName, GetDefaultDict(), Magnifier.Default);
        }
        public IEnumerator SetTranslationToMeshes(TextMesh[] textmeshes, string moduleName, Dictionary<string, string> dict)
        {
            return SetTranslationToMeshes(textmeshes, moduleName, dict, Magnifier.Default);
        }
        public IEnumerator SetTranslationToMeshes(TextMesh[] textmeshes, string moduleName, Magnifier magnifier)
        {
            return SetTranslationToMeshes(textmeshes, moduleName, GetDefaultDict(), magnifier);
        }
        public IEnumerator SetTranslationToMeshes(TextMesh[] textmeshes, string moduleName, Dictionary<string, string> dict, Magnifier magnifier)
        {
            foreach(var textMesh in textmeshes)
            {
                yield return SetTranslationToMesh(textMesh, moduleName, dict, magnifier);
            }
            
        }
        public IEnumerator SetTranslationToMesh(TextMesh textmesh, string moduleName, Dictionary<string, string> dict, Magnifier magnifier)
        {
            MeshRenderer renderer = textmesh.GetComponent<MeshRenderer>();
            Transform transform = textmesh.GetComponent<Transform>();

            if (GetTranslation(textmesh.text, dict, moduleName) is string translated)
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
                yield return null; // rerender
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
        public static DefaultTranslator? GetTranslatorFromCode(string code, ILog logger, Font font, Material fontMaterial, Settings settings)
        {
            return code switch
            {
                "ja" => new JapaneseTranslator(logger, font, fontMaterial, settings),
                "fr" => new FrenchTranslator(logger, font, fontMaterial, settings),
                _ => null,
            };
        }
    }
}
