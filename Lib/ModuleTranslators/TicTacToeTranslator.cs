using System;
using System.Collections;
using UnityEngine;
using HarmonyLib;
using System.Reflection;
using System.Linq;

namespace TranslationService.ModuleTranslators
{
    class TicTacToeTranslator : ModuleTranslator
    {
        public TicTacToeTranslator()
        {
        }

        private static Magnifier displayMagnifier(string langCode) => langCode == "ja" ? new Magnifier.VectorMagnifier(1f, 0.026812f) : new Magnifier.VectorMagnifier(1f, 0.017875f);

        public override void StartTranslation(KMBombModule module, Translator translator)
        {
            TicTacToeTranslator.translator = translator;
        }

        public override void AwakeTranslation(KMBombModule module, Translator translator)
        {
            // p = "PASS" ; u = "UP NEXT:"
            var texts = module.GetComponentsInChildren<TextMesh>().Where(text => text.text == "u" || text.text == "p").ToArray();
            Font font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            translator.SetTranslationToMeshes(texts, module, displayMagnifier(translator.langCode), font, true);
        }

        public static Translator translator = null;
    }
}
