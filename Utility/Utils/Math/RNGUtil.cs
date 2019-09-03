///====================================================================================================
///
///     NoiseUtil by
///     - CantyCanadian
///
///====================================================================================================

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
        private const ulong MULTIPLIER = 6364136223846793005ul;
        private const double INTTODOUBLE = 1.0 / 4294967296.0;

        private ulong m_State;
        private ulong m_Increment;

        #region Next()

        /// <summary>
        /// Returns value between 0 and 4 294 967 295.
        /// </summary>
        public uint Next()
        {
            return GenerateNewValue();
        }

        /// <summary>
        /// Returns value between -2 147 483 648 and 2 147 483 647.
        /// </summary>
        public int Next()
        {
            return (int)(GenerateNewValue() >> 1);
        }

        /// <summary>
        /// Returns value between 0 and 0.999999999.
        /// </summary>
        public float Next()
        {
            return (float)(GenerateNewValue() * INTTODOUBLE);
        }

        /// <summary>
        /// Returns value between 0 and 0.9999999997671694.
        /// </summary>
        public double Next()
        {
            return GenerateNewValue() * INTTODOUBLE;
        }

        /// <summary>
        /// Returns value between 0 and 255.
        /// </summary>
        public byte Next()
        {
            return (byte)(GenerateNewValue() % 256);
        }

        /// <summary>
        /// Returns either 0 or 1.
        /// </summary>
        public bool Next()
        {
            return GenerateNewValue() % 2 == 1;
        }

        /// <summary>
        /// Returns a random enum from a passed-in type.
        /// </summary>
        public Enum Next<Enum>() where Enum : struct, IConvertible
        {
            Array values = Enum.GetValues(typeof(Enum));
            return (Enum)values.GetValue(Next(values.Length));
        }

        /// <summary>
        /// Returns a random value from a passed-in array.
        /// </summary>
        public T Next<T>(T[] values)
        {
            return values.GetValue(Next(values.Length));
        }

        #endregion

        #region Next(maxExclusive)

        /// <summary>
        /// Retuns a value between 0 and maxExclusive.
        /// </summary>
        public uint Next(uint maxExclusive)
        {
            return GenerateNewValue() % maxExclusive;
        }

        /// <summary>
        /// Retuns a value between 0 and maxExclusive (or vice versa if negative).
        /// </summary>
        public int Next(int maxExclusive)
        {
            return (int)(GenerateNewValue() >> 1) % maxExclusive;
        }

        /// <summary>
        /// Retuns a value between 0 and maxExclusive (or vice versa if negative).
        /// </summary>
        public float Next(float max)
        {
            return (float)(GenerateNewValue() * INTTODOUBLE) * maxExclusive;
        }

        /// <summary>
        /// Retuns a value between 0 and maxExclusive (or vice versa if negative).
        /// </summary>
        public double Next(double maxExclusive)
        {
            return (GenerateNewValue() * INTTODOUBLE) * maxExclusive;
        }

        /// <summary>
        /// Retuns a value between 0 and maxExclusive.
        /// </summary>
        public byte Next(byte maxExclusive)
        {
            return (byte)(GenerateNewValue() % 256) % maxExclusive;
        }

        #endregion

        #region Next(minInclusive, maxExclusive)

        /// <summary>
        /// Retuns a value between minInclusive and maxExclusive. Returns 0 if max is under min.
        /// </summary>
        public uint Next(uint minInclusive, uint maxExclusive)
        {
            return maxExclusive > minInclusive ? GenerateNewValue() % (maxExclusive - minInclusive) + minInclusive : 0;
        }

        /// <summary>
        /// Retuns a value between minInclusive and maxExclusive. Returns 0 if max is under min.
        /// </summary>
        public int Next(int minInclusive, int maxExclusive)
        {
            return maxExclusive > minInclusive ? (int)(GenerateNewValue() >> 1) % (maxExclusive - minInclusive) + minInclusive : 0;
        }

        /// <summary>
        /// Retuns a value between minInclusive and maxExclusive. Returns 0 if max is under min.
        /// </summary>
        public float Next(float minInclusive, float maxExclusive)
        {
            return maxExclusive > minInclusive ? (float)(GenerateNewValue() * INTTODOUBLE) % (maxExclusive - minInclusive) + minInclusive : 0;
        }

        /// <summary>
        /// Retuns a value between minInclusive and maxExclusive. Returns 0 if max is under min.
        /// </summary>
        public double Next(double minInclusive, double maxExclusive)
        {
            return maxExclusive > minInclusive ? (GenerateNewValue() * INTTODOUBLE) % (maxExclusive - minInclusive) + minInclusive : 0;
        }

        /// <summary>
        /// Retuns a value between minInclusive and maxExclusive. Returns 0 if max is under min.
        /// </summary>
        public byte Next(byte minInclusive, byte maxExclusive)
        {
            return maxExclusive > minInclusive ? (byte)(GenerateNewValue() % 256) % (maxExclusive - minInclusive) + minInclusive : 0;
        }

        #endregion

        #region Nexts(count)

        /// <summary>
        /// Retuns an array of values between 0 and 4 294 967 295.
        /// </summary>
        public List<uint> Nexts(uint count)
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
        public List<int> Nexts(uint count)
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
        public List<float> Nexts(uint count)
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
        public List<double> Nexts(uint count)
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
        public List<byte> Nexts(uint count)
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
        public List<bool> Nexts(uint count)
        {
            bool[] items = new bool[count];

            for (int i = 0; i < count; i++)
            {
                items.Add((byte)(GenerateNewValue() % 256));
            }

            return GenerateNewValue() % 2 == 1;
        }

        #endregion

        #region Nexts(count, unique)

        /// <summary>
        /// Returns multiple random enums from a passed-in type.
        /// If unique, no values in the list will duplicate.
        /// </summary>
        public Enum Next<Enum>(uint count, bool unique = false) where Enum : struct, IConvertible
        {
            Array values = Enum.GetValues(typeof(Enum));

            if (unique && count > values.Length)
            {
                Debug.Error("RNGUtil : Impossible to get an unique list due to lack of available values.");
                return new Enum[count];
            }

            List<Enum> items = new List<Enum>();

            if (unique)
            {
                for (int i = 0; i < count; i++)
                {
                    while (items.length <= i)
                    {
                        uint value = GenerateNewValue() % values.Length;

                        if (!items.contains(values[value]))
                        {
                            items.Add((Enum)values.GetValue(value));
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    items.Add((Enum)values.GetValue(GenerateNewValue() % values.Length));
                }
            }

            return items;
        }

        /// <summary>
        /// Returns multiple random values from a passed-in array.
        /// If unique, no values in the list will duplicate.
        /// </summary>
        public T Next<T>(T[] values, uint count, bool unique = false)
        {
            if (unique && count > values.Length)
            {
                Debug.Error("RNGUtil : Impossible to get an unique list due to lack of available values.");
                return new T[count];
            }

            List<T> items = new List<T>();

            if (unique)
            {
                for (int i = 0; i < count; i++)
                {
                    while (items.length <= i)
                    {
                        uint value = GenerateNewValue() % values.Length;

                        if (!items.contains(values[value]))
                        {
                            items.Add(values.GetValue(value));
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    items.Add(values.GetValue(GenerateNewValue() % values.Length));
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
        public uint Nexts(uint count, uint maxExclusive, bool unique = false)
        {
            if (unique && count > maxExclusive)
            {
                Debug.Error("RNGUtil : Impossible to get an unique list due to lack of available values.");
                return new uint[count];
            }

            List<uint> items = new List<uint>();

            if (unique)
            {
                for (int i = 0; i < count; i++)
                {
                    while (items.length <= i)
                    {
                        uint value = GenerateNewValue() % maxExclusive;

                        if (!items.contains(value))
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
        public int Nexts(uint count, uint maxExclusive, bool unique = false)
        {
            if (unique && count > maxExclusive)
            {
                Debug.Error("RNGUtil : Impossible to get an unique list due to lack of available values.");
                return new int[count];
            }

            int[] items = new int[count];

            if (unique)
            {
                for (int i = 0; i < count; i++)
                {
                    while (items.length <= i)
                    {
                        int value = (int)(GenerateNewValue() >> 1) % maxExclusive;

                        if (!items.contains(value))
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
                    items.Add((int)(GenerateNewValue() >> 1) % maxExclusive);
                }
            }

            return items;
        }

        /// <summary>
        /// Retuns an array of values between 0 and maxExclusive (or vice versa if negative).
        /// If unique, no numbers in the list will duplicate.
        /// </summary>
        public float Nexts(uint count, float maxExclusive, bool unique = false)
        {
            float[] items = new float[count];

            if (unique)
            {
                for (int i = 0; i < count; i++)
                {
                    while (items.length <= i)
                    {
                        int value = (float)(GenerateNewValue() * INTTODOUBLE) * maxExclusive;

                        if (!items.contains(value))
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
        public double Nexts(uint count, double maxExclusive, bool unique = false)
        {
            double[] items = new double[count];

            if (unique)
            {
                for (int i = 0; i < count; i++)
                {
                    while (items.length <= i)
                    {
                        int value = (GenerateNewValue() * INTTODOUBLE) * maxExclusive;

                        if (!items.contains(value))
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
                    items.Add((float)((GenerateNewValue() * INTTODOUBLE) * maxExclusive));
                }
            }

            return items;
        }

        /// <summary>
        /// Retuns an array of values between 0 and maxExclusive.
        /// If unique, no numbers in the list will duplicate.
        /// </summary>
        public byte Nexts(uint count, byte maxExclusive, bool unique = false)
        {
            if (unique && count > maxExclusive)
            {
                Debug.Error("RNGUtil : Impossible to get an unique list due to lack of available values.");
                return new byte[count];
            }

            byte[] items = new byte[count];

            for (int i = 0; i < count; i++)
            {
                items[i] = (byte)(GenerateNewValue() % 256) % maxExclusive;
            }

            if (unique)
            {
                for (int i = 0; i < count; i++)
                {
                    while (items.length <= i)
                    {
                        int value = (byte)(GenerateNewValue() % 256) % maxExclusive;

                        if (!items.contains(value))
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
                    items.Add((byte)(GenerateNewValue() % 256) % maxExclusive);
                }
            }

            return items;
        }

        #endregion

        #region Nexts(count, maxExclusive, minInclusive, unique)

        /// <summary>
        /// Retuns an array of values between minExclusive and maxExclusive. Returns 0 if max is under min.
        /// </summary>
        public uint Nexts(uint count, uint maxExclusive, uint minExclusive, bool unique = false)
        {
            if (unique && count > (maxExclusive - minExclusive))
            {
                Debug.Error("RNGUtil : Impossible to get an unique list due to lack of available values.");
                return new uint[count];
            }

            uint[] items = new uint[count];

            if (unique)
            {
                for (int i = 0; i < count; i++)
                {
                    while (items.length <= i)
                    {
                        int value = maxExclusive > minInclusive ? GenerateNewValue() % (maxExclusive - minInclusive) + minInclusive : 0;

                        if (!items.contains(value))
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
        /// Retuns an array of values minExclusive and maxExclusive. Returns 0 if max is under min.
        /// </summary>
        public int Nexts(uint count, int maxExclusive, int minExclusive, bool unique = false)
        {
            if (unique && count > (maxExclusive - minExclusive))
            {
                Debug.Error("RNGUtil : Impossible to get an unique list due to lack of available values.");
                return new uint[count];
            }

            int[] items = new int[count];

            if (unique)
            {
                for (int i = 0; i < count; i++)
                {
                    while (items.length <= i)
                    {
                        int value = maxExclusive > minInclusive ? (int)(GenerateNewValue() >> 1) % (maxExclusive - minInclusive) + minInclusive : 0;

                        if (!items.contains(value))
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
        /// Retuns an array of values between minExclusive and maxExclusive. Returns 0 if max is under min.
        /// </summary>
        public float Nexts(uint count, float maxExclusive, float minExclusive, bool unique = false)
        {
            float[] items = new float[count];

            if (unique)
            {
                for (int i = 0; i < count; i++)
                {
                    while (items.length <= i)
                    {
                        int value = maxExclusive > minInclusive ? (float)(GenerateNewValue() * INTTODOUBLE) % (maxExclusive - minInclusive) + minInclusive : 0;

                        if (!items.contains(value))
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
        /// Retuns an array of values between minExclusive and maxExclusive. Returns 0 if max is under min.
        /// </summary>
        public double Nexts(uint count, double maxExclusive, double minExclusive, bool unique = false)
        {
            double[] items = new double[count];

            if (unique)
            {
                for (int i = 0; i < count; i++)
                {
                    while (items.length <= i)
                    {
                        int value = maxExclusive > minInclusive ? (GenerateNewValue() * INTTODOUBLE) % (maxExclusive - minInclusive) + minInclusive : 0;

                        if (!items.contains(value))
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
        public byte Nexts(uint count, byte maxExclusive, byte minExclusive, bool unique = false)
        {
            if (unique && count > (maxExclusive - minExclusive))
            {
                Debug.Error("RNGUtil : Impossible to get an unique list due to lack of available values.");
                return new uint[count];
            }

            byte[] items = new byte[count];

            if (unique)
            {
                for (int i = 0; i < count; i++)
                {
                    while (items.length <= i)
                    {
                        int value = maxExclusive > minInclusive ? (byte)(GenerateNewValue() % 256) % (maxExclusive - minInclusive) + minInclusive : 0;

                        if (!items.contains(value))
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
                    items.Add(maxExclusive > minInclusive ? (byte)(GenerateNewValue() % 256) % (maxExclusive - minInclusive) + minInclusive : 0);
                }
            }

            return items;
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
            ulong currentPlus = m_Increment;

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
            m_Increment = (721347520444481703ul << 1) | 1;
            NextUInt();
            m_State += seed;
            NextUInt();
        }

        #endregion

        private uint GenerateNewValue()
        {
            ulong oldState = m_State;
            m_State = unchecked(oldState * MULTIPLIER + m_Increment);
            uint xorShifted = (uint)(((oldState >> 18) ^ oldState) >> 27);
            int shift = (int)(oldState >> 59);
            return (xorShifted >> shift) | (xorShifted << (-shift & 31));
        }
    }
}