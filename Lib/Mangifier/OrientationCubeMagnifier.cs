
using UnityEngine;

namespace TranslationService
{
    public class OrientationCubeMagnifier: Magnifier
    {
        public override float GetMagnifier(Vector3 beforeBounds, Vector3 afterBounds, string text)
        {
            switch (text) {
                case "TOP":
                    return base.GetMagnifier(new Vector3(0.02f, 0, 0.02f), afterBounds, text);
                case "RIGHT":
                case "LEFT":
                    return base.GetMagnifier(new Vector3(0, 0.02f, 0.02f), afterBounds, text);
                case "BACK":
                case "FRONT":
                    return base.GetMagnifier(new Vector3(0.02f, 0.02f, 0), afterBounds, text);
                default:
                    return base.GetMagnifier(beforeBounds, afterBounds, text);
            }

        }
    }
}
