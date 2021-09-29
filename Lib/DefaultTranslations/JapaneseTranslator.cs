#nullable enable

using KeepCoding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TranslationService
{
    public class JapaneseTranslator : DefaultTranslator
    {
        public JapaneseTranslator(ILog logger, Font? font, Material? fontmaterial, Settings settings) : base(logger, font, fontmaterial, settings) { }

        public override Dictionary<string, string> GetDefaultDict()
        {
            return new Dictionary<string, string>()
                {
                    {"submit", "送信" },
                    {"sub", "送信" },
                    {"check", "確認" },
                    {"clear", "クリア" },
                    {"clr", "クリア" },
                    {"reset", "リセット" },
                    {"query", "クエリ" },
                    {"right", "右" },
                    {"left", "左" },
                    {"up", "上" },
                    {"down", "下" },
                    {"play", "再生" },
                    {"record", "録音" },
                    // Orientation Cube
                    {"OrientationCube:top", "上" },
                    {"OrientationCube:front", "前" },
                    {"OrientationCube:back", "後" },
                    {"OrientationCube:bottom", "下" },
                    {"OrientationCube:set", "セット"},
                    // Minesweeper
                    {"MinesweeperModule:digging", "掘削" },
                    {"MinesweeperModule:flagging", "フラグ" },
                    {"MinesweeperModule:mode", "モード" },
                    // Neutralization
                    {"neutralization:filter", "フィルタ" },
                    {"neutralization:titrate", "滴定" },
                    // Murder
                    {"murder:accuse", "告訴" },
                    {"notMurder:accuse", "告訴" },
                    // A message
                    {"AMessage:send", "送信" },
                    {"AMessage:submit", "提出" },
                    // Four card monte
                    {"Krit4CardMonte:deal", "開始" },
                    // The Swan
                    {"theSwan:execute", "実行" }
                };
        }
    }
}
