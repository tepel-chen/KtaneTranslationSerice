using HarmonyLib;
using System;
using System.Reflection;
using UnityEngine;

namespace TranslationService.ModuleTranslators
{
    class PokerTranslator : ModuleTranslator
    {
        private readonly MethodInfo mButtonPressPostfix;
        public PokerTranslator(Harmony harmony)
        {
            if (isPatched || componentType == null) return;
            isPatched = true;
            mButtonPressPostfix = SymbolExtensions.GetMethodInfo((MonoBehaviour __instance) => ButtonPressPostfix(__instance));
            harmony.Patch(AccessTools.Method(componentType, "ButtonPress"), null, new HarmonyMethod(mButtonPressPostfix));
        }

        private static readonly Type componentType = ReflectionHelper.FindType("PokerScript");
        private static bool isPatched = false;
        private static Magnifier displayMagnifier(string langCode) => new Magnifier.VectorMagnifier(0.9f, 0.79f);

        public override void StartTranslation(KMBombModule module, Translator translator)
        {
            PokerTranslator.translator = translator;
            var texts = module.GetComponentsInChildren<TextMesh>();
            translator.SetTranslationToMeshes(texts, module, Magnifier.Default);
        }

        public static Translator translator = null;

        public static void ButtonPressPostfix(MonoBehaviour __instance)
        {
            if (translator != null) translator.SetTranslationToMesh(__instance.GetValue<TextMesh>("ResponseText"), __instance.GetComponent<KMBombModule>(), displayMagnifier(translator.langCode));
        }
    }

}
