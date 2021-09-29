using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TranslationService
{
    public class OrientationCubeMagnifier: Magnifier
    {
        public override float GetMagnifier(Vector3 beforeBounds, Vector3 afterBounds, string text)
        {
            switch (text) {
                case "TOP":
                case "RIGHT":
                case "LEFT":
                case "BACK":
                case "FRONT":
                    return base.GetMagnifier(new Vector3(0.007f, 0.007f, 0.007f), afterBounds, text);
                default:
                    return base.GetMagnifier(beforeBounds, afterBounds, text);
            }

        }
    }
}
