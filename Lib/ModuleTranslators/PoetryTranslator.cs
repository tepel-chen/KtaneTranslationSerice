using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TranslationService.ModuleTranslators
{
    class PoetryTranslator : ModuleTranslator
    {
        public PoetryTranslator(Harmony harmony)
        {
            if (isPatched || componentType == null) return;
            isPatched = true;
        }

        private static readonly string moduleName = "Poetry";
        private static readonly Type componentType = ReflectionHelper.FindType("PoetryModule");
        private static bool isPatched = false;

        public override void StartTranslation(KMBombModule module, Translator translator)
        {
            PoetryTranslator.translator = translator;
            var poetryModule = module.GetComponentInParent(componentType);

            string[,] wordTable = new string[,]
            {
                {"Melanie",translator.GetTranslation("clarity", moduleName),translator.GetTranslation("flow", moduleName),translator.GetTranslation("fatigue", moduleName),translator.GetTranslation("hollow", moduleName),"Jane"},
                {translator.GetTranslation("energy", moduleName),translator.GetTranslation("sunshine", moduleName),translator.GetTranslation("ocean", moduleName),translator.GetTranslation("reflection", moduleName),translator.GetTranslation("identity", moduleName),translator.GetTranslation("black", moduleName)},
                {translator.GetTranslation("crowd", moduleName),translator.GetTranslation("heart", moduleName),translator.GetTranslation("weather", moduleName),translator.GetTranslation("words", moduleName),translator.GetTranslation("past", moduleName),translator.GetTranslation("solitary", moduleName)},
                {translator.GetTranslation("relax", moduleName),translator.GetTranslation("dance", moduleName),translator.GetTranslation("weightless", moduleName),translator.GetTranslation("morality", moduleName),translator.GetTranslation("gaze", moduleName),translator.GetTranslation("failure", moduleName)},
                {translator.GetTranslation("bunny", moduleName),translator.GetTranslation("lovely", moduleName),translator.GetTranslation("romance", moduleName),translator.GetTranslation("future", moduleName),translator.GetTranslation("focus", moduleName),translator.GetTranslation("search", moduleName)},
                {"Hana",translator.GetTranslation("cookies", moduleName),translator.GetTranslation("compassion", moduleName),translator.GetTranslation("creation", moduleName),translator.GetTranslation("patience", moduleName),"Lacy"}
            };

            if (wordTable.Cast<string>().Any(v => v.Length == 0)) return;
            ReflectionHelper.SetValue(componentType, "wordTable", wordTable, poetryModule);
        }

        public static Translator translator = null;
    }
}
