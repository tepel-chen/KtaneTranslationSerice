#nullable enable

using KeepCoding;
using System.Collections.Generic;
using UnityEngine;

namespace TranslationService.Loaders
{
    public enum ModuleSupportStatus
    {
        Unsupported = -1,
        Default,
        Adjust,
        Partial,
        PartialAdjust,
        Custom,
        Native
    }
    public class ModuleSupport
    {
        public readonly string moduleName;
        public readonly ModuleSupportStatus status;
        public readonly float? wAdjust;
        public readonly float? hAdjust;

        public ModuleSupport(string moduleName, ModuleSupportStatus status, float? wAdjust, float? hAdjust) {
            this.moduleName = moduleName;
            this.status = status;
            this.wAdjust = wAdjust;
            this.hAdjust = hAdjust;
        }
    }

    public class SupportLoader : CustomYieldInstruction
    {
        private readonly GoogleSheet sheet;
        private readonly ILog logger;
        private Dictionary<string, ModuleSupport>? supportList;


        public override bool keepWaiting => sheet.keepWaiting;

        public SupportLoader(ILog logger)
        {
            sheet = new GoogleSheet("1isY3ScLImQOZ5BfpebYzKvUeCoqh8hjKV33T05flKhM","support", "1183533099");
            this.logger = logger;
        }


        public ModuleSupport? GetSupport(string moduleName)
        {
            if (supportList != null && supportList.TryGetValue(moduleName, out ModuleSupport support)) return support;

            if (sheet.Success)
            {
                supportList = new Dictionary<string, ModuleSupport>();
                var rows = sheet.GetRows();
                foreach (var row in rows)
                {
                    if(row.TryGetValue("Module name", out string name) && row.TryGetValue("Support info", out string info))
                    {
                        var status = info switch
                        {
                            "Default" => ModuleSupportStatus.Default,
                            "Adjust" => ModuleSupportStatus.Adjust,
                            "Custom" => ModuleSupportStatus.Custom,
                            "Partial" => ModuleSupportStatus.Partial,
                            "Partial Adjust" => ModuleSupportStatus.PartialAdjust,
                            _ => ModuleSupportStatus.Unsupported
                        };
                        float? wAdjust = row.TryGetValue("Width adjustment", out string wstr) && float.TryParse(wstr, out float wvalue) ? wvalue : null;
                        float? hAdjust = row.TryGetValue("Height adjustment", out string hstr) && float.TryParse(hstr, out float hvalue) ? hvalue : null;
                        supportList.Add(name, new ModuleSupport(name, status, wAdjust, hAdjust));
                    }
                }

                return supportList.TryGetValue(moduleName, out ModuleSupport support1) ? support1 : null;
            }
            logger.Log("Loading support sheet was unsuccessful");
            return null;
        }
    }
}
