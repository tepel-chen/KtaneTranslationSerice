#nullable enable

using KeepCoding;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TranslationService.Loaders
{
    public class TranslationLoader: CustomYieldInstruction
    {
        private readonly GoogleSheet sheet;
        private readonly ILog logger;

        public override bool keepWaiting => sheet.keepWaiting;

        public TranslationLoader(ILog logger)
        {
            sheet = new GoogleSheet("1isY3ScLImQOZ5BfpebYzKvUeCoqh8hjKV33T05flKhM", "translation");
            this.logger = logger;
        }


        public Dictionary<string, string>? GetTranslation(string code)
        {
            if(sheet.Success)
            {
                var rows = sheet.GetRows();
                var result = new Dictionary<string, string>();
                foreach (var row in rows)
                {
                    Debug.Log(row.Keys.Count);
                    if(
                        row.TryGetValue(code, out string translation) && translation.Length > 0 &&
                        row.TryGetValue("Field name", out string field) && field.Length > 0
                    )
                    {
                        var key = 
                            row.TryGetValue("Module name", out string module) && module.Length > 0? 
                            $"{module}:{field.ToLowerInvariant()}" : 
                            field.ToLowerInvariant();
                        result.Add(key, translation);
                    }
                }
                return result;
            }
            logger.Log("Loading translation sheet was unsuccessful");
            return null;
        }
    }
}
