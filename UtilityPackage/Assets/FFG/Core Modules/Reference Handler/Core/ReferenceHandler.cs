using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FFG
{
    /// <summary>
    /// This class is used to dynamically store and retrieve references at any point
    /// </summary>
    public static class ReferenceHandler
    {
        /*.............................................Private Fields.......................................................*/

        private static Dictionary<string, Object> s_container = new Dictionary<string, Object>();

        /*.............................................Private Methods......................................................*/

        /// <summary>
        /// Returns true if duplicate exists
        /// </summary>
        /// <param name="tag"></param>
        private static bool DuplicateCheck(string tag)
        {
            if (s_container.ContainsKey(tag))
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
            foreach (var temp in s_container)
                if (temp.Value == value)
                    return temp.Key;
            return null;
        }

        /*.............................................Public Methods.......................................................*/

        /// <summary>
        /// Adds an object
        /// </summary>
        public static void Add(Object value)
        {
            s_container.Add(value.name, value);
        }

        /// <summary>
        /// Adds an object
        /// </summary>
        /// <param name="tag">Name tag given to the object</param>
        public static void Add(string tag, Object value)
        {
            if (DuplicateCheck(tag))
                return;
            s_container.Add(tag, value);
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
            if (s_container.ContainsKey(tag))
                s_container.Remove(tag);
        }

        /// <summary>
        /// Removes object reference of the same data type
        /// </summary>
        public static void RemoveAll<T>() where T : class
        {
            foreach (string tag in s_container.Keys.ToList())
                if (s_container[tag] is T)
                    s_container.Remove(tag);
        }

        /// <summary>
        /// Returns the reference of the object
        /// </summary>
        public static Object GetReference(string tag)
        {
            if (!s_container.ContainsKey(tag))
                return null;
            return s_container[tag];
        }

        /// <summary>
        /// Returns true if the reference was found, false otheriwse
        /// </summary>
        public static bool GetReference<T>(out T value) where T : class
        {
            value = null;
            foreach (string tag in s_container.Keys)
                if (s_container[tag] is T)
                {
                    value = s_container[tag] as T;
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
            if (!s_container.ContainsKey(tag))
                return false;
            value = s_container[tag] as T;
            return true;
        }

        /// <summary>
        /// Returns true if the reference was found, false otheriwse
        /// </summary>
        public static bool GetReferences<T>(out List<T> value) where T : class
        {
            bool flag = false;
            value = new List<T>();

            foreach (string tag in s_container.Keys)
                if (s_container[tag] is T)
                {
                    value.Add(s_container[tag] as T);
                    flag = true;
                }
            return flag;
        }

        /// <summary>
        /// Returns true if the reference exists
        /// </summary>
        public static bool CheckIfReferenceExists(Object value) => s_container.ContainsValue(value);

        /// <summary>
        /// Returns true if the reference exists
        /// </summary>
        public static bool CheckIfReferenceExists(string tag) => s_container.ContainsKey(tag);
    }
}