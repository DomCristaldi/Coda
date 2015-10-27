using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Analyzer : MonoBehaviour {

	public AudioClip clip;
	private float a0 = 0.35875f;
	private float a1 = 0.48829f;
	private float a2 = 0.14128f;
	private float a3 = 0.01168f;
	

	// Use this for initialization
	void Start () 
	{	
			
		float[] samples = new float[clip.samples/20000 * clip.channels];
		//print (samples.Length);
		
		//print (clip.samples/1000);
		clip.GetData(samples, 16000);

		//print ("num samples: " + samples.Length);
		
		for(int n = 0; n < samples.Length; n++)
		{
			samples[n] *= a0 - a1 * Mathf.Cos((2 * Mathf.PI * n)/samples.Length-1) + a2 * Mathf.Cos((4*Mathf.PI * n)/samples.Length-1) - a3 * Mathf.Cos((6 * Mathf.PI * n)/samples.Length-1);
		}
		
		FFT2 test = new FFT2();
		test.init(10);
		double[] double_samples = samples.ToList ().ConvertAll<double>(new System.Converter<float, double>(f2d)).ToArray ();
		test.run (double_samples, new double[samples.Length], false);
		//samples = double_samples.ToList ().ConvertAll<float> (new System.Converter<double, float> (d2f)).ToArray ();

		double avg = samples.Average ();	
		print (avg);

		for(int i = 1; i < samples.Length-1; i++)
		{
			Debug.DrawLine(new Vector3(Mathf.Log(i - 1), samples[i] + 10, 0), new Vector3(Mathf.Log(i), samples[i + 1] + 10, 0), Color.red);
		}
		
	}

	public static double f2d(float f)
	{
		return (double)f;
	}

	public static float d2f(double d)
	{
		return (float)d;
	}
	
}
