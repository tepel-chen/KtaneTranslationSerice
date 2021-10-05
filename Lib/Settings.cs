#nullable enable

using Newtonsoft.Json;

namespace TranslationService { 
    public class Settings
    {
        [JsonProperty("Overrides language setting")]
        public string LanguageCodeOverride = "";
        [JsonProperty("Suggests untranslated text from module in log")]
        public bool EnableSuggestionLog = false;
        [JsonProperty("Apply to partial supports")]
        public bool ApplyToPartial = false;
        [JsonProperty("Apply default translation to untested modules. May cause some problems.")]
        public bool ApplyToUntestedModule = false;
    }
}
