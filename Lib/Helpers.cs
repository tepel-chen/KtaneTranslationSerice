using KeepCoding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace TranslationService
{
    public static class Helpers
    {
        public static void ShowChildTree(ILog log, Transform transform, int depth = 5, int indent = 0)
        {
            if(indent > 0)
            {
                log.Log(new string('-', indent * 2) + transform.name);
            } else
            {
                log.Log(transform.name);
            }
            if (depth == 0) return;
            foreach(Transform child in transform.GetComponentsInChildren<Transform>())
            {
                ShowChildTree(log, child, depth - 1, indent + 1);
            }
        }

        public static void ShowCurrentSize(ILog logger, KMBombModule module)
        {
            var meshes = module.GetComponentsInChildren<TextMesh>();
            foreach(var mesh in meshes)
            {
                logger.Log($"Found text \"{mesh.text}\" in module {module.ModuleType}, size are {mesh.GetComponent<MeshRenderer>().bounds.size.ToString("F5")}");
            }
        }
    }
}
