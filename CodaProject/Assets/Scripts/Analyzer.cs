using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Coda {

	[System.Serializable]
	public class Analyzer {
		
		public AudioClip clip;
		private const float a0 = 0.35875f;
		private const float a1 = 0.48829f;
		private const float a2 = 0.14128f;
		private const float a3 = 0.01168f;
		private BeatMap _beatList;
		private double[] averages;
		
		int numPartitions = 10000;
		float overlapPercent = 0.5f;
		float inverseOverlap {
			get {
				return 1.0f / overlapPercent;
			}
		}


		public Analyzer() {
			//_beatList = new BeatMap(name);
		}
		
		
		public double[] ProcessAudio (AudioClip clip) {	
			averages = new double[(int)(numPartitions * inverseOverlap) - 1];
	        int samplesPerPartition = (int)(clip.samples / numPartitions);


			int numDivisions = (int)(numPartitions * inverseOverlap) -1;
			for (int i = 0; i < numDivisions; i++) {

	            float[] samples = new float[samplesPerPartition];
	            

	                int input = i * ((int) (samples.Length * overlapPercent));

	                clip.GetData(samples, input);
	                        
				for (int n = 0; n < samples.Length; n++) {
					samples [n] *= a0 - a1 * Mathf.Cos ((2 * Mathf.PI * n) / samples.Length - 1) + a2 * Mathf.Cos ((4 * Mathf.PI * n) / samples.Length - 1) - a3 * Mathf.Cos ((6 * Mathf.PI * n) / samples.Length - 1);
				}
				
				FFT2 FFT = new FFT2 ();
				FFT.init ((uint)Mathf.Log(samplesPerPartition,2));
				double[] double_samples = samples.ToList ().ConvertAll<double> (new System.Converter<float, double> (f2d)).ToArray ();
				FFT.run (double_samples, new double[samples.Length], false);
				

				double avg = double_samples.Average ();
				averages[i] = avg;

			}
			
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

		public BeatMap AnalyzeData(double[] data, AudioClip clip) {
	        _beatList = new BeatMap(clip.name, clip.length);
			int numParts = (int)clip.length;
			int partitionSize = (data.Length+1)/numParts;
			float overlapPercent = .5f;
			float threshold = 1 - .75f; //larger float values are more strict

			data = data.ToList().Select(i => (double)Mathf.Abs((float)i)).ToArray();

			DrawData(data);
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
	                _beatList.AddBeat(((float)largest / data.Length) * clip.length, 1f,data[largest]);
	                //Debug.Log("Beat found at " + ((float)largest/data.Length) * clip.length);
	            }
	        }

	        for(int i = 0; i < _beatList.beats.Count-1; i++) {
	            if((_beatList.beats[i+1].timeStamp - _beatList.beats[i].timeStamp) < .05) {
	                if(_beatList.beats[i + 1].energy > _beatList.beats[i].energy) {
	                    _beatList.beats.RemoveAt(i);
	                }
	                else {
	                    _beatList.beats.RemoveAt(i + 1);
	                }
	                i--;
	            }
	            //Debug.DrawLine(new Vector3((largest) * .01f, 0, 0), new Vector3((largest) * .01f, -1, 0), Color.green);

	        }
	        //Debug.Log(_beatList.beats[0].timeStamp);
	        return _beatList;
		}
		
		public static double f2d(float f) {
			return (double)f;
		}
	}

}
