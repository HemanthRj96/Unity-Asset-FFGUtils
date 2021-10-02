using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FickleFrames
{
    /// <summary>
    /// This class is used to dynamically store and retrieve references at any point
    /// </summary>
    public static class ReferenceHandler
    {
        #region Internals

        private static Dictionary<string, Object> container = new Dictionary<string, Object>();


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
        private static string FindKey(Object value)
        {
            foreach (var temp in container)
                if (temp.Value == value)
                    return temp.Key;
            return null;
        }

        #endregion Internals


        /// <summary>
        /// Adds an object
        /// </summary>
        public static void Add(Object value)
        {
            container.Add(value.name, value);
        }


        /// <summary>
        /// Adds an object
        /// </summary>
        /// <param name="tag">Name tag given to the object</param>
        public static void Add(string tag, Object value)
        {
            if (DuplicateCheck(tag))
                return;
            container.Add(tag, value);
        }


        /// <summary>
        /// Removes object reference
        /// </summary>
        public static void Remove(Object value)
        {
            Remove(FindKey(value));
        }


        /// <summary>
        /// Removes object reference
        /// </summary>
        public static void Remove(string tag)
        {
            if (container.ContainsKey(tag))
                container.Remove(tag);
        }


        /// <summary>
        /// Removes object reference of the same data type
        /// </summary>
        public static void RemoveAll<T>() where T : class
        {
            foreach (string tag in container.Keys.ToList())
                if (container[tag] is T)
                    container.Remove(tag);
        }


        /// <summary>
        /// Returns the reference of the object
        /// </summary>
        public static Object GetReference(string tag)
        {
            if (!container.ContainsKey(tag))
                return null;
            return container[tag];
        }


        /// <summary>
        /// Returns true if the reference was found, false otheriwse
        /// </summary>
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
        /// Returns true if the reference was found, false otheriwse
        /// </summary>
        public static bool GetReference<T>(string tag, out T value) where T : class
        {
            value = null;
            if (!container.ContainsKey(tag))
                return false;
            value = container[tag] as T;
            return true;
        }


        /// <summary>
        /// Returns true if the reference was found, false otheriwse
        /// </summary>
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
        public static bool CheckIfReferenceExists(Object value) => container.ContainsValue(value);


        /// <summary>
        /// Returns true if the reference exists
        /// </summary>
        public static bool CheckIfReferenceExists(string tag) => container.ContainsKey(tag);
    }
}