using System;
using System.Collections;
using System.Collections.Generic;
using Canty;
using UnityEngine;

public class PCG
{
    private const ulong MULTIPLIER = 6364136223846793005ul;

    private ulong m_State;
    private ulong m_Increment;

    public T Next<T>()
    {
        Type type = typeof(T);

    }

    /*
     *
     * public int Next();

public int Next(int maxExclusive);

public int Next(int minInclusive, int maxExclusive);

public int[] NextInts(int count)

public int[] NextInts(int count, int maxExclusive)

public int[] NextInts(int count, int minInclusive, int maxExclusive)

public uint NextUInt()

public uint NextUInt(uint maxExclusive);

public uint NextUInt(uint minInclusive, uint maxExclusive);

public uint[] NextUInts(int count);

public uint[] NextUInts(int count, uint maxExclusive);

public uint[] NextUInts(int count, uint minInclusive, uint maxExclusive);

public float NextFloat();

public float NextFloat(float maxInclusive);

public float NextFloat(float minInclusive, float maxInclusive);

public float[] NextFloats(int count);

public float[] NextFloats(int count, float maxInclusive);

public float[] NextFloats(int count, float minInclusive, float maxInclusive);

public double NextDouble();

public double NextDouble(double maxInclusive);

public double NextDouble(double minInclusive, double maxInclusive);

public double[] NextDoubles(int count);

public double[] NextDoubles(int count, double maxInclusive);

public double[] NextDoubles(int count, double minInclusive, double maxInclusive);

public byte NextByte();

public byte[] NextBytes(int count);
      
public bool NextBool();

public bool[] NextBools(int count);
     */

    public uint NextUInt()
    {
        ulong oldState = m_State;
        m_State = unchecked(oldState * MULTIPLIER + m_Increment);
        uint xorShifted = (uint) (((oldState >> 18) ^ oldState) >> 27);
        int shift = (int) (oldState >> 59);
        return (xorShifted >> shift) | (xorShifted << (-shift & 31));
    }

    public void Skip()
    {
        Skip(1);
    }

    public void Backstep()
    {
        Skip(-1);
    }

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

    public void Backstep(int steps)
    {
        Skip(-steps);
    }

    public PCG() : this(Environment.TickCount) { }

    public PCG(int seed) : this((ulong)seed) { }

    public PCG(int seed, int sequence) : this((ulong)seed, (ulong)sequence) { }

    public PCG(ulong seed, ulong sequence = 721347520444481703ul)
    {
        m_State = 0;
        m_Increment = (sequence << 1) | 1;
        NextUInt();
        m_State += seed;
        NextUInt();
    }
}
