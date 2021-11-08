#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TranslationService
{
    class TranslationServiceProperties: PropertiesBehaviour
    {
        internal  bool isLoading = true;
        internal IDictionary<string, string>? translationDict;
        internal Material? fontMaterial;
        internal Font? font;
        internal string? langCode;
        public TranslationServiceProperties()
        {
            AddProperty("IsLoading", new Property(() => isLoading, null));
            AddProperty("TranslationDict", new Property(() => translationDict, null));
            AddProperty("FontMaterial", new Property(() => fontMaterial, null));
            AddProperty("Font", new Property(() => font, null));
            AddProperty("LangCode", new Property(() => langCode, null));
        }
    }
}
