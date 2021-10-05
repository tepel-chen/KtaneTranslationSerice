#nullable enable

using System.Collections;
using UnityEngine;

namespace TranslationService.ModuleTranslators
{
    class OnlyStartModuleTranslator : ModuleTranslator
    {

        private readonly Magnifier magnifier;
        public OnlyStartModuleTranslator(Magnifier magnifier)
        {
            this.magnifier = magnifier;
        }

        public override void StartTranslation(KMBombModule module, Translator translator)
        {
            var texts = module.GetComponentsInChildren<TextMesh>();
            translator.SetTranslationToMeshes(texts, module, magnifier);
        }
    }
}
