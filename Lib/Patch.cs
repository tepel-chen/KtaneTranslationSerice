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
        internal static Translator translator;
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
        public static IEnumerator AwakeTranslation(BombComponent __instance)
        {
            if (__instance.ComponentType == Assets.Scripts.Missions.ComponentTypeEnum.Mod && Patcher.assigner != null)
            {
                yield return Patcher.assigner;
                KMBombModule module = __instance.GetComponentInChildren<KMBombModule>();
                if (module == null)
                {
                    Debug.Log("[Translator Service] Module not found. Please send a bug report to tepel#5876");
                }
                Patcher.assigner.TranslateModule(module, true);
            }
            else
            {
                yield return null;
            }
        }
        public static void Postfix(BombComponent __instance)
        {
            __instance.StartCoroutine(AwakeTranslation(__instance));
        }
    }

    [HarmonyPatch(typeof(File), "WriteAllText", new Type[] { typeof(string), typeof(string) })]
    public class FileWritePatch
    {
        private static string currentLangCode;
        private static bool isSetting = false;
        internal static IEnumerator SetLanguageCode()
        {
            if (isSetting) yield break;
            var logger = Patcher.service.logger;
            isSetting = true;
            var settings = new ModConfig<Settings>().Read();
            if (settings.LanguageCodeOverride is string langOverride && langOverride.Length > 0)
            {
                logger.Log($"Current language: {Game.PlayerSettings.LanguageCode}");
                logger.Log($"Overriding to language: {langOverride}");
                currentLangCode = langOverride;
            }
            else
            {
                logger.Log($"Current language: {Game.PlayerSettings.LanguageCode}");
                currentLangCode = Game.PlayerSettings.LanguageCode;
            }
            var font = Patcher.service.fonts.TryGetValue(currentLangCode, out Font fontRes) ? fontRes : null;
            var fontMaterial = Patcher.service.fontMaterials.TryGetValue(currentLangCode, out Material matRes) ? matRes : null;
            Patcher.translator = new Translator(logger, font, fontMaterial, settings, currentLangCode);
            Patcher.assigner = new SupportAssign(logger, Patcher.translator, settings, Patcher.harmony);
            yield return Patcher.translator;
            yield return Patcher.assigner;
            Patcher.service.SetDict(Patcher.translator.LoadTranslations());
            isSetting = false;
        }

        public static void Postfix(string path, string contents)
        {
            if (path == Path.Combine(Path.Combine(Application.persistentDataPath, "Modsettings"), "TranslationService-settings.json"))
            {
                var gen = SetLanguageCode();
                while (gen.MoveNext()) { }
            }
        }
    }
}
