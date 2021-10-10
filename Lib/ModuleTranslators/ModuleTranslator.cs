using HarmonyLib;
using System.Collections;

namespace TranslationService.ModuleTranslators
{
    abstract class ModuleTranslator
    {
        public virtual void StartTranslation(KMBombModule module, Translator translator) { }
        public virtual void AwakeTranslation(KMBombModule module, Translator translator) { }
    }
}
