using System;
using HarmonyLib;
using UnityEngine;

namespace TranslationService.ModuleTranslators
{
    class LaundryTranslator : ModuleTranslator
    {
        public LaundryTranslator(Harmony harmony)
        {
            if (isPatched || componentType == null) return;
            isPatched = true;
        }

        private static readonly string moduleName = "Laundry";
        private static readonly Type componentType = ReflectionHelper.FindType("Laundry");
        private static bool isPatched = false;

        public override void StartTranslation(KMBombModule module, Translator translator)
        {
            LaundryTranslator.translator = translator;
            var laundryModule = module.GetComponentInParent(componentType);

            string[] washingText = new string[] { translator.GetTranslation("Machine Wash Permanent Press", moduleName), translator.GetTranslation("Machine Wash Gentle or Delicate", moduleName), translator.GetTranslation("Hand Wash", moduleName), translator.GetTranslation("Do Not Wash", moduleName), translator.GetTranslation("30°C", moduleName), translator.GetTranslation("40°C", moduleName), translator.GetTranslation("50°C", moduleName), translator.GetTranslation("60°C", moduleName), translator.GetTranslation("70°C", moduleName), translator.GetTranslation("95°C", moduleName), translator.GetTranslation("Do Not Wring", moduleName) };
            string[] dryingText = new string[] { translator.GetTranslation("Tumble Dry", moduleName), translator.GetTranslation("1 Dot", moduleName), translator.GetTranslation("2 Dot", moduleName), translator.GetTranslation("3 Dot", moduleName), translator.GetTranslation("No Heat", moduleName), translator.GetTranslation("Hang to Dry", moduleName), translator.GetTranslation("Drip Dry", moduleName), translator.GetTranslation("Dry Flat", moduleName), translator.GetTranslation("Dry in the Shade", moduleName), translator.GetTranslation("Do Not Dry", moduleName), translator.GetTranslation("Do Not Tumble Dry", moduleName), translator.GetTranslation("Dry", moduleName) };
            string[] ironingText = new string[] { translator.GetTranslation("Iron", moduleName), translator.GetTranslation("Do Not Iron", moduleName), translator.GetTranslation("110°C", moduleName), translator.GetTranslation("300°F", moduleName), translator.GetTranslation("200°C", moduleName), translator.GetTranslation("No Steam", moduleName) };
            string[] specialText = new string[] { translator.GetTranslation("Bleach", moduleName), translator.GetTranslation("Don't Bleach", moduleName), translator.GetTranslation("No Chlorine", moduleName), translator.GetTranslation("Dryclean", moduleName), translator.GetTranslation("Any Solvent", moduleName), translator.GetTranslation("No Tetrachlore", moduleName), translator.GetTranslation("Petroleum Only", moduleName), translator.GetTranslation("Wet Cleaning", moduleName), translator.GetTranslation("Do Not Dryclean", moduleName), translator.GetTranslation("Short Cycle", moduleName), translator.GetTranslation("Reduced Moist", moduleName), translator.GetTranslation("Low Heat", moduleName), translator.GetTranslation("No Steam Finish", moduleName) };
            string[] clothingNames = new string[] { translator.GetTranslation("Corset", moduleName), translator.GetTranslation("Shirt", moduleName), translator.GetTranslation("Skirt", moduleName), translator.GetTranslation("Skort", moduleName), translator.GetTranslation("Shorts", moduleName), translator.GetTranslation("Scarf", moduleName) };
            string[] materialNames = new string[] { translator.GetTranslation("Polyester", moduleName), translator.GetTranslation("Cotton", moduleName), translator.GetTranslation("Wool", moduleName), translator.GetTranslation("Nylon", moduleName), translator.GetTranslation("Corduroy", moduleName), translator.GetTranslation("Leather", moduleName) };
            string[] colorNames = new string[] { translator.GetTranslation("Ruby Fountain", moduleName), translator.GetTranslation("Star Lemon Quartz", moduleName), translator.GetTranslation("Sapphire Springs", moduleName), translator.GetTranslation("Jade Cluster", moduleName), translator.GetTranslation("Clouded Pearl", moduleName), translator.GetTranslation("Malinite", moduleName) };
            ReflectionHelper.SetValue(componentType, "washingText", washingText, laundryModule);
            ReflectionHelper.SetValue(componentType, "dryingText", dryingText, laundryModule);
            ReflectionHelper.SetValue(componentType, "ironingText", ironingText, laundryModule);
            ReflectionHelper.SetValue(componentType, "specialText", specialText, laundryModule);
            ReflectionHelper.SetValue(componentType, "clothingNames", clothingNames, laundryModule);
            ReflectionHelper.SetValue(componentType, "materialNames", materialNames, laundryModule);
            ReflectionHelper.SetValue(componentType, "colorNames", colorNames, laundryModule);

            var texts = module.GetComponentsInChildren<TextMesh>();
            translator.SetTranslationToMeshes(texts, module, Magnifier.Default);
        }

        public static Translator translator = null;
    }
}
