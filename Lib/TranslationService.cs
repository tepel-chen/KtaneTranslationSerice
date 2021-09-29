
using KeepCoding;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using static TranslationService.Magnifier;

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
        DefaultTranslator translator;
        KeepCoding.Logger logger;
        Font font;
        Material fontMaterial;
        Settings settings;

        protected void Start()
        {
            bombInfo = GetComponent<KMBombInfo>();
            logger = new KeepCoding.Logger("Translation Service");
        }

        private void SetLanguageCode(string code)
        {
            font = code switch
            {
                "ja" => jaFont,
                _ => null
            };
            fontMaterial = code switch
            {
                "ja" => jaFontMaterial,
                _ => null
            };
            translator = DefaultTranslator.GetTranslatorFromCode(code, logger, font, fontMaterial, settings);
        }

        protected void Update()
        {
            if (needsTranslationUpdate && bombInfo.IsBombPresent() && SceneManager.Instance.GameplayState.Bombs.Count > 0)
            {
                settings = new ModConfig<Settings>().Read();


                logger.Log($"Current language: {Game.PlayerSettings.LanguageCode}");
                if (settings.LanguageCodeOverride is string langOverride && langOverride.Length > 0)
                {
                    logger.Log($"Overriding to language: {langOverride}");
                    SetLanguageCode(langOverride);
                }
                else
                {
                    SetLanguageCode(Game.PlayerSettings.LanguageCode);
                }
                if (translator != null) TranslateModules();
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
                StartCoroutine(TranslateModule(module));
            }
            
        }

        private IEnumerator TranslateModule(KMBombModule module)
        {
            yield return null;
            var texts = module.GetComponentsInChildren<TextMesh>();
            switch (module.ModuleType)
            {
                // Custom translator dict
                case "OrientationCube":
                    yield return translator.SetTranslationToMeshes(texts, module.ModuleType, new OrientationCubeMagnifier());
                    break;
                // Adjust magnifier
                case "GameOfLifeSimple":
                case "GameOfLifeCruel":
                case "wackGameOfLife":
                case "LifeIteration":
                case "Krit4CardMonte":
                    yield return translator.SetTranslationToMeshes(texts, module.ModuleType, new SizeLimitMagnifier(1.1f, 1.1f));
                    break;
                case "AdjacentLettersModule":
                case "SkewedSlotsModule":
                case "fastMath":
                case "algebra":
                case "TwoBits":
                case "neutralization":
                case "MinesweeperModule":
                case "murder":
                case "notMurder":
                case "NonogramModule":
                case "Backgrounds":
                case "BlindMaze":
                    yield return translator.SetTranslationToMeshes(texts, module.ModuleType, new HightLimitMagnifier(1.2f));
                    break;
                case "Logic":
                case "symbolicPasswordModule":
                case "graphModule":
                case "MazeV2":
                case "resistors":
                case "dragonEnergy":
                case "timezone":
                case "curriculum":
                    yield return translator.SetTranslationToMeshes(texts, module.ModuleType, new HightLimitMagnifier(1.3f));
                    break;
                case "fizzBuzzModule":
                    yield return translator.SetTranslationToMeshes(texts, module.ModuleType, new HightLimitMagnifier(1.5f));
                    break;
                case "CheapCheckoutModule":
                case "cheepCheckout":
                case "radiator":
                case "FlagsModule":
                case "mashematics":
                case "BitOps":
                case "PolyhedralMazeModule":
                case "combinationLock":
                case "theSwan":
                    yield return translator.SetTranslationToMeshes(texts, module.ModuleType, Default);
                    break;
                default:
                    if(settings.ApplyToUntestedModule) yield return translator.SetTranslationToMeshes(texts, module.ModuleType, Default);
                    break;
            }
        }

    }
}
