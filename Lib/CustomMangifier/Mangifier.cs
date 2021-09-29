using KeepCoding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TranslationService
{
    public class Magnifier
    {
        public virtual float GetMagnifier(Vector3 beforeBounds, Vector3 afterBounds, string text)
        {
            var x = beforeBounds.x < beforeBounds.y && beforeBounds.x < beforeBounds.z ? 1 : beforeBounds.x / afterBounds.x;
            var y = beforeBounds.y < beforeBounds.z && beforeBounds.y < beforeBounds.x ? 1 : beforeBounds.y / afterBounds.y;
            var z = beforeBounds.z < beforeBounds.y && beforeBounds.z < beforeBounds.x ? 1 : beforeBounds.z / afterBounds.z;
            return new float[] { x, y, z }.Min();
        }

        public class StaticMagnifier: Magnifier
        {
            public override float GetMagnifier(Vector3 beforeBounds, Vector3 afterBounds, string text)
            {
                return 1f;
            }
        }

        public class MaxSizeMagnifier: Magnifier
        {
            Vector3 size;
            public MaxSizeMagnifier(float x, float y, float z) { size = new Vector3(x, y, z); }
            public MaxSizeMagnifier(Vector3 size) { this.size = size; }
            public override float GetMagnifier(Vector3 beforeBounds, Vector3 afterBounds, string text)
            {
                return base.GetMagnifier(size, afterBounds, text);
            }
        }

        public class HightLimitMagnifier: Magnifier
        {
            float maxHeight;
            public HightLimitMagnifier(float maxHeight) { this.maxHeight = maxHeight; }
            public override float GetMagnifier(Vector3 b, Vector3 afterBounds, string text)
            {
                Vector3 v;
                if((b.x < b.y && b.x > b.z) || (b.x < b.z && b.x > b.y))
                {
                    v = new Vector3(b.x * maxHeight, b.y, b.z);
                } else if((b.y < b.x && b.y > b.z) || (b.y < b.z && b.y > b.x))
                {
                    v = new Vector3(b.x, b.y * maxHeight, b.z);
                } else
                {
                    v = new Vector3(b.x, b.y, b.z * maxHeight);
                }
                return base.GetMagnifier(v, afterBounds, text);
            }
        }

        public class WidthLimitMagnifier: Magnifier
        {
            float maxWidth;
            public WidthLimitMagnifier(float maxWidth) { this.maxWidth = maxWidth; }
            public override float GetMagnifier(Vector3 b, Vector3 afterBounds, string text)
            {
                Vector3 v;
                if (b.x > b.y && b.x > b.z)
                {
                    v = new Vector3(b.x * maxWidth, b.y, b.z);
                }
                else if (b.y > b.x && b.y > b.z)
                {
                    v = new Vector3(b.x, b.y * maxWidth, b.z);
                }
                else
                {
                    v = new Vector3(b.x, b.y, b.z * maxWidth);
                }
                return base.GetMagnifier(v, afterBounds, text);
            }
        }

        public class SizeLimitMagnifier: Magnifier
        {
            float maxHeight;
            float maxWidth;
            public SizeLimitMagnifier(float maxWidth, float maxHeight) { 
                this.maxWidth = maxWidth;
                this.maxHeight = maxHeight;
            }

            public override float GetMagnifier(Vector3 b, Vector3 afterBounds, string text)
            {
                Vector3 v;
                if (b.x > b.y && b.x > b.z)
                {
                    if(b.y > b.z)
                    {
                        v = new Vector3(b.x * maxWidth, b.y * maxHeight, b.z);
                    } else
                    {
                        v = new Vector3(b.x * maxWidth, b.y , b.z * maxHeight);
                    }
                }
                else if (b.y > b.x && b.y > b.z)
                {
                    if (b.x > b.z)
                    {
                        v = new Vector3(b.x * maxHeight, b.y * maxWidth, b.z);
                    }
                    else
                    {
                        v = new Vector3(b.x, b.y * maxWidth, b.z * maxHeight);
                    }
                }
                else
                {
                    if (b.x > b.y)
                    {
                        v = new Vector3(b.x * maxHeight, b.y, b.z * maxWidth);
                    }
                    else
                    {
                        v = new Vector3(b.x, b.y * maxHeight, b.z * maxWidth);
                    }
                }
                return base.GetMagnifier(v, afterBounds, text);
            }


        }

        public static Magnifier Default = new Magnifier();
        public static Magnifier Static = new StaticMagnifier();
    }

}
