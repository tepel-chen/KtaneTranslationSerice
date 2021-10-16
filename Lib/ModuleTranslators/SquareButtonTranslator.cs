using System;
using UnityEngine;
using HarmonyLib;
using System.Reflection;
using System.Linq;
using KeepCoding;

namespace TranslationService.ModuleTranslators
{
    class SquareButtonTranslator : ModuleTranslator
    {

        private readonly MethodInfo mPressPostfix;
        public SquareButtonTranslator(Harmony harmony)
        {
            if (isPatched || componentType == null) return;
            isPatched = true;
            mPressPostfix = SymbolExtensions.GetMethodInfo((MonoBehaviour __instance) => PressPostfix(__instance));
            harmony.Patch(AccessTools.Method(componentType, "Press"), null, new HarmonyMethod(mPressPostfix));
        }

        private static readonly Type componentType = ReflectionHelper.FindType("AdvancedButton");
        private static bool isPatched = false;
        private static Magnifier displayMagnifier(string langCode) => langCode == "ja" ? new Magnifier.VectorMagnifier(0.1f, 0.025275f) : new Magnifier.VectorMagnifier(0.1f, 0.01685f);

        public override void AwakeTranslation(KMBombModule module, Translator translator)
        {
            SquareButtonTranslator.translator = translator;
            var text = module.GetComponentInChildren<TextMesh>();
            translator.SetTranslationToMesh(text, module, displayMagnifier(translator.langCode));
        }

        public static Translator translator = null;
        private static bool isPressPatched = false;

        public static void PressPostfix(MonoBehaviour __instance)
        {
            if (!isPressPatched && translator != null)
            {
                isPressPatched = true;
                var labels = __instance.GetValue<string[]>("buttonLabels");
                for (int i = 0; i < labels.Length; ++i)
                    labels[i] = translator.GetTranslation(labels[i], "Square Button");
                __instance.SetValue("buttonLabels", labels);
            }
        }
    }
}
