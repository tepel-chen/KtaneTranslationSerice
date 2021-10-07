
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TranslationService 
{ 

    class TranslationService: MonoBehaviour
    {

        [SerializeField]
        private List<string> fontLang;
        [SerializeField]
        private List<Font> fontFont;
        [SerializeField]
        private List<Material> fontMaterial;

        internal Dictionary<string, Font> fonts;
        internal Dictionary<string, Material> fontMaterials;

        protected void Awake()
        {
            fonts = Enumerable.Range(0, fontLang.Count).ToDictionary(i => fontLang[i], i => fontFont[i]);
            fontMaterials = Enumerable.Range(0, fontLang.Count).ToDictionary(i => fontLang[i], i => fontMaterial[i]);
            Patcher.Patch();
            Patcher.service = this;
            StartCoroutine(FileWritePatch.SetLanguageCode());
        }
        
    }
}
