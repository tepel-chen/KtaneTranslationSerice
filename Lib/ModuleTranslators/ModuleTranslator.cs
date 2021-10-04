using System.Collections;

namespace TranslationService.ModuleTranslators
{
    abstract class ModuleTranslator
    {
        public abstract IEnumerator StartTranslation(KMBombModule module, Translator translator);
    }
}
