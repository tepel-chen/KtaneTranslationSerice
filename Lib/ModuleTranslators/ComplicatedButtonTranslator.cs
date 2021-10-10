﻿using System;
using UnityEngine;
using HarmonyLib;
using System.Reflection;

namespace TranslationService.ModuleTranslators
{
    class ComplicatedButtonTranslator : ModuleTranslator
    {
        private readonly MethodInfo mOnActivatePostfix;
        public ComplicatedButtonTranslator(Harmony harmony)
        {
            if (isPatched || componentType == null) return;
            isPatched = true;
            mOnActivatePostfix = SymbolExtensions.GetMethodInfo((MonoBehaviour __instance) => OnActivatePostfix(__instance));
            harmony.Patch(AccessTools.Method(componentType, "OnActivate"), null, new HarmonyMethod(mOnActivatePostfix));
        }

        private static readonly Type componentType = ReflectionHelper.FindType("ComplicatedButtonsModule");
        private static bool isPatched = false;
        private static Magnifier displayMagnifier(string langCode) => new Magnifier.VectorMagnifier(0.5f, 0.015f);

        public override void StartTranslation(KMBombModule module, Translator translator)
        {
            ComplicatedButtonTranslator.translator = translator;
        }

        public static Translator translator = null;

        public static void OnActivatePostfix(MonoBehaviour __instance)
        {
            if (translator != null) translator.SetTranslationToMeshes(__instance.GetValue<TextMesh[]>("Labels"), __instance.GetComponent<KMBombModule>(), displayMagnifier(translator.langCode));
        }

    }
}
