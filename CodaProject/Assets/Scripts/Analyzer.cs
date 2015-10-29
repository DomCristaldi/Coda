using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Beat {
	public readonly float timeStamp;
	public readonly float frequency;
	public readonly float energy;
	
	Beat(float tStamp, float freq, float e) {
		timeStamp = tStamp;
		frequency = freq;
		energy = e;
	}
	
}

[System.Serializable]
public class Analyzer {
	
	public AudioClip clip;
	private const float a0 = 0.35875f;
	private const float a1 = 0.48829f;
	private const float a2 = 0.14128f;
	private const float a3 = 0.01168f;
	private List<Beat> _beatList;
	private double[] averages;
	
	int numPartitions = 10000;
	float overlapPercent = 0.5f;
	float inverseOverlap {
		get {
			return 1.0f / overlapPercent;
		}
	}


	public Analyzer() {
		_beatList = new List<Beat>();
	}
	
	public void ProcessAudio (AudioClip clip) {
		IEnumerator routine = ProcessAudioRoutine (clip).GetEnumerator ();
		while (routine.MoveNext()) {
			//yield return null;
		}
		
	}
	
	public IEnumerable ProcessAudioRoutine (AudioClip clip) {	
		averages = new double[(int)(numPartitions * inverseOverlap) - 1];
        int samplesPerPartition = (int)(clip.samples / numPartitions);

		//Debug.Log (clip.samples);
        //Debug.Log ((int)(((numPartitions * inverseOverlap) - 1) * samplesPerPartition * overlapPercent));
        //Debug.Log (samplesPerPartition * overlapPercent);

        /*
        float percentOfSong = 0.1f;
        float[] sampleArray = new float[(int) (clip.samples * percentOfSong)];

        clip.GetData(sampleArray, 0);

        for (int i = 0; i < sampleArray.Length - 1; ++i) {
            Debug.DrawLine(new Vector3((i - 1) / 100, (float)sampleArray[i] * 10, 0), new Vector3((i) / 100, (float)sampleArray[i + 1] * 10, 0), Color.red);
        }
        */

		//for(int i = 0; i < 10000; i++) {
		int numDivisions = (int)(numPartitions * inverseOverlap) -1;
		for (int i = 0; i < numDivisions; i++) {
        //for (int i = 0; i < 0; ++i) {
            //Debug.Log ("" + i + " " + (i * (samplesPerPartition) * overlapPercent));
            //Debug.Log((int)(i * samplesPerPartition * overlapPercent));
			//Debug.Log("" + i + " / " + ((numPartitions * inverseOverlap) - 1));

            float[] samples = new float[samplesPerPartition];
            
            //Debug.Log((int)(i * (samples.Length * overlapPercent * clip.channels)));

           // if ((int)(i * (samples.Length * overlapPercent)) > 8000000) {
                //Debug.Log("boom");

                //Debug.Log(clip.samples);

                //Debug.LogFormat("samplesPerpartition: {0}", samplesPerPartition);

                int input = i * ((int) (samples.Length * overlapPercent));

                //Debug.Log(input);

                clip.GetData(samples, input);

                //Debug.DrawLine(new Vector3(Mathf.Log(i - 1), (float)samples[i] * 10, 0), new Vector3(Mathf.Log(i), (float)samples[i + 1] * 10, 0), Color.red);
            
           // }
            
			for (int n = 0; n < samples.Length; n++) {
				samples [n] *= a0 - a1 * Mathf.Cos ((2 * Mathf.PI * n) / samples.Length - 1) + a2 * Mathf.Cos ((4 * Mathf.PI * n) / samples.Length - 1) - a3 * Mathf.Cos ((6 * Mathf.PI * n) / samples.Length - 1);
			}
			
			FFT2 test = new FFT2 ();
			test.init (10);
			double[] double_samples = samples.ToList ().ConvertAll<double> (new System.Converter<float, double> (f2d)).ToArray ();
			test.run (double_samples, new double[samples.Length], false);
			

			double avg = double_samples.Average ();
			averages[i] = avg;
			/*if(avg > 0)
			{
				Debug.Log ("iteration " + i + " average: " + avg);
			}*/
			yield return null;
		}
        
        for(int i = 1; i < averages.Length-1; i++) {
            float xScaling = 0.01f;
            float yScaling = 175.0f;

            Vector3 drawStartPos = new Vector3((i - 1) * xScaling,
                                               ((float) averages[i - 1]) * yScaling,
                                               0.0f);
            Vector3 drawEndPos = new Vector3(i * xScaling,
                                             ((float)averages[i]) * yScaling,
                                             0.0f);

            Debug.DrawLine(drawStartPos, drawEndPos, Color.red);
            
            //Debug.DrawLine(new Vector3((i - 1) / 1000, (float)averages[i] * 10, 0), new Vector3((i) / 1000, (float)averages[i + 1] * 10, 0), Color.red);

            //Debug.DrawLine(new Vector3((i - 1) / 1000, (float)averages[i] * 10, 0), new Vector3((i) / 1000, (float)averages[i + 1] * 10, 0), Color.red);
            //Debug.DrawLine(new Vector3(Mathf.Log(i - 1), (float)averages[i] * 10, 0), new Vector3(Mathf.Log(i), (float)averages[i + 1] * 10, 0), Color.red);
        }
        
        yield break;
	}
	
	public static double f2d(float f) {
		return (double)f;
	}
}
