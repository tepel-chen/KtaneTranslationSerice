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
        internal IDictionary<string, Material> materials;
        internal IDictionary<string, Font> fonts;
        public TranslationServiceProperties()
        {
            AddProperty("IsLoading", new Property(() => isLoading, null));
            AddProperty("TranslationDict", new Property(() => translationDict, null));
            AddProperty("Materials", new Property(() => materials, null));
            AddProperty("Fonts", new Property(() => fonts, null));
        }
    }
}
