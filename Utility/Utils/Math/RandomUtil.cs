///====================================================================================================
///
///     RandomUtil by
///     - CantyCanadian
///
///====================================================================================================

using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Canty
{
    public static class RandomUtil
    {
        /// <summary>
        /// Extension of Unity's Random.Range function, making so it gets multiple numbers at once.
        /// </summary>
        /// <param name="min">Minimum value [inclusive]</param>
        /// <param name="max">Maximum value [exclusive]</param>
        /// <param name="count">How many values to get.</param>
        /// <returns>All the random values.</returns>
        public static int[] RangeMultiple(int min, int max, int count)
        {
            int[] result = new int[count];

            for(int i = 0; i < count; i++)
            {
                result[i] = Random.Range(min, max);
            }

            return result;
        }

        /// <summary>
        /// Extension of Unity's Random.Range function, making so it gets multiple numbers at once.
        /// </summary>
        /// <param name="min">Minimum value [inclusive]</param>
        /// <param name="max">Maximum value [inclusive]</param>
        /// <param name="count">How many values to get.</param>
        /// <returns>All the random values.</returns>
        public static float[] RangeMultiple(float min, float max, int count)
        {
            float[] result = new float[count];

            for (int i = 0; i < count; i++)
            {
                result[i] = Random.Range(min, max);
            }

            return result;
        }

        /// <summary>
        /// Extension of Unity's Random.Range function, making so it gets multiple numbers at once. However, each numbers are unique.
        /// </summary>
        /// <param name="min">Minimum value [inclusive]</param>
        /// <param name="max">Maximum value [exclusive]</param>
        /// <param name="count">How many values to get.</param>
        /// <returns>All the random values.</returns>
        public static int[] RangeMultipleUnique(int min, int max, int count)
        {
            List<int> container = ListUtil.RangePopulate(min, max);

            if (container.Count == count)
            {
                return container.ToArray();
            }

            if (container.Count < count)
            {
                Debug.LogError("RandomUtil : Requesting more unique random number than there are available.");
                return container.ToArray();
            }

            int[] result = new int[count];

            for (int i = 0; i < count; i++)
            {
                result[i] = container.GetRandom();
                container.Remove(result[i]);
            }

            return result;
        }

        /// <summary>
        /// Extension of Unity's Random.Range function, adding extra integers that the result CANNOT be.
        /// </summary>
        /// <param name="min">Minimum value [inclusive]</param>
        /// <param name="max">Maximum value [exclusive]</param>
        /// <param name="exceptions">Values that can't be the result.</param>
        /// <returns>Random value.</returns>
        public static int RangeExcept(int min, int max, params int[] exceptions)
        {
            List<int> container = ListUtil.RangePopulate(min, max);
            container.RemoveEquals(new List<int>(exceptions));
            
            return container.GetRandom();
        }


        /// <summary>
        /// Implementation of Unity's Random.Range function, but to return a value from a passed in array set as a params argument.
        /// </summary>
        /// <param name="values">Possible values to pick from.</param>
        public static T RangeFrom<T>(params T[] values)
        {
            return values[Random.Range(0, values.Length)];
        }

        /// <summary>
        /// Implementation of Unity's Random.Range function, but to return multiple values from a passed in array set as a params argument.
        /// </summary>
        /// <param name="count">How many values to get.</param>
        /// <param name="values">Possible values to pick from.</param>
        public static T[] RangeFromMultiple<T>(int count, params T[] values)
        {
            T[] result = new T[count];

            List<T> container = new List<T>(values);

            for (int i = 0; i < count; i++)
            {
                result[i] = container.GetRandom();
            }

            return result;
        }

        /// <summary>
        /// Implementation of Unity's Random.Range function, but to return multiple values from a passed in array set as a params argument. No values are picked twice.
        /// </summary>
        /// <param name="count">How many values to get.</param>
        /// <param name="values">Possible values to pick from.</param>
        public static T[] RangeFromMultipleUnique<T>(int count, params T[] values)
        {
            List<T> container = new List<T>(values);

            if (container.Count == count)
            {
                return container.ToArray();
            }

            if (container.Count < count)
            {
                Debug.LogError("RandomUtil : Requesting more unique random number than there are available.");
                return container.ToArray();
            }

            T[] result = new T[count];

            for (int i = 0; i < count; i++)
            {
                result[i] = container.GetRandom();
                container.Remove(result[i]);
            }

            return result;
        }

        /// <summary>
        /// Extension of Unity's Random.Range function, adding extra integers that the result CANNOT be.
        /// </summary>
        /// <param name="min">Minimum value [inclusive]</param>
        /// <param name="max">Maximum value [exclusive]</param>
        /// <param name="exceptions">Values that can't be the result.</param>
        /// <returns>Random value.</returns>
        public static int[] RangeMultipleExcept(int min, int max, int count, params int[] exceptions)
        {
            int[] result = new int[count];

            List<int> container = ListUtil.RangePopulate(min, max);
            container.RemoveEquals(new List<int>(exceptions));

            for (int i = 0; i < count; i++)
            {
                result[i] = container.GetRandom();
            }

            return result;
        }

        /// <summary>
        /// Extension of Unity's Random.Range function, adding extra integers that the result CANNOT be.
        /// </summary>
        /// <param name="min">Minimum value [inclusive]</param>
        /// <param name="max">Maximum value [exclusive]</param>
        /// <param name="exceptions">Values that can't be the result.</param>
        /// <returns>Random value.</returns>
        public static int[] RangeMultipleUniqueExcept(int min, int max, int count, params int[] exceptions)
        {
            List<int> container = ListUtil.RangePopulate(min, max);
            container.RemoveEquals(new List<int>(exceptions));

            if (container.Count == count)
            {
                return container.ToArray();
            }

            if (container.Count < count)
            {
                Debug.LogError("RandomUtil : Requesting more unique random number than there are available.");
                return container.ToArray();
            }

            int[] result = new int[count];

            for (int i = 0; i < count; i++)
            {
                result[i] = container.GetRandom();
                container.Remove(result[i]);
            }

            return result;
        }

        /// <summary>
        /// Returns a random enum value from within an enum type.
        /// </summary>
        /// <typeparam name="E">Enum type.</typeparam>
        /// <returns>Random enum value.</returns>
        public static E RandomEnum<E>() where E : struct, IConvertible
        {
            Array values = Enum.GetValues(typeof(E));
            return (E)values.GetValue(Random.Range(0, values.Length));
        }

        /// <summary>
        /// Returns multiple random enum values from within an enum type.
        /// </summary>
        /// <typeparam name="E">Enum type.</typeparam>
        /// <param name="count">How many values.</param>
        /// <returns>Random enum value.</returns>
        public static T[] RandomEnumsMultiple<T>(int count) where T : struct, IConvertible
        {
            List<T> container = List<T>(Enum.GetValues(typeof(T)));

            if (container.Length == count)
            {
                return container.ToArray();
            }
            else if (container.Length > count)
            {
                Debug.LogError("RandomUtil : Requesting more unique random enum values than there are available.");
                return container.ToArray();
            }

            T[] returnList = new T[count];

            for (int i = 0; i < count; i++)
            {
                int randomValue = Random.Range(0, container.Count);
                returnList[i] = (T)values.GetValue(randomValue);
            }

            return returnList;
        }

        /// <summary>
        /// Returns multiple random enum values from within an enum type. Each values have no duplicates.
        /// </summary>
        /// <typeparam name="E">Enum type.</typeparam>
        /// <param name="count">How many values.</param>
        /// <returns>Random enum value.</returns>
        public static T[] RandomEnumsMultipleUnique<T>(int count) where T : struct, IConvertible
        {
            List<T> container = List<T>(Enum.GetValues(typeof(T)));

            if (container.Length == count)
            {
                return container.ToArray();
            }
            else if (container.Length > count)
            {
                Debug.LogError("RandomUtil : Requesting more unique random enum values than there are available.");
                return container.ToArray();
            }

            T[] returnList = new T[count];

            for(int i = 0; i < count; i++)
            {
                int randomValue = Random.Range(0, container.Count);
                returnList[i] = (T)values.GetValue(randomValue);
                container.RemoveAt(randomValue);
            }

            return returnList;
        }

        /// <summary>
        /// Implementation of Unity's Random.Range function, but to return a bool instead of a number.
        /// </summary>
        /// <returns>Random bool.</returns>
        public static bool RandomBool()
        {
            return Random.Range(0, 2) == 0 ? false : true;
        }
    }
}
