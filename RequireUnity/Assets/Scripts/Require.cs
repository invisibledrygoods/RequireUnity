using UnityEngine;
using System.Collections.Generic;

namespace Require
{
    public static class TransformExtension
    {
        public static T Require<T>(this Transform transform) where T : Component
        {
            return transform.GetComponent<T>() ?? transform.gameObject.AddComponent<T>();
        }

        public static Transform GetModuleRoot(this Transform transform)
        {
            Transform parent = transform;

            while (parent != null)
            {
                if (parent.GetComponent<ModuleRoot>() != null)
                {
                    return parent;
                }

                parent = parent.parent;
            }

            return transform;
        }

        public static T[] GetComponentsInModule<T>(this Transform transform) where T : Component
        {
            Transform root = GetModuleRoot(transform);
            List<T> components = new List<T>();

            foreach (T component in root.GetComponentsInChildren<T>())
            {
                if (component.transform.GetModuleRoot() == root)
                {
                    components.Add(component);
                }
            }

            return components.ToArray();
        }
    }
}
