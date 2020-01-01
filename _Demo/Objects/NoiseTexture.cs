using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoiseTexture : MonoBehaviour
{
    public enum Noise
    {
        White,
        Blue,
        Red,
        Wave,
        Compound
    }

    public Noise ChosenNoise;

    // Start is called before the first frame update
    void Start()
    {
        int textureSize = 512;
        Texture2D newTex = new Texture2D(textureSize, textureSize);

        switch(ChosenNoise)
        {
            case Noise.White:
                {
                    Canty.RandomNumberGenerator whiteRNG = new Canty.RandomNumberGenerator();

                    for(int x = 0; x < textureSize; x++)
                    {
                        for (int y = 0; y < textureSize; y++)
                        {
                            float next = whiteRNG.NextFloat();
                            newTex.SetPixel(x, y, new Color(next, next, next));
                        }
                    }

                    newTex.Apply();
                }
                break;

            case Noise.Blue:
                {
                    Canty.BlueNoiseGenerator blueRNG = new Canty.BlueNoiseGenerator();
                    newTex = blueRNG.NextTexture(textureSize, textureSize);
                }
                break;

            case Noise.Red:
                {
                    Canty.RedNoiseGenerator redRNG = new Canty.RedNoiseGenerator();
                    newTex = redRNG.NextTexture(textureSize, textureSize);
                }
                break;

            case Noise.Wave:
                {
                    Canty.SineWaveGenerator waveRNG = new Canty.SineWaveGenerator(64, 0);
                    newTex = waveRNG.NextTexture(textureSize, textureSize);
                }
                break;

            case Noise.Compound:
                {
                    Canty.SineWaveGenerator waveRNG = new Canty.SineWaveGenerator(64, 0);
                    Canty.WeightedCompoundNoiseGenerators compoundRNG = new Canty.WeightedCompoundNoiseGenerators
                        (
                        new Canty.WeightedCompoundNoiseGenerators.NoiseGeneratorPair(waveRNG, 0.5f),
                        new Canty.WeightedCompoundNoiseGenerators.NoiseGeneratorPair(new Canty.RedNoiseGenerator(), 0.5f)
                        );

                    newTex = compoundRNG.NextTexture(textureSize, textureSize);
                }
                break;
        }

        GetComponent<Image>().material.mainTexture = newTex;
    }
}
