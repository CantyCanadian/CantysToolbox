///====================================================================================================
///
///     ClassUtil by
///     - CantyCanadian
///
///====================================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Canty
{
    public static class ClassUtil
    {
        public static Type[] GetAllTypesOfChildrenClasses<T>() where T : class
        {
            List<Type> objects = new List<Type>();
            foreach (Type type in Assembly.GetAssembly(typeof(T)).GetTypes().Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T))))
            {
                objects.Add(type);
            }

            return objects.ToArray();
        }
    }
}
