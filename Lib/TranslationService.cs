
using KeepCoding;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        private TranslationServiceProperties properties;

        internal readonly KeepCoding.Logger logger = new KeepCoding.Logger("Translation Service");

        protected void Awake()
        {
            fonts = Enumerable.Range(0, fontLang.Count).ToDictionary(i => fontLang[i], i => fontFont[i]);
            fontMaterials = Enumerable.Range(0, fontLang.Count).ToDictionary(i => fontLang[i], i => fontMaterial[i]);
            Patcher.Patch();
            Patcher.service = this;
            if (!Application.isEditor)
            { 
                logger.Log($"Using version: {(ModManager.Instance.InstalledModInfos.Values.First(info => info.ID == "TranslationService").Version)}");
            }
            var propertyObject = new GameObject("TranslationServiceProperties");
            propertyObject.transform.parent = gameObject.transform;
            properties = propertyObject.AddComponent<TranslationServiceProperties>();
            StartCoroutine(FileWritePatch.SetLanguageCode());
        }

        public void SetDict(Dictionary<string, string> dict)
        {
            properties.isLoading = false;
            properties.translationDict = dict;
        }
        
    }
}
