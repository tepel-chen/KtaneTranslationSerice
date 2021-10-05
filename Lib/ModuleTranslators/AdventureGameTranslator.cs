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
            mUpdateStatDisplayPostfix = SymbolExtensions.GetMethodInfo((object __instance) => UpdateStatDisplayPostfix(__instance));
            mUpdateInvDisplayPostfix = SymbolExtensions.GetMethodInfo((object __instance) => UpdateInvDisplayPostfix(__instance));
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

        public static void UpdateStatDisplayPostfix(object __instance)
        {
            if(translator != null) translator.SetTranslationToMesh(__instance.GetValue<TextMesh>("TextStatus"), (KMBombModule)__instance, new Magnifier.VectorMagnifier(0.07f, 0.016176f));
        }
        public static void UpdateInvDisplayPostfix(object __instance)
        {
            if (translator != null) translator.SetTranslationToMesh(__instance.GetValue<TextMesh>("TextInventory"), (KMBombModule)__instance, new Magnifier.VectorMagnifier(0.07f, 0.016176f));
        }
    }
}
