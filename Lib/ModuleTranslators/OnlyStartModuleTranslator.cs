#nullable enable

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TranslationService.ModuleTranslators
{
    class OnlyStartModuleTranslator : ModuleTranslator
    {

        private readonly Magnifier magnifier;
        private readonly List<string>? categories;
        public OnlyStartModuleTranslator(Magnifier magnifier, List<string>? categories)
        {
            this.magnifier = magnifier;
            this.categories = categories;
        }

        public override void StartTranslation(KMBombModule module, Translator translator)
        {
            var texts = module.GetComponentsInChildren<TextMesh>();
            translator.SetTranslationToMeshes(texts, module, magnifier, categories);
        }

        public override void AwakeTranslation(KMBombModule module, Translator translator)
        {
            var texts = module.GetComponentsInChildren<TextMesh>();
            translator.SetTranslationToMeshes(texts, module, magnifier, categories);
        }
    }
}
