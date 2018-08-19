using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://github.com/keijiro/PerlinNoise
public static class NoiseUtil
{
    #region Perlin Noise functions

    public static float PerlinNoise(float x)
    {
        int floorLast8BitsX = Mathf.FloorToInt(x) & 0xff;

        float floorX = Mathf.Floor(x);

        return Mathf.Lerp(Fade(floorX), Gradient(PermanentValues[floorLast8BitsX], floorX), Gradient(PermanentValues[floorLast8BitsX + 1], floorX - 1)) * 2;
    }

    public static float PerlinNoise(float x, float y)
    {
        int floorLast8BitsX = Mathf.FloorToInt(x) & 0xff;
        int floorLast8BitsY = Mathf.FloorToInt(y) & 0xff;

        float floorX = Mathf.Floor(x);
        float floorY = Mathf.Floor(y);

        float fadeX = Fade(floorX);
        float fadeY = Fade(floorY);

        int last8Bits1 = (PermanentValues[floorLast8BitsX] + floorLast8BitsY) & 0xff;
        int last8Bits2 = (PermanentValues[floorLast8BitsX + 1] + floorLast8BitsY) & 0xff;

        return Mathf.Lerp(fadeY, Mathf.Lerp(fadeX, Gradient(PermanentValues[last8Bits1], floorX, floorY), Gradient(PermanentValues[last8Bits2], floorX - 1, floorY)), Mathf.Lerp(fadeX, Gradient(PermanentValues[last8Bits1 + 1], floorX, floorY - 1), Gradient(PermanentValues[last8Bits2 + 1], floorX - 1, floorY - 1)));
    }

    public static float PerlinNoise(Vector2 coord)
    {
        return PerlinNoise(coord.x, coord.y);
    }

    public static float PerlinNoise(float x, float y, float z)
    {
        int floorLast8BitsX = Mathf.FloorToInt(x) & 0xff;
        int floorLast8BitsY = Mathf.FloorToInt(y) & 0xff;
        int floorLast8BitsZ = Mathf.FloorToInt(z) & 0xff;

        float floorX = Mathf.Floor(x);
        float floorY = Mathf.Floor(y);
        float floorZ = Mathf.Floor(z);

        float fadeX = Fade(floorX);
        float fadeY = Fade(floorY);
        float fadeZ = Fade(floorZ);

        int last8BitsA = (PermanentValues[floorLast8BitsX] + floorLast8BitsY) & 0xff;
        int last8BitsB = (PermanentValues[floorLast8BitsX + 1] + floorLast8BitsY) & 0xff;
        int last8BitsAA = (PermanentValues[last8BitsA] + floorLast8BitsZ) & 0xff;
        int last8BitsBA = (PermanentValues[last8BitsB] + floorLast8BitsZ) & 0xff;
        int last8BitsAB = (PermanentValues[last8BitsA + 1] + floorLast8BitsZ) & 0xff;
        int last8BitsBB = (PermanentValues[last8BitsB + 1] + floorLast8BitsZ) & 0xff;

        return Mathf.Lerp(fadeZ, Mathf.Lerp(fadeY, Mathf.Lerp(fadeX, Gradient(PermanentValues[last8BitsAA], floorX, floorY, floorZ), Gradient(PermanentValues[last8BitsBA], floorX - 1, floorY, floorZ)), Mathf.Lerp(fadeX, Gradient(PermanentValues[last8BitsAB], floorX, floorY - 1, floorZ), Gradient(PermanentValues[last8BitsBB], floorX - 1, floorY - 1, floorZ))), Mathf.Lerp(fadeY, Mathf.Lerp(fadeX, Gradient(PermanentValues[last8BitsAA + 1], floorX, floorY, floorZ - 1), Gradient(PermanentValues[last8BitsBA + 1], floorX - 1, floorY, floorZ - 1)), Mathf.Lerp(fadeX, Gradient(PermanentValues[last8BitsAB + 1], floorX, floorY - 1, floorZ - 1), Gradient(PermanentValues[last8BitsBB + 1], floorX - 1, floorY - 1, floorZ - 1))));
    }

    public static float PerlinNoise(Vector3 coord)
    {
        return PerlinNoise(coord.x, coord.y, coord.z);
    }

    #endregion

    #region Fractional Brownian Motion functions

    public static float FractionalBrownianMotion(float x, int octave)
    {
        float value = 0.0f;
        float amplitude = 0.5f;
        for (var i = 0; i < octave; i++)
        {
            value += amplitude * PerlinNoise(x);
            x *= 2.0f;
            amplitude *= 0.5f;
        }
        return value;
    }

    public static float FractionalBrownianMotion(Vector2 coord, int octave)
    {
        float value = 0.0f;
        float amplitude = 0.5f;
        for (var i = 0; i < octave; i++)
        {
            value += amplitude * PerlinNoise(coord);
            coord *= 2.0f;
            amplitude *= 0.5f;
        }
        return value;
    }

    public static float FractionalBrownianMotion(float x, float y, int octave)
    {
        return FractionalBrownianMotion(new Vector2(x, y), octave);
    }

    public static float FractionalBrownianMotion(Vector3 coord, int octave)
    {
        float value = 0.0f;
        float amplitude = 0.5f;
        for (var i = 0; i < octave; i++)
        {
            value += amplitude * PerlinNoise(coord);
            coord *= 2.0f;
            amplitude *= 0.5f;
        }
        return value;
    }

    public static float FractionalBrownianMotion(float x, float y, float z, int octave)
    {
        return FractionalBrownianMotion(new Vector3(x, y, z), octave);
    }

    #endregion

    #region Private functions

    static float Fade(float t)
    {
        return t * t * t * (t * (t * 6 - 15) + 10);
    }

    static float Gradient(int hash, float x)
    {
        return (hash & 1) == 0 ? x : -x;
    }

    static float Gradient(int hash, float x, float y)
    {
        return ((hash & 1) == 0 ? x : -x) + ((hash & 2) == 0 ? y : -y);
    }

    static float Gradient(int hash, float x, float y, float z)
    {
        var h = hash & 15;
        var u = h < 8 ? x : y;
        var v = h < 4 ? y : (h == 12 || h == 14 ? x : z);
        return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);
    }

    static int[] PermanentValues = 
        {
        151,160,137,91,90,15,
        131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,
        190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
        88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
        77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
        102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196,
        135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,
        5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
        223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,
        129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
        251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
        49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
        138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180,
        151
        };

    #endregion
}