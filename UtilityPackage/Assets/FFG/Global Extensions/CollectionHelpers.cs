using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public static class CollectionHelpers
{
    /// <summary>
    /// Prints all the values inside an array
    /// </summary>
    public static void PrintValues<TType>(this TType[] arr)
    {
        for (int i = 0; i < arr.Length; ++i)
            Debug.Log($"Item #{i} : {arr[i]}");
    }

    /// <summary>
    /// Prints all the value inside a list
    /// </summary>
    public static void PrintValues<TType>(this List<TType> list)
    {
        for (int i = 0; i < list.Count; ++i)
            Debug.Log($"Item #{i} : {list[i]}");
    }

    /// <summary>
    /// Adds an item if it doesn't exist
    /// </summary>
    public static void TryAdd<TType>(this List<TType> list, TType item)
    {
        if (!list.Contains(item))
            list.Add(item);
    }

    /// <summary>
    /// Shuffles elements in a list
    /// </summary>
    public static void Shuffle<TType>(this List<TType> list)
    {
        System.Random random = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            TType value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    /// <summary>
    /// Prints all the values inside the dictionary
    /// </summary>
    public static void PrintValues<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
    {
        foreach (var temp in dictionary)
            Debug.Log($"Key : {temp.Key}, Value : {temp.Value}");
    }

    /// <summary>
    /// Removes an item in dictionary if it exists
    /// </summary>
    public static void TryRemove<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key)
    {
        if (dictionary.ContainsKey(key))
            dictionary.Remove(key);
    }

    /// <summary>
    /// Removes a value from dictionary if found and returns true if found and false otherwise
    /// </summary>
    /// <param name="value">Target value</param>
    public static bool TryRemoveValue<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TValue value)
    {
        bool found = false;
        foreach (var temp in dictionary.ToList())
            if (temp.Value.Equals(value))
            {
                found = true;
                dictionary.Remove(temp.Key);
            }
        return found;
    }

    /// <summary>
    /// Adds key and value if it doesn't exist
    /// </summary>
    public static void TryAdd<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
    {
        if (!dictionary.ContainsKey(key))
            dictionary.Add(key, value);
    }

    /// <summary>
    /// Returns the value if it exists or null
    /// </summary>
    public static TValue GetOrNull<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key) where TValue : class
    {
        if (dictionary.ContainsKey(key))
            return dictionary[key];
        return null;
    }
}
