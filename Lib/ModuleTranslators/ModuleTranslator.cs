using HarmonyLib;
using System.Collections;

namespace TranslationService.ModuleTranslators
{
    abstract class ModuleTranslator
    {
        public abstract void StartTranslation(KMBombModule module, Translator translator);
    }
}
