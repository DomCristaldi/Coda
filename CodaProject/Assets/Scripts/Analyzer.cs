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
	
	int numPartitions = 5000;
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
		
		//Debug.Log (clip.samples);
		//Debug.Log ((int)(((numPartitions * inverseOverlap) - 1) * clip.samples / numPartitions * overlapPercent));
		//Debug.Log (clip.samples / numPartitions * overlapPercent);

		//for(int i = 0; i < 10000; i++) {
		int numDivisions = (int)(numPartitions * inverseOverlap) -1;
		for (int i = 0; i < numDivisions; i++) {
			//Debug.Log ("" + i + " " + (i * (clip.samples / numPartitions) * overlapPercent));
			//Debug.Log((int)(i * clip.samples / numPartitions * overlapPercent));
			//Debug.Log("" + i + " / " + ((numPartitions * inverseOverlap) - 1));
			
			float[] samples = new float[clip.samples / numPartitions * clip.channels];
			clip.GetData (samples, (int)(i * ((clip.samples / numPartitions) * overlapPercent)));
			
			for (int n = 0; n < samples.Length; n++) {
				samples [n] *= a0 - a1 * Mathf.Cos ((2 * Mathf.PI * n) / samples.Length - 1) + a2 * Mathf.Cos ((4 * Mathf.PI * n) / samples.Length - 1) - a3 * Mathf.Cos ((6 * Mathf.PI * n) / samples.Length - 1);
			}
			
			FFT2 test = new FFT2 ();
			test.init (10);
			double[] double_samples = samples.ToList ().ConvertAll<double> (new System.Converter<float, double> (f2d)).ToArray ();
			test.run (double_samples, new double[samples.Length], false);
			
			double avg = samples.Average ();
			averages[i] = avg;
			/*if(avg > 0)
			{
				Debug.Log ("iteration " + i + " average: " + avg);
			}*/
			yield return null;
		}
		
		for(int i = 1; i < averages.Length-1; i++) {
			
			Debug.DrawLine(new Vector3(Mathf.Log(i - 1), (float)averages[i] + 10, 0), new Vector3(Mathf.Log(i), (float)averages[i + 1] + 10, 0), Color.red);
		}
		yield break;
	}
	
	public static double f2d(float f) {
		return (double)f;
	}
}
