using UnityEngine;
using System.Collections.Generic;
using System;

namespace Require
{
    public static class Require
    {
        public static T Require<T>(this Transform transform) where T : Component
        {
            var component = transform.GetComponent<T>();

            if (component == null)
            {
                return transform.gameObject.AddComponent<T>();
            }
            else
            {
                return component;
            }
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

		public static Transform GetModuleSuper(this Transform transform)
		{
			if (transform.parent == null)
			{
				throw new Exception("Trying to get module super for a module that has no super");
			}

			Transform super = transform.parent.GetModuleRoot();

			if (super.GetComponent<ModuleRoot>() == null)
			{
				throw new Exception("Trying to get module super for a module that has no super");
			}

			return super;
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

        public static Transform RequireTag(this Transform transform, string tag)
        {
            if (transform.tag == tag)
            {
                return transform;
            }

            return RequireTag(tag);
        }

        public static Transform RequireTag(string tag)
        {
            GameObject instance = GameObject.FindGameObjectWithTag(tag);

            if (instance == null)
            {
                instance = new GameObject(tag);
                instance.tag = tag;
            }

            return instance.transform;
        }
    }
}
