///====================================================================================================
///
///     RandomNumberGenerator by
///     - CantyCanadian
///
///====================================================================================================

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Canty
{
    /// <summary>
    /// Creates a generator object that uses the PCG randomizer algorithm. It is more performant than Unity's default random number generator, as well as including extra
    /// features like moving forward and back in the number sequence at a O(log n) speed. To use, create a class either take the default seed or provide your own.
    /// If two generators have the same seed, they will give the same sequence of results. Use this to your advantage to create either totally random or repeatable random
    /// sequences.
    /// </summary>
    public class RandomNumberGenerator
    {
        private const ulong INCREMENT = (721347520444481703ul << 1) | 1;
        private const ulong MULTIPLIER = 6364136223846793005ul;
        private const double INTTODOUBLE = 1.0 / 4294967296.0;

        private ulong m_State;

        #region Next()

        /// <summary>
        /// Returns value between 0 and 4 294 967 295.
        /// </summary>
        public uint NextUInt()
        {
            return GenerateNewValue();
        }

        /// <summary>
        /// Returns value between -2 147 483 648 and 2 147 483 647.
        /// </summary>
        public int NextInt()
        {
            return (int)(GenerateNewValue() >> 1);
        }

        /// <summary>
        /// Returns value between 0 and 0.999999999.
        /// </summary>
        public float NextFloat()
        {
            return (float)(GenerateNewValue() * INTTODOUBLE);
        }

        /// <summary>
        /// Returns value between 0 and 0.9999999997671694.
        /// </summary>
        public double NextDouble()
        {
            return GenerateNewValue() * INTTODOUBLE;
        }

        /// <summary>
        /// Returns value between 0 and 255.
        /// </summary>
        public byte NextByte()
        {
            return (byte)(GenerateNewValue() % 256);
        }

        /// <summary>
        /// Returns either 0 or 1.
        /// </summary>
        public bool NextBool()
        {
            return GenerateNewValue() % 2 == 1;
        }

        /// <summary>
        /// Returns a random enum from a passed-in type.
        /// </summary>
        public E NextEnum<E>() where E : struct, IConvertible
        {
            Array values = Enum.GetValues(typeof(E));
            return (E)values.GetValue(NextInt(values.Length));
        }

        /// <summary>
        /// Returns a random value from a passed-in array.
        /// </summary>
        public T NextArrayValue<T>(T[] values)
        {
            return (T)values.GetValue(NextInt(values.Length));
        }

        #endregion

        #region Next(maxExclusive)

        /// <summary>
        /// Retuns a value between 0 and maxExclusive.
        /// </summary>
        public uint NextUInt(uint maxExclusive)
        {
            return GenerateNewValue() % maxExclusive;
        }

        /// <summary>
        /// Retuns a value between 0 and maxExclusive (or vice versa if negative).
        /// </summary>
        public int NextInt(int maxExclusive)
        {
            return (int)(GenerateNewValue() >> 1) % maxExclusive;
        }

        /// <summary>
        /// Retuns a value between 0 and maxExclusive (or vice versa if negative).
        /// </summary>
        public float NextFloat(float maxExclusive)
        {
            return (float)(GenerateNewValue() * INTTODOUBLE) * maxExclusive;
        }

        /// <summary>
        /// Retuns a value between 0 and maxExclusive (or vice versa if negative).
        /// </summary>
        public double NextDouble(double maxExclusive)
        {
            return (GenerateNewValue() * INTTODOUBLE) * maxExclusive;
        }

        /// <summary>
        /// Retuns a value between 0 and maxExclusive.
        /// </summary>
        public byte NextByte(byte maxExclusive)
        {
            return (byte)((GenerateNewValue() % 256) % maxExclusive);
        }

        #endregion

        #region Next(minInclusive, maxExclusive)

        /// <summary>
        /// Retuns a value between minInclusive and maxExclusive. Returns 0 if max is under min.
        /// </summary>
        public uint NextUInt(uint minInclusive, uint maxExclusive)
        {
            return maxExclusive > minInclusive ? GenerateNewValue() % (maxExclusive - minInclusive) + minInclusive : 0;
        }

        /// <summary>
        /// Retuns a value between minInclusive and maxExclusive. Returns 0 if max is under min.
        /// </summary>
        public int NextInt(int minInclusive, int maxExclusive)
        {
            return maxExclusive > minInclusive ? (int)(GenerateNewValue() >> 1) % (maxExclusive - minInclusive) + minInclusive : 0;
        }

        /// <summary>
        /// Retuns a value between minInclusive and maxExclusive. Returns 0 if max is under min.
        /// </summary>
        public float NextFloat(float minInclusive, float maxExclusive)
        {
            return maxExclusive > minInclusive ? (float)(GenerateNewValue() * INTTODOUBLE) % (maxExclusive - minInclusive) + minInclusive : 0;
        }

        /// <summary>
        /// Retuns a value between minInclusive and maxExclusive. Returns 0 if max is under min.
        /// </summary>
        public double NextDouble(double minInclusive, double maxExclusive)
        {
            return maxExclusive > minInclusive ? (GenerateNewValue() * INTTODOUBLE) % (maxExclusive - minInclusive) + minInclusive : 0;
        }

        /// <summary>
        /// Retuns a value between minInclusive and maxExclusive. Returns 0 if max is under min.
        /// </summary>
        public byte NextByte(byte minInclusive, byte maxExclusive)
        {
            return maxExclusive > minInclusive ? (byte)((GenerateNewValue() % 256) % (maxExclusive - minInclusive) + minInclusive) : (byte)0;
        }

        #endregion

        #region Nexts(count)

        /// <summary>
        /// Retuns an array of values between 0 and 4 294 967 295.
        /// </summary>
        public List<uint> NextUInts(uint count)
        {
            List<uint> items = new List<uint>();

            for(int i = 0; i < count; i++)
            {
                items.Add(GenerateNewValue());
            }

            return items;
        }

        /// <summary>
        /// Retuns an array of values -2 147 483 648 and 2 147 483 647.
        /// </summary>
        public List<int> NextInts(uint count)
        {
            List<int> items = new List<int>();

            for (int i = 0; i < count; i++)
            {
                items.Add((int)(GenerateNewValue() >> 1));
            }

            return items;
        }

        /// <summary>
        /// Retuns an array of values between 0 and 0.999999999.
        /// </summary>
        public List<float> NextFloats (uint count)
        {
            List<float> items = new List<float>();

            for (int i = 0; i < count; i++)
            {
                items.Add((float)(GenerateNewValue() * INTTODOUBLE));
            }

            return items;
        }

        /// <summary>
        /// Retuns an array of values between 0 and 0.9999999997671694.
        /// </summary>
        public List<double> NextDoubles(uint count)
        {
            List<double> items = new List<double>();

            for (int i = 0; i < count; i++)
            {
                items.Add(GenerateNewValue() * INTTODOUBLE);
            }

            return items;
        }

        /// <summary>
        /// Retuns an array of values between 0 and 255.
        /// </summary>
        public List<byte> NextBytes(uint count)
        {
            List<byte> items = new List<byte>();

            for (int i = 0; i < count; i++)
            {
                items.Add((byte)(GenerateNewValue() % 256));
            }

            return items;
        }

        /// <summary>
        /// Returns an array of values that are either 0 or 1.
        /// </summary>
        public List<bool> NextBools(uint count)
        {
            List<bool> items = new List<bool>();

            for (int i = 0; i < count; i++)
            {
                items.Add(GenerateNewValue() % 2 == 1);
            }

            return items;
        }

        #endregion

        #region Nexts(count, unique)

        /// <summary>
        /// Returns multiple random enums from a passed-in type.
        /// If unique, no values in the list will duplicate.
        /// </summary>
        public List<E> NextEnums<E>(uint count, bool unique = false) where E : struct, IConvertible
        {
            Array values = Enum.GetValues(typeof(E));

            if (unique && count > values.Length)
            {
                Debug.LogError("RNGUtil : Impossible to get an unique list due to lack of available values.");
                return new List<E>();
            }

            List<E> items = new List<E>();

            if (unique)
            {
                for (int i = 0; i < count; i++)
                {
                    while (items.Count <= i)
                    {
                        long value = GenerateNewValue() % values.Length;

                        if (!items.Contains((E)values.GetValue(value)))
                        {
                            items.Add((E)values.GetValue(value));
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    items.Add((E)values.GetValue(GenerateNewValue() % values.Length));
                }
            }

            return items;
        }

        /// <summary>
        /// Returns multiple random values from a passed-in array.
        /// If unique, no values in the list will duplicate.
        /// </summary>
        public List<T> NextArrayValues<T>(T[] values, uint count, bool unique = false)
        {
            if (unique && count > values.Length)
            {
                Debug.LogError("RNGUtil : Impossible to get an unique list due to lack of available values.");
                return new List<T>();
            }

            List<T> items = new List<T>();

            if (unique)
            {
                for (int i = 0; i < count; i++)
                {
                    while (items.Count <= i)
                    {
                        long value = GenerateNewValue() % values.Length;

                        if (!items.Contains(values[value]))
                        {
                            items.Add(values[value]);
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    items.Add(values[GenerateNewValue() % values.Length]);
                }
            }

            return items;
        }

        #endregion

        #region Nexts(count, maxExclusive, unique)

        /// <summary>
        /// Retuns an array of values between 0 and maxExclusive.
        /// If unique, no numbers in the list will duplicate.
        /// </summary>
        public List<uint> NextUInts(uint count, uint maxExclusive, bool unique = false)
        {
            if (unique && count > maxExclusive)
            {
                Debug.LogError("RNGUtil : Impossible to get an unique list due to lack of available values.");
                return new List<uint>();
            }

            List<uint> items = new List<uint>();

            if (unique)
            {
                for (int i = 0; i < count; i++)
                {
                    while (items.Count <= i)
                    {
                        uint value = GenerateNewValue() % maxExclusive;

                        if (!items.Contains(value))
                        {
                            items.Add(value);
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    items.Add(GenerateNewValue() % maxExclusive);                    
                }
            }

            return items;
        }

        /// <summary>
        /// Retuns an array of values 0 and maxExclusive.
        /// If unique, no numbers in the list will duplicate.
        /// </summary>
        public List<int> NextInts(uint count, uint maxExclusive, bool unique = false)
        {
            if (unique && count > maxExclusive)
            {
                Debug.LogError("RNGUtil : Impossible to get an unique list due to lack of available values.");
                return new List<int>();
            }

            List<int> items = new List<int>();

            if (unique)
            {
                for (int i = 0; i < count; i++)
                {
                    while (items.Count <= i)
                    {
                        int value = (int)((GenerateNewValue() >> 1) % maxExclusive);

                        if (!items.Contains(value))
                        {
                            items.Add(value);
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    items.Add((int)((GenerateNewValue() >> 1) % maxExclusive));
                }
            }

            return items;
        }

        /// <summary>
        /// Retuns an array of values between 0 and maxExclusive (or vice versa if negative).
        /// If unique, no numbers in the list will duplicate.
        /// </summary>
        public List<float> NextFloats(uint count, float maxExclusive, bool unique = false)
        {
            List<float> items = new List<float>();

            if (unique)
            {
                for (int i = 0; i < count; i++)
                {
                    while (items.Count <= i)
                    {
                        float value = (float)((GenerateNewValue() * INTTODOUBLE) * maxExclusive);

                        if (!items.Contains(value))
                        {
                            items.Add(value);
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    items.Add((float)(GenerateNewValue() * INTTODOUBLE) * maxExclusive);
                }
            }

            return items;
        }

        /// <summary>
        /// Retuns an array of values between 0 and maxExclusive (or vice versa if negative).
        /// If unique, no numbers in the list will duplicate.
        /// </summary>
        public List<double> NextDoubles(uint count, double maxExclusive, bool unique = false)
        {
            List<double> items = new List<double>();

            if (unique)
            {
                for (int i = 0; i < count; i++)
                {
                    while (items.Count <= i)
                    {
                        double value = (GenerateNewValue() * INTTODOUBLE) * maxExclusive;

                        if (!items.Contains(value))
                        {
                            items.Add(value);
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    items.Add((GenerateNewValue() * INTTODOUBLE) * maxExclusive);
                }
            }

            return items;
        }

        /// <summary>
        /// Retuns an array of values between 0 and maxExclusive.
        /// If unique, no numbers in the list will duplicate.
        /// </summary>
        public List<byte> NextBytes(uint count, byte maxExclusive, bool unique = false)
        {
            if (unique && count > maxExclusive)
            {
                Debug.LogError("RNGUtil : Impossible to get an unique list due to lack of available values.");
                return new List<byte>();
            }

            List<byte> items = new List<byte>();

            if (unique)
            {
                for (int i = 0; i < count; i++)
                {
                    while (items.Count <= i)
                    {
                        byte value = (byte)((GenerateNewValue() % 256) % maxExclusive);

                        if (!items.Contains(value))
                        {
                            items.Add(value);
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    items.Add((byte)((GenerateNewValue() % 256) % maxExclusive));
                }
            }

            return items;
        }

        #endregion

        #region Nexts(count, maxExclusive, minInclusive, unique)

        /// <summary>
        /// Retuns an array of values between minInclusive and maxExclusive. Returns 0 if max is under min.
        /// </summary>
        public List<uint> NextUInts(uint count, uint maxExclusive, uint minInclusive, bool unique = false)
        {
            if (unique && count > (maxExclusive - minInclusive))
            {
                Debug.LogError("RNGUtil : Impossible to get an unique list due to lack of available values.");
                return new List<uint>();
            }

            List<uint> items = new List<uint>();

            if (unique)
            {
                for (int i = 0; i < count; i++)
                {
                    while (items.Count <= i)
                    {
                        uint value = maxExclusive > minInclusive ? GenerateNewValue() % (maxExclusive - minInclusive) + minInclusive : 0;

                        if (!items.Contains(value))
                        {
                            items.Add(value);
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    items.Add(maxExclusive > minInclusive ? GenerateNewValue() % (maxExclusive - minInclusive) + minInclusive : 0);
                }
            }

            return items;
        }

        /// <summary>
        /// Retuns an array of values minInclusive and maxExclusive. Returns 0 if max is under min.
        /// </summary>
        public List<int> NextInts(uint count, int maxExclusive, int minInclusive, bool unique = false)
        {
            if (unique && count > (maxExclusive - minInclusive))
            {
                Debug.LogError("RNGUtil : Impossible to get an unique list due to lack of available values.");
                return new List<int>();
            }

            List<int> items = new List<int>();

            if (unique)
            {
                for (int i = 0; i < count; i++)
                {
                    while (items.Count <= i)
                    {
                        int value = maxExclusive > minInclusive ? (int)(GenerateNewValue() >> 1) % (maxExclusive - minInclusive) + minInclusive : 0;

                        if (!items.Contains(value))
                        {
                            items.Add(value);
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    items.Add(maxExclusive > minInclusive ? (int)(GenerateNewValue() >> 1) % (maxExclusive - minInclusive) + minInclusive : 0);
                }
            }

            return items;
        }

        /// <summary>
        /// Retuns an array of values between minInclusive and maxExclusive. Returns 0 if max is under min.
        /// </summary>
        public List<float> NextFloats(uint count, float maxExclusive, float minInclusive, bool unique = false)
        {
            List<float> items = new List<float>();

            if (unique)
            {
                for (int i = 0; i < count; i++)
                {
                    while (items.Count <= i)
                    {
                        float value = maxExclusive > minInclusive ? (float)(GenerateNewValue() * INTTODOUBLE) % (maxExclusive - minInclusive) + minInclusive : 0;

                        if (!items.Contains(value))
                        {
                            items.Add(value);
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    items.Add(maxExclusive > minInclusive ? (float)(GenerateNewValue() * INTTODOUBLE) % (maxExclusive - minInclusive) + minInclusive : 0);
                }
            }

            return items;
        }

        /// <summary>
        /// Retuns an array of values between minInclusive and maxExclusive. Returns 0 if max is under min.
        /// </summary>
        public List<double> NextDoubles(uint count, double maxExclusive, double minInclusive, bool unique = false)
        {
            List<double> items = new List<double>();

            if (unique)
            {
                for (int i = 0; i < count; i++)
                {
                    while (items.Count <= i)
                    {
                        double value = maxExclusive > minInclusive ? (GenerateNewValue() * INTTODOUBLE) % (maxExclusive - minInclusive) + minInclusive : 0;

                        if (!items.Contains(value))
                        {
                            items.Add(value);
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    items.Add(maxExclusive > minInclusive ? (GenerateNewValue() * INTTODOUBLE) % (maxExclusive - minInclusive) + minInclusive : 0);
                }
            }

            return items;
        }

        /// <summary>
        /// Retuns an array of values between minExclusive and maxExclusive. Returns 0 if max is under min.
        /// </summary>
        public List<byte> NextBytes(uint count, byte maxExclusive, byte minInclusive, bool unique = false)
        {
            if (unique && count > (maxExclusive - minInclusive))
            {
                Debug.LogError("RNGUtil : Impossible to get an unique list due to lack of available values.");
                return new List<byte>();
            }

            List<byte> items = new List<byte>();

            if (unique)
            {
                for (int i = 0; i < count; i++)
                {
                    while (items.Count <= i)
                    {
                        byte value = maxExclusive > minInclusive ? (byte)((GenerateNewValue() % 256) % (maxExclusive - minInclusive) + minInclusive) : (byte)0;

                        if (!items.Contains(value))
                        {
                            items.Add(value);
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    items.Add(maxExclusive > minInclusive ? (byte)((GenerateNewValue() % 256) % (maxExclusive - minInclusive) + minInclusive) : (byte)0);
                }
            }

            return items;
        }

        #endregion

        #region CheckNext()

        /// <summary>
        /// Checks the next value without updating the generator (0 and 4 294 967 295).
        /// </summary>
        public uint CheckNextUInt()
        {
            return CheckNextUInt(1);
        }

        /// <summary>
        /// Checks the next value without updating the generator (-2 147 483 648 and 2 147 483 647).
        /// </summary>
        public int CheckNextInt()
        {
            return CheckNextInt(1);
        }

        /// <summary>
        /// Checks the next value without updating the generator (0 and 0.999999999).
        /// </summary>
        public float CheckNextFloat()
        {
            return CheckNextFloat(1);
        }

        /// <summary>
        /// Checks the next value without updating the generator (0 and 0.9999999997671694).
        /// </summary>
        public double CheckNextDouble()
        {
            return CheckNextDouble(1);
        }

        /// <summary>
        /// Checks the next value without updating the generator (0 and 255).
        /// </summary>
        public byte CheckNextByte()
        {
            return CheckNextByte(1);
        }

        #endregion

        #region CheckNext(steps)

        /// <summary>
        /// Checks the value in X steps without updating the generator (0 and 4 294 967 295).
        /// </summary>
        public uint CheckNextUInt(uint steps)
        {
            ulong oldState = m_State;
            Skip((int)steps - 1);
            uint nextValue = GenerateNewValue();
            m_State = oldState;

            return nextValue;
        }

        /// <summary>
        /// Checks the value in X steps without updating the generator (-2 147 483 648 and 2 147 483 647).
        /// </summary>
        public int CheckNextInt(uint steps)
        {
            ulong oldState = m_State;
            Skip((int)steps - 1);
            int nextValue = (int)(GenerateNewValue() >> 1);
            m_State = oldState;

            return nextValue;
        }

        /// <summary>
        /// Checks the value in X steps without updating the generator (0 and 0.999999999).
        /// </summary>
        public float CheckNextFloat(uint steps)
        {
            ulong oldState = m_State;
            Skip((int)steps - 1);
            float nextValue = (float)(GenerateNewValue() * INTTODOUBLE);
            m_State = oldState;

            return nextValue;
        }

        /// <summary>
        /// Checks the value in X steps without updating the generator (0 and 0.9999999997671694).
        /// </summary>
        public double CheckNextDouble(uint steps)
        {
            ulong oldState = m_State;
            Skip((int)steps - 1);
            double nextValue = GenerateNewValue() * INTTODOUBLE;
            m_State = oldState;

            return nextValue;
        }

        /// <summary>
        /// Checks the value in X steps without updating the generator (0 and 255).
        /// </summary>
        public byte CheckNextByte(uint steps)
        {
            ulong oldState = m_State;
            Skip((int)steps - 1);
            byte nextValue = (byte)(GenerateNewValue() % 256);
            m_State = oldState;

            return nextValue;
        }

        #endregion

        #region Skip / Backstep

        /// <summary>
        /// Skip a value in the sequence.
        /// </summary>
        public void Skip()
        {
            Skip(1);
        }

        /// <summary>
        /// Skip a specific amount of values in the sequence.
        /// </summary>
        public void Skip(int steps)
        {
            ulong currentMult = MULTIPLIER;
            ulong currentPlus = INCREMENT;

            ulong newMult = 1;
            ulong newPlus = 0;

            while (steps > 0)
            {
                if (BitwiseUtil.GetBitAtPosition(steps, 0) & 0x1 == 1)
                {
                    newMult *= currentMult;
                    newPlus = newPlus * currentMult + currentPlus;
                }

                currentPlus = (currentMult + 1) * currentPlus;
                currentMult *= currentMult;
                steps >>= 1;
            }

            m_State = newMult * m_State + newPlus;
        }

        /// <summary>
        /// Backsteps a value in the sequence.
        /// </summary>
        public void Back()
        {
            Skip(-1);
        }

        /// <summary>
        /// Backsteps a specific amount of values in the sequence.
        /// </summary>
        public void Back(int steps)
        {
            Skip(-steps);
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a RNG item that uses the environment's tick count as a seed.
        /// </summary>
        public RandomNumberGenerator() : this(Environment.TickCount) { }

        /// <summary>
        /// Creates a RNG item that uses a custom seed.
        /// </summary>
        public RandomNumberGenerator(int seed) : this((ulong)seed) { }

        private RandomNumberGenerator(ulong seed)
        {
            m_State = 0;
            NextUInt();
            m_State += seed;
            NextUInt();
        }

        #endregion

        private uint GenerateNewValue()
        {
            ulong oldState = m_State;
            m_State = unchecked(oldState * MULTIPLIER + INCREMENT);
            uint xorShifted = (uint)(((oldState >> 18) ^ oldState) >> 27);
            int shift = (int)(oldState >> 59);
            return (xorShifted >> shift) | (xorShifted << (-shift & 31));
        }
    }
}