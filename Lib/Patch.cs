using HarmonyLib;
using KeepCoding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TranslationService
{
    public static class Patcher
    {
        internal static Harmony harmony;
        internal static SupportAssign assigner;
        internal static TranslationService service;

        public static void Patch()
        {
            if (harmony != null) return;
            Harmony.DEBUG = true;
            harmony = new Harmony("tepel.translationService");
            harmony.PatchAll();
            return;
        }
    }
    [HarmonyPatch(typeof(BombComponent), "Start")]
    public class StartPatch
    {
        public static IEnumerator StartTranslation(BombComponent __instance)
        {
            if (__instance.ComponentType == Assets.Scripts.Missions.ComponentTypeEnum.Mod && Patcher.assigner != null)
            {
                yield return Patcher.assigner;
                KMBombModule module = __instance.GetComponentInChildren<KMBombModule>();
                if (module == null)
                {
                    Debug.Log("[Translator Service] Module not found. Please send a bug report to tepel#5876");
                }
                Patcher.assigner.TranslateModule(module);
            } else
            {
                yield return null;
            }
        }
        public static void Postfix(BombComponent __instance)
        {
            __instance.StartCoroutine(StartTranslation(__instance));
        }
    }
    [HarmonyPatch(typeof(BombComponent), "Activate")]
    public class ActivatePatch
    {
        public static void Postfix(BombComponent __instance)
        {
            __instance.StartCoroutine(StartPatch.StartTranslation(__instance));
        }
    }

    [HarmonyPatch(typeof(File), "WriteAllText", new Type[] { typeof(string), typeof(string) })]
    public class FileWritePatch
    {
        private static string currentLangCode;
        private static bool isSetting = false;
        private static readonly KeepCoding.Logger logger = new KeepCoding.Logger("Translation Service");
        internal static IEnumerator SetLanguageCode()
        {
            if (isSetting) yield break;
            isSetting = true;
            Time.timeScale = 0;
            var settings = new ModConfig<Settings>().Read();
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
            var font = currentLangCode switch
            {
                "ja" => Patcher.service.jaFont,
                _ => null
            };
            var fontMaterial = currentLangCode switch
            {
                "ja" => Patcher.service.jaFontMaterial,
                _ => null
            };
            var translator = new Translator(logger, font, fontMaterial, settings, currentLangCode);
            Patcher.assigner = new SupportAssign(logger, translator, settings, Patcher.harmony);
            yield return translator;
            yield return Patcher.assigner;
            isSetting = false;
            Time.timeScale = 1;
        }

        public static void Postfix(string path, string contents)
        {
            if (path == Path.Combine(Path.Combine(Application.persistentDataPath, "Modsettings"), "TranslationService-settings.json"))
            {
                Patcher.service.StartCoroutine(SetLanguageCode());
            }
        }
    }
}
