using System;
using System.Collections.Generic;
using System.Reflection;


public static class ReflectionHelpers
{
    /// <summary>
    /// Returns the derived types of this basetypes
    /// </summary>
    public static Type[] GetAllDerivedTypes(this Type baseType)
    {
        Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
        List<Type> derivedTypes = new List<Type>();

        foreach (Assembly assembly in assemblies)
        {
            var types = assembly.GetTypes();
            foreach (Type type in types)
            {
                if (type.IsSubclassOf(baseType))
                    derivedTypes.Add(type);
            }
        }

        return derivedTypes.ToArray();
    }

    /// <summary>
    /// Returns the derived types of this basetypes, from specific assemblies
    /// </summary>
    /// <param name="assemblies">Target asemblies</param>
    public static Type[] GetAllDerivedTypes(this Type baseType, Assembly[] assemblies)
    {
        List<Type> derivedTypes = new List<Type>();

        foreach (Assembly assembly in assemblies)
        {
            var types = assembly.GetTypes();
            foreach (Type type in types)
            {
                if (type.IsSubclassOf(baseType))
                    derivedTypes.Add(type);
            }
        }

        return derivedTypes.ToArray();
    }
}
