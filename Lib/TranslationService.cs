
using HarmonyLib;
using KeepCoding;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace TranslationService
{
    class TranslationService: MonoBehaviour
    {
        [SerializeField]
        Font jaFont;
        [SerializeField]
        Material jaFontMaterial;

        bool needsTranslationUpdate = true;
        KMBombInfo bombInfo;
        Translator translator;
        SupportAssign assigner;
        KeepCoding.Logger logger;
        Font font;
        Material fontMaterial;
        Settings settings;
        string currentLangCode;
        Harmony harmony;

        protected void Start()
        {
            bombInfo = GetComponent<KMBombInfo>();
            harmony = new Harmony("tepel.translationService");
            logger = new KeepCoding.Logger("Translation Service");
            settings = new ModConfig<Settings>().Read();
            StartCoroutine(SetLanguageCode());
        }

        private IEnumerator SetLanguageCode()
        {
            if (settings.LanguageCodeOverride is string langOverride && langOverride.Length > 0)
            {
                if (currentLangCode == langOverride) yield break;
                logger.Log($"Current language: {Game.PlayerSettings.LanguageCode}");
                logger.Log($"Overriding to language: {langOverride}");
                currentLangCode = langOverride;
            }
            else
            {
                if (currentLangCode == Game.PlayerSettings.LanguageCode) yield break;
                logger.Log($"Current language: {Game.PlayerSettings.LanguageCode}");
                currentLangCode = Game.PlayerSettings.LanguageCode;
            }
            font = currentLangCode switch
            {
                "ja" => jaFont,
                _ => null
            };
            fontMaterial = currentLangCode switch
            {
                "ja" => jaFontMaterial,
                _ => null
            };
            translator = new Translator(logger, font, fontMaterial, settings, currentLangCode);
            assigner = new SupportAssign(logger, translator, settings, harmony);
            while (translator.keepWaiting || assigner.keepWaiting) yield return null;
        }

        protected void Update()
        {
            if (needsTranslationUpdate && bombInfo.IsBombPresent() && SceneManager.Instance.GameplayState.Bombs.Count > 0)
            {
                settings = new ModConfig<Settings>().Read();

                StartCoroutine(SetLanguageCode());
                TranslateModules();
                needsTranslationUpdate = false;
            } else if(!bombInfo.IsBombPresent())
            {
                needsTranslationUpdate = true;
            }
        }

        private void TranslateModules()
        {
            logger.Log("Start translating modules");
            var modules = SceneManager.Instance.GameplayState.Bombs[0].GetComponentsInChildren<KMBombModule>();
            logger.Log($"Found: {modules.Length} modules");
            foreach (var module in modules)
            {
                StartCoroutine(assigner.TranslateModule(module));
            }
            
        }

    }
}
