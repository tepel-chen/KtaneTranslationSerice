
using UnityEngine;

namespace TranslationService
{
    public class OrientationCubeMagnifier: Magnifier
    {
        public override float GetMagnifier(Vector2 beforeBounds, Vector2 afterBounds, string text)
        {
            switch (text) {
                case "TOP":
                case "RIGHT":
                case "LEFT":
                case "BACK":
                case "FRONT":
                    return base.GetMagnifier(new Vector2(0.02f, 0.02f), afterBounds, text);
                default:
                    return base.GetMagnifier(beforeBounds, afterBounds, text);
            }

        }
    }
}
