#nullable enable

using System.Collections;
using UnityEngine;

namespace TranslationService.ModuleTranslators
{
    class OnlyStartModuleTranslator : ModuleTranslator
    {

        private readonly Magnifier magnifier;
        private readonly Translator? translator;
        public OnlyStartModuleTranslator(Magnifier magnifier)
        {
            this.magnifier = magnifier;
        }

        public override IEnumerator StartTranslation(KMBombModule module, Translator translator)
        {
            yield return translator;
            var texts = module.GetComponentsInChildren<TextMesh>();
            translator.SetTranslationToMeshes(texts, module.ModuleDisplayName, magnifier);
        }
    }
}
