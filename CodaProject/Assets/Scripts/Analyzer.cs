using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public struct Beat
{
    private float timeStamp;
    private float frequency;
}

[System.Serializable]
public class Analyzer{

	public AudioClip clip;
	private const float a0 = 0.35875f;
	private const float a1 = 0.48829f;
	private const float a2 = 0.14128f;
	private const float a3 = 0.01168f;
    private List<Beat> _beatList;

	public Analyzer() {
        _beatList = new List<Beat>();
    }

	public void ProcessAudio (AudioClip clip) {	
			
		float[] samples = new float[clip.samples/20000 * clip.channels];
		
		clip.GetData(samples, 16000);

		
		for(int n = 0; n < samples.Length; n++) {
			samples[n] *= a0 - a1 * Mathf.Cos((2 * Mathf.PI * n)/samples.Length-1) + a2 * Mathf.Cos((4*Mathf.PI * n)/samples.Length-1) - a3 * Mathf.Cos((6 * Mathf.PI * n)/samples.Length-1);
		}
		
		FFT2 test = new FFT2();
		test.init(10);
		double[] double_samples = samples.ToList ().ConvertAll<double>(new System.Converter<float, double>(f2d)).ToArray ();
		test.run (double_samples, new double[samples.Length], false);

		double avg = samples.Average ();	
		Debug.Log (avg);

		for(int i = 1; i < samples.Length-1; i++) {
			Debug.DrawLine(new Vector3(Mathf.Log(i - 1), samples[i] + 10, 0), new Vector3(Mathf.Log(i), samples[i + 1] + 10, 0), Color.red);
		}
		
	}

	public static double f2d(float f) {
		return (double)f;
	}

	public static float d2f(double d) {
		return (float)d;
	}
	
}
