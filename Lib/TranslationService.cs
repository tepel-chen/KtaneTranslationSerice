
using HarmonyLib;
using KeepCoding;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace TranslationService
{
    class TranslationService: MonoBehaviour
    {
        [SerializeField]
        internal Font jaFont;
        [SerializeField]
        internal Material jaFontMaterial;

        protected void Awake()
        {
            Patcher.Patch();
            Patcher.service = this;
            StartCoroutine(FileWritePatch.SetLanguageCode());
        }

    }
}
