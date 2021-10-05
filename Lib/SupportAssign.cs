#nullable enable

using HarmonyLib;
using KeepCoding;
using System;
using System.Collections;
using System.Collections.Generic;
using TranslationService.Loaders;
using TranslationService.ModuleTranslators;
using UnityEngine;
using static TranslationService.Magnifier;

namespace TranslationService
{
    class SupportAssign : CustomYieldInstruction
    {
        private readonly SupportLoader loader;
        private readonly Translator translator;
        private readonly Settings settings;
        private readonly ILog logger;
        private readonly Harmony harmony;
        private readonly Dictionary<string, ModuleTranslator> moduleTranslatorCache = new Dictionary<string, ModuleTranslator>();
        public SupportAssign(ILog logger, Translator translator, Settings settings, Harmony harmony)
        {
            loader = new SupportLoader(logger);
            this.logger = logger;
            this.translator = translator;
            this.settings = settings;
            this.harmony = harmony;
        }

        public override bool keepWaiting => loader.keepWaiting;

        public void TranslateModule(KMBombModule module)
        {
            var support = loader.GetSupport(module.ModuleDisplayName);
            if (support != null)
            {
                ModuleTranslator? moduleTranslator = moduleTranslatorCache.TryGetValue(module.ModuleDisplayName, out ModuleTranslator cache) ? cache : null;
                moduleTranslator ??= (support.status) switch
                {
                    ModuleSupportStatus.Default => new OnlyStartModuleTranslator(Default),
                    ModuleSupportStatus.Adjust => new OnlyStartModuleTranslator(new SizeLimitMagnifier(support.wAdjust ?? 1f, support.hAdjust ?? 1f)),
                    ModuleSupportStatus.Custom => CustomDictionary.TryGetValue(module.ModuleDisplayName, out Func<Harmony, ModuleTranslator> t) ? t(harmony) : null,
                    _ => settings.ApplyToUntestedModule ? new OnlyStartModuleTranslator(Default) : null,
                };
                if(moduleTranslator == null && support.status == ModuleSupportStatus.Custom)
                {
                    logger.Log($"Module {module.ModuleDisplayName} is labeled custom, but custom translator not found");
                }
                if(moduleTranslator != null)
                {
                    if(!moduleTranslatorCache.ContainsKey(module.ModuleDisplayName)) moduleTranslatorCache.Add(module.ModuleDisplayName, moduleTranslator);
                    moduleTranslator.StartTranslation(module, translator);
                }
            }
        }


        public readonly static Dictionary<string, Func<Harmony, ModuleTranslator>> CustomDictionary = new Dictionary<string, Func<Harmony, ModuleTranslator>>() {
            {"Orientation Cube", harmony => new OnlyStartModuleTranslator(new OrientationCubeMagnifier()) },
            {"Adventure Game", harmony => new AdventureGameTranslator(harmony) }
        };
    }
}
