using System;
using System.Collections;
using UnityEngine;
using HarmonyLib;
using System.Reflection;

namespace TranslationService.ModuleTranslators
{
    class AdventureGameTranslator : ModuleTranslator
    {
        private readonly MethodInfo mUpdateStatDisplayPostfix;
        private readonly MethodInfo mUpdateInvDisplayPostfix;
        public AdventureGameTranslator(Harmony harmony)
        {
            mUpdateStatDisplayPostfix = SymbolExtensions.GetMethodInfo((MonoBehaviour __instance) => UpdateStatDisplayPostfix(__instance));
            mUpdateInvDisplayPostfix = SymbolExtensions.GetMethodInfo((MonoBehaviour __instance) => UpdateInvDisplayPostfix(__instance));
            harmony.Patch(AccessTools.Method(componentType, "UpdateStatDisplay"), null, new HarmonyMethod(mUpdateStatDisplayPostfix));
            harmony.Patch(AccessTools.Method(componentType, "UpdateInvDisplay"), null, new HarmonyMethod(mUpdateInvDisplayPostfix));
        }

        private static readonly Type componentType = ReflectionHelper.FindType("AdventureGameModule");

        public override void StartTranslation(KMBombModule module, Translator translator)
        {
            AdventureGameTranslator.translator = translator;
            var texts = module.GetComponentsInChildren<TextMesh>();
            translator.SetTranslationToMeshes(texts, module, Magnifier.Default);

        }

        public static Translator translator = null;

        public static void UpdateStatDisplayPostfix(MonoBehaviour __instance)
        {
            if (translator != null) translator.SetTranslationToMesh(__instance.GetValue<TextMesh>("TextStatus"), __instance.GetComponent<KMBombModule>(), new Magnifier.VectorMagnifier(0.07f, 0.016176f));
        }
        public static void UpdateInvDisplayPostfix(MonoBehaviour __instance)
        {
            if (translator != null) translator.SetTranslationToMesh(__instance.GetValue<TextMesh>("TextInventory"), __instance.GetComponent<KMBombModule>(), new Magnifier.VectorMagnifier(0.07f, 0.016176f));
        }
    }
}
