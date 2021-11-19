using System;
using UnityEngine;
using HarmonyLib;
using System.Reflection;
using System.Linq;

namespace TranslationService.ModuleTranslators
{
    class TwoBitsTranslator : ModuleTranslator
    {
        private readonly MethodInfo mUpdateDisplayPostfix;
        public TwoBitsTranslator(Harmony harmony)
        {
            if (isPatched || componentType == null) return;
            isPatched = true;
            mUpdateDisplayPostfix = SymbolExtensions.GetMethodInfo((MonoBehaviour __instance) => UpdateDisplayPostfix(__instance));
            harmony.Patch(AccessTools.Method(componentType, "UpdateDisplay"), null, new HarmonyMethod(mUpdateDisplayPostfix));
        }

        private static readonly string moduleName = "Two Bits";
        private static readonly Type componentType = ReflectionHelper.FindType("TwoBitsModule");
        private static bool isPatched = false;
        private static Magnifier displayMagnifier(string langCode) => new Magnifier.VectorMagnifier(1f, 0.5f);

        public override void StartTranslation(KMBombModule module, Translator translator)
        {
            TwoBitsTranslator.translator = translator;
            var texts = module.GetComponentsInChildren<TextMesh>().Where(text => text.text == "Query" || text.text == "Submit").ToArray();
            translator.SetTranslationToMeshes(texts, module, Magnifier.Default);

            var twoBitsModule = module.GetComponentInParent(componentType);
            ReflectionHelper.SetValue(componentType, "ERROR_STRING", translator.GetTranslation("ERROR", moduleName), twoBitsModule);
            ReflectionHelper.SetValue(componentType, "INCORRECT_SUBMISSION_STRING", translator.GetTranslation("INCORRECT", moduleName), twoBitsModule);
        }

        public static Translator translator = null;

        public static void UpdateDisplayPostfix(MonoBehaviour __instance)
        {
            if (translator != null)
            {
                var textMesh = __instance.GetValue<TextMesh>("DisplayText");
                if (textMesh.text.StartsWith("Result:"))
                {
                    var textParts = textMesh.text.Split(' ');
                    textParts[0] = translator.GetTranslation("Result:", moduleName);
                    textMesh.text = textParts.Join(delimiter: " ");
                }
                translator.SetTranslationToMesh(textMesh, __instance.GetComponent<KMBombModule>(), displayMagnifier(translator.langCode));
            }

        }
    }
}
