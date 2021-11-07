using System;
using UnityEngine;
using HarmonyLib;
using System.Reflection;
using System.Linq;

namespace TranslationService.ModuleTranslators
{
    class IdentityParadeTranslator : ModuleTranslator
    {
        private readonly MethodInfo mOnhairPostfix;
        private readonly MethodInfo mOnbuildPostfix;
        private readonly MethodInfo mOnattirePostfix;
        private readonly MethodInfo mOnsuspectPostfix;
        public IdentityParadeTranslator(Harmony harmony)
        {
            if (isPatched || componentType == null) return;
            isPatched = true;
            mOnhairPostfix = SymbolExtensions.GetMethodInfo((MonoBehaviour __instance) => OnhairPostfix(__instance));
            mOnbuildPostfix = SymbolExtensions.GetMethodInfo((MonoBehaviour __instance) => OnbuildPostfix(__instance));
            mOnattirePostfix = SymbolExtensions.GetMethodInfo((MonoBehaviour __instance) => OnattirePostfix(__instance));
            mOnsuspectPostfix = SymbolExtensions.GetMethodInfo((MonoBehaviour __instance) => OnsuspectPostfix(__instance));
            harmony.Patch(AccessTools.Method(componentType, "OnhairLeft"), null, new HarmonyMethod(mOnhairPostfix));
            harmony.Patch(AccessTools.Method(componentType, "OnhairRight"), null, new HarmonyMethod(mOnhairPostfix));
            harmony.Patch(AccessTools.Method(componentType, "OnbuildLeft"), null, new HarmonyMethod(mOnbuildPostfix));
            harmony.Patch(AccessTools.Method(componentType, "OnbuildRight"), null, new HarmonyMethod(mOnbuildPostfix));
            harmony.Patch(AccessTools.Method(componentType, "OnattireLeft"), null, new HarmonyMethod(mOnattirePostfix));
            harmony.Patch(AccessTools.Method(componentType, "OnattireRight"), null, new HarmonyMethod(mOnattirePostfix));
            harmony.Patch(AccessTools.Method(componentType, "OnsuspectLeft"), null, new HarmonyMethod(mOnsuspectPostfix));
            harmony.Patch(AccessTools.Method(componentType, "OnsuspectRight"), null, new HarmonyMethod(mOnsuspectPostfix));
        }

        private static readonly Type componentType = ReflectionHelper.FindType("identityParadeScript");
        private static bool isPatched = false;
        private static Magnifier displayMagnifier(string langCode) => langCode == "ja" ? new Magnifier.VectorMagnifier(0.1f, 0.02f) : new Magnifier.VectorMagnifier(0.1f, 0.013311f);

        public override void StartTranslation(KMBombModule module, Translator translator)
        {
            IdentityParadeTranslator.translator = translator;
            var text = module.GetComponentsInChildren<TextMesh>().First(text => text.text == "CONVICT");
            translator.SetTranslationToMesh(text, module, Magnifier.Default);
        }

        public override void AwakeTranslation(KMBombModule module, Translator translator)
        {
            var texts = module.GetComponentsInChildren<TextMesh>().Where(text => text.text != "CONVICT").ToArray();
            translator.SetTranslationToMeshes(texts, module, displayMagnifier(translator.langCode));
        }

        public static Translator translator = null;

        public static void OnhairPostfix(MonoBehaviour __instance)
        {
            if (translator != null) translator.SetTranslationToMesh(__instance.GetValue<TextMesh>("hairText"), __instance.GetComponent<KMBombModule>(), displayMagnifier(translator.langCode));
        }

        public static void OnbuildPostfix(MonoBehaviour __instance)
        {
            if (translator != null) translator.SetTranslationToMesh(__instance.GetValue<TextMesh>("buildText"), __instance.GetComponent<KMBombModule>(), displayMagnifier(translator.langCode));
        }

        public static void OnattirePostfix(MonoBehaviour __instance)
        {
            if (translator != null) translator.SetTranslationToMesh(__instance.GetValue<TextMesh>("attireText"), __instance.GetComponent<KMBombModule>(), displayMagnifier(translator.langCode));
        }

        public static void OnsuspectPostfix(MonoBehaviour __instance)
        {
            if (translator != null) translator.SetTranslationToMesh(__instance.GetValue<TextMesh>("suspectText"), __instance.GetComponent<KMBombModule>(), displayMagnifier(translator.langCode));
        }
    }
}
