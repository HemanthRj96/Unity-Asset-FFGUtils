using System.Collections.Generic;
using UnityEngine;

namespace FickleFrames.ReferenceSystem
{
    /// <summary>
    /// This class is used to dynamically store and retrieve references at any point
    /// </summary>
    public static class ReferenceHandler
    {
        /// <summary>
        /// Container that stores all the references
        /// </summary>
        public static Dictionary<string, Object> container = new Dictionary<string, Object>();

        /// <summary>
        /// Use this function to add object reference to the world reference handler
        /// </summary>
        /// <param name="value">The class reference you need to store</param>
        public static void Add(Object value)
        {
            container.Add(value.name, value);
        }

        /// <summary>
        /// Use this function to add object reference to the world reference handler
        /// </summary>
        /// <param name="value">The class reference you need to store</param>
        public static void Add(string tag, Object value)
        {
            if (DuplicateCheck(tag))
                return;
            container.Add(tag, value);
        }

        /// <summary>
        /// Removes reference of the given value
        /// </summary>
        /// <param name="value">The class reference you need to remove</param>
        public static void Remove(Object value)
        {
            string key = FindTag(value);
            if (key != null)
                container.Remove(key);
        }

        /// <summary>
        /// Removes reference of the given value
        /// </summary>
        /// <param name="tag">The tag you want to remove</param>
        public static void Remove(string tag)
        {
            if (container.ContainsKey(tag))
                container.Remove(tag);
        }

        /// <summary>
        /// Removes references of the same data type
        /// </summary>
        /// <param name="value"></param>
        public static void RemoveAll<T>() where T : class
        {
            foreach (string tag in container.Keys)
                if (container[tag] is T)
                    container.Remove(tag);
        }

        /// <summary>
        /// Return the the object from the container if found and null otherwise
        /// </summary>
        /// <param name="tag">String tag for the camera controller</param>
        public static Object GetReference(string tag)
        {
            if (!container.ContainsKey(tag))
                return null;
            return container[tag];
        }

        /// <summary>
        /// Returns true if the specified data type is found and false otherwise
        /// </summary>
        /// <param name="value">Out variable of the required type</param>
        public static bool GetReference<T>(out T value) where T : class
        {
            value = null;
            foreach (string tag in container.Keys)
                if (container[tag] is T)
                {
                    value = container[tag] as T;
                    return true;
                }
            return false;
        }

        /// <summary>
        /// Returns true if the tag matches to the reference tag
        /// </summary>
        /// <param name="tag">String value that is linked to the data</param>
        /// <param name="value">Out parameter of the required reference</param>
        public static bool GetReference<T>(string tag, out T value) where T : class
        {
            value = null;
            if (!container.ContainsKey(tag))
                return false;
            value = container[tag] as T;
            return true;
        }

        /// <summary>
        /// Returns true if the specified data type is found and false otherwise
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">Out array variable of the required type</param>
        /// <returns></returns>
        public static bool GetReferences<T>(out List<T> value) where T : class
        {
            bool flag = false;
            value = new List<T>();

            foreach (string tag in container.Keys)
                if (container[tag] is T)
                {
                    value.Add(container[tag] as T);
                    flag = true;
                }
            return flag;
        }

        /// <summary>
        /// Returns true if the reference exists
        /// </summary>
        /// <param name="value">Target value</param>
        public static bool CheckIfReferenceExists(Object value) => container.ContainsValue(value);

        /// <summary>
        /// Returns true if the reference exists
        /// </summary>
        /// <param name="tag">Target tag</param>
        public static bool CheckIfReferenceExists(string tag) => container.ContainsKey(tag);

        /// <summary>
        /// Returns true if duplicate exists
        /// </summary>
        /// <param name="tag"></param>
        private static bool DuplicateCheck(string tag)
        {
            if (container.ContainsKey(tag))
            {
                Debug.LogError($"Found duplicate key :{tag}");
                return true;
            }
            return false;
        }

        /// <summary>
        /// Returns the tag for the target value
        /// </summary>
        /// <param name="value">Target value</param>
        private static string FindTag(Object value)
        {
            foreach (var temp in container)
                if (temp.Value == value)
                    return temp.Key;
            return null;
        }
    }
}