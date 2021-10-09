using System;
using System.Collections;
using UnityEngine;
using HarmonyLib;
using System.Reflection;

namespace TranslationService.ModuleTranslators
{
    class MurderTranslator : ModuleTranslator
    {
        private readonly MethodInfo mUpdateDisplayPostfix;
        public MurderTranslator(Harmony harmony)
        {
            mUpdateDisplayPostfix = SymbolExtensions.GetMethodInfo((MonoBehaviour __instance) => UpdateDisplayPostfix(__instance));
            harmony.Patch(AccessTools.Method(componentType, "ChangeDisplay"), null, new HarmonyMethod(mUpdateDisplayPostfix));
        }

        private static readonly Type componentType = ReflectionHelper.FindType("MurderModule");

        public override void StartTranslation(KMBombModule module, Translator translator)
        {
            MurderTranslator.translator = translator;
            var texts = module.GetComponentsInChildren<TextMesh>();
            translator.SetTranslationToMeshes(texts, module, Magnifier.Default);

        }

        public static Translator translator = null;

        public static void UpdateDisplayPostfix(MonoBehaviour __instance)
        {
            if (translator != null) translator.SetTranslationToMeshes(__instance.GetValue<TextMesh[]>("Display"), __instance.GetComponent<KMBombModule>(), new Magnifier.VectorMagnifier(1f, 0.011566f));
        }
    }
}