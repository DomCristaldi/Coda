using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


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
	
	
	public double[] ProcessAudio (AudioClip clip) {	
		averages = new double[(int)(numPartitions * inverseOverlap) - 1];
        int samplesPerPartition = (int)(clip.samples / numPartitions);


		int numDivisions = (int)(numPartitions * inverseOverlap) -1;
		for (int i = 0; i < numDivisions; i++) {

            float[] samples = new float[samplesPerPartition];
            

                int input = i * ((int) (samples.Length * overlapPercent));

                //Debug.Log(input);

                clip.GetData(samples, input);

                //Debug.DrawLine(new Vector3(Mathf.Log(i - 1), (float)samples[i] * 10, 0), new Vector3(Mathf.Log(i), (float)samples[i + 1] * 10, 0), Color.red);
            
           // }
            
			for (int n = 0; n < samples.Length; n++) {
				samples [n] *= a0 - a1 * Mathf.Cos ((2 * Mathf.PI * n) / samples.Length - 1) + a2 * Mathf.Cos ((4 * Mathf.PI * n) / samples.Length - 1) - a3 * Mathf.Cos ((6 * Mathf.PI * n) / samples.Length - 1);
			}
			
			FFT2 test = new FFT2 ();
			test.init ((uint)Mathf.Log(samplesPerPartition,2));
			double[] double_samples = samples.ToList ().ConvertAll<double> (new System.Converter<float, double> (f2d)).ToArray ();
			test.run (double_samples, new double[samples.Length], false);
			

			double avg = double_samples.Average ();
			averages[i] = avg;

		}
		
		AnalyzeData(averages);
        return averages;
	}

	public void DrawData(double[] data) {
		for(int i = 1; i < averages.Length-1; i++) {
			float xScaling = 0.01f;
			float yScaling = 175.0f;
			
			Vector3 drawStartPos = new Vector3((i - 1) * xScaling,
			                                   ((float)data[i - 1]) * yScaling,
			                                   0.0f);
			Vector3 drawEndPos = new Vector3(i * xScaling,
			                                 ((float)data[i]) * yScaling,
			                                 0.0f);
			
			Debug.DrawLine(drawStartPos, drawEndPos, Color.red);
		}
	}

	public void AnalyzeData(double[] data) {
		int numParts = 200;
		int partitionSize = (data.Length+1)/numParts;
		float overlapPercent = .5f;
		float threshold = 1 - .75f; //larger float values are more strict

		data = data.ToList().Select(i => (double)Mathf.Abs((float)i)).ToArray();

		DrawData(data);
		//Debug.Log(data.Length);
		//Debug.Log(partitionSize/2);
		//Debug.Log (data.ToList().Skip(1).Take(5).ToArray().Length);
		for(int i = 0; i < data.Length-(int)(partitionSize * overlapPercent); i += (int)(partitionSize * overlapPercent)) {
			//finds the average value of the sub-partition starting at index i and of size partitionSize
			double avg = data.Skip(i).Take(partitionSize).Average();
            int largest = i;
			for(int j = 0; j < partitionSize * overlapPercent; j++)
			{
                if (data[i + j] > data[largest]) {
                    largest = i+j;
                }
			}
            if (data[largest] * threshold > avg) {
                Debug.DrawLine(new Vector3((largest) * .01f, 0, 0), new Vector3((largest) * .01f, -1, 0), Color.green);
                //Debug.Log(data[i+j]);
            }
        }
	}
	
	public static double f2d(float f) {
		return (double)f;
	}
}
