using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Framework
{
    static class ListEx
    {
        /// <summary>
        /// Returns true if the index is within range of the list. Always returns false is the list is empty.
        /// </summary>
        public static bool InRange<T>(this IList<T> list, int index)
        {
            if (list.Count == 0)
            {
                return false;
            }
            else
            {
                return index >= 0 && index <= list.Count - 1;
            }
        }
    }

    static class Vector3Ex
    {
        public static Vector2 ToVec2(this Vector3 v)
        {
            return new Vector2(v.x, v.y);
        }
    }

    static class ObjectEx
    {
        public static T FindObjectOfType<T>(Action<T> onFind = null) where T : MonoBehaviour
        {
            var obj = GameObject.FindObjectOfType<T>();
            Assert.IsNotNull(obj, string.Format("A GameObject with the {0} component must exist somewhere in the scene.", typeof(T).FullName));
            if (onFind != null)
                onFind(obj);
            return obj;
        }
    }

    static class GameObjectEx
    {
        public static T GetComponent<T>(this GameObject obj, Action<T> onGet)
        {
            var comp = obj.GetComponent<T>();
            if (comp != null)
                onGet(comp);
            return comp;
        }

        public static T GetOrAddComponent<T>(this GameObject obj) where T : Component
        {
            var comp = obj.GetComponent<T>();
            if (comp != null)
            {
                return comp;
            }
            else
            {
                return obj.gameObject.AddComponent<T>();
            }
        }

        public static List<Transform> GetChildTransforms(this GameObject obj, bool includeInActive = false)
        {
            var children = obj.GetComponentsInChildren<Transform>(includeInActive);
            var childTransforms = new List<Transform>();
            foreach (var child in children)
            {
                if (child.parent == obj.transform)
                {
                    childTransforms.Add(child);
                }
            }
            return childTransforms;
        }
    }
}
