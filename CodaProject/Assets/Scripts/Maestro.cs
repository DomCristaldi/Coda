using UnityEngine;
using System.Collections;

namespace Coda {

	[RequireComponent(typeof(AudioSource))]
	public class Maestro : MonoBehaviour {

	    public static Maestro current = null;

	    public int volumeSamples = 256;
	    public int frequencySamples = 8192;
	    public int sampleRate = 44100;

	    public float frequency
	    {
	        get { return _freq; }
	    }

	    private AudioSource _audio;
	    private float _freq;

	    void Awake()
	    {
	        if (current == null)
	        {
	            current = this;
	        }
	        _audio = GetComponent<AudioSource>();
	    }

		void Start () {
	        _audio.Play();
		}

	    private float[] DoFFT(int sampleNum)
	    {
	        float[] data = new float[sampleNum];
	        _audio.GetSpectrumData(data, 0, FFTWindow.BlackmanHarris);
	        return data;
	    }

	    /* Credit to kaappine.fi */
	    public float GetAveragedVolume()
	    {
	        float[] data = new float[volumeSamples];
	        float avg = 0.0f;
	        _audio.GetOutputData(data, 0);
	        foreach (float f in data)
	        {
	            avg += f;
	        }
	        avg /= volumeSamples;
	        return avg;
	    }

	    public float GetAverageFrequency()
	    {
	        float[] data = DoFFT(frequencySamples);
	        float avg = 0;
	        for (int i = 0; i < frequencySamples; i++)
	        {
	            avg += data[i];
	        }
	        avg /= frequencySamples;
	        return avg;
	    }

	    /* Credit to kaappine.fi */
	    public float GetFundamentalFrequency() {
	        float[] data = DoFFT(frequencySamples);
	        int i = 0;          //index of the bin with strongest frequency
	        float s = 0f;       //strongest frequency
	        for (int j = 1; j < frequencySamples; j++)
	        {
	            if (data[j] > s)
	            {
	                s = data[j];
	                i = j;
	            }
	        }

	        float freqIndex = i;
	        if (i > 0 && i < frequencySamples - 1)
	        {
	            float dL = data[i - 1] / data[i];
	            float dR = data[i + 1] / data[i];
	            freqIndex += 0.5f * (dR * dR - dL * dL);
	        }

	        return freqIndex * (AudioSettings.outputSampleRate) / frequencySamples;

	        // funFreq = (float)(i * sampleRate) / frequencySamples;
	    }
		
		void Update () {
	        _freq = GetFundamentalFrequency();
	        Debug.Log(_freq);
		}
	}

}
