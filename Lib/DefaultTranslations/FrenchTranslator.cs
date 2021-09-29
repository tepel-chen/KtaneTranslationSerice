#nullable enable 

using KeepCoding;
using System.Collections.Generic;
using UnityEngine;

namespace TranslationService
{
    public class FrenchTranslator : DefaultTranslator
    {
        public FrenchTranslator(ILog logger, Font? font, Material? fontmaterial, Settings settings) : base(logger, font, fontmaterial, settings) { }

        private static Dictionary<string, string> _dict = new Dictionary<string, string>()
        {
            {"submit", "soumettre" },
            {"set", "placer"},
            {"check", "vérifier" },
            {"clear", "nettoyer" },
            {"reset", "remettre à zéro" }
        };

        public override Dictionary<string, string> GetDefaultDict()
        {
            return _dict;
        }
    }
}
