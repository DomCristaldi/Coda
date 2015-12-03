using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Coda {

	/// <summary>
	/// Analyzes songs to obtain beatmaps.
	/// </summary>
	[System.Serializable]
	public class Analyzer {
		
		public AudioClip clip;
        //Float constants for use in the Blackman-Harris windowing function
		private const float _a0 = 0.35875f;
		private const float _a1 = 0.48829f;
		private const float _a2 = 0.14128f;
		private const float _a3 = 0.01168f;
		private BeatMap _beatList;
		private double[] _averages;
		
		//public int numPartitions = 10000;
		//public float overlapPercent = 0.5f; 
        //public float threshold = 1 - 0.75f; //larger float values are more strict
        //public float beatDetectionOverlapPercent = 0.5f;


		/// <summary>
		/// Processes raw audio data to find average energy for overlapping partitions.
		/// </summary>
		/// <returns>The FFT data array.</returns>
		/// <param name="clip">Audio clip to process.</param>
        /// <param name="numPartitions">Number of pieces to split the song into for analysis</param>
        /// <param name="overlapPercent">The percentage which the partitions overlap each other</param>
        public double[] ProcessAudio(AudioClip clip, int numPartitions, float overlapPercent) {

            _averages = new double[(int)(numPartitions / overlapPercent) - 1];
	        int samplesPerPartition = (int)(clip.samples / numPartitions);


            int numDivisions = (int)(numPartitions / overlapPercent) - 1; 
            //Because the partitions overlap, the number of iterations is the number of partitions multiplied by the inverse of the overlap percent
			for (int i = 0; i < numDivisions; i++) {

	            float[] samples = new float[samplesPerPartition];
	            int input = i * ((int) (samples.Length * overlapPercent)); //the offset to start getting song data increases by overlapPercent as i is incremented
	            clip.GetData(samples, input); 
	            
                //the raw partition data is run through the Blackman-Harris windowing function            
				for (int n = 0; n < samples.Length; n++) {
					samples [n] *= _a0 - _a1 * Mathf.Cos ((2 * Mathf.PI * n) / samples.Length - 1) + _a2 * Mathf.Cos ((4 * Mathf.PI * n) / samples.Length - 1) - _a3 * Mathf.Cos ((6 * Mathf.PI * n) / samples.Length - 1);
				}


                FFT2 FFT = new FFT2 ();
				FFT.init ((uint)Mathf.Log(samplesPerPartition,2));
                //our array of floats is converted to an array of doubles for use in the FFT function
                double[] double_samples = samples.ToList ().ConvertAll<double> (new System.Converter<float, double> (F2D)).ToArray ();
                //runs our sample data through a Fast Fourier Transform to convert it to the frequency domain
				FFT.run (double_samples, new double[samples.Length], false);
				
                //gets the average value for this partition and adds it to an array.
                //when all of the partitions are completed, averages will contain data for the entire song
				double avg = double_samples.Average ();
				_averages[i] = avg;

			}
			
	        return _averages;
		}

		/// <summary>
		/// Draws the data as a waveform.
		/// </summary>
		/// <param name="data">Data to draw.</param>
		public void DrawData(double[] data) {
			for(int i = 1; i < _averages.Length-1; i++) {
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

		/// <summary>
		/// Analyzes the song data for beat information
		/// </summary>
		/// <returns>A beatmap of the song.</returns>
		/// <param name="data">Raw data.</param>
		/// <param name="clip">Audio clip to analyze.</param>
        /// <param name="thresholdModifier">Threshold value modifier</param>
        public BeatMap AnalyzeData(double[] data, AudioClip clip, float thresholdModifier) {
	        _beatList = new BeatMap(clip.name, clip.length);
			int numParts = (int)clip.length;
			int partitionSize = (data.Length+1)/numParts;

            //We only care about the magnitude of our data, so we get the absolute values before doing analysis
			data = data.ToList().Select(i => (double)Mathf.Abs((float)i)).ToArray();

	        for(int i = 0; i < data.Length-(int)(partitionSize); i += (int)(partitionSize)) {
	            //finds the average value of the sub-partition starting at index i and of size partitionSize
	            double avg = data.Skip(i).Take(partitionSize).Average();
                //finds the highest energy sample in the current partition
	            //int largest = i;
                //calculate the average energy variance in the partition
                double variance = 0;
				for(int j = 0; j < partitionSize; j++)
				{
                    variance += (data[i + j] - avg) * (data[i + j] - avg);
				}
                
                variance /= partitionSize;
                //calculate the base threshold using some magic numbers and the variance
                double thresh = (-0.0025714 * variance) + 1.5142857;
                thresh *= thresholdModifier;

                //if the any sample is threshold percent larger than the average, then we mark it as a beat.
                for (int j = 0; j < partitionSize; j++) {
                    if (data[i+j] > thresh * avg) {
                        _beatList.AddBeat(((float)(i + j) / data.Length) * clip.length, 1f, data[i + j]);
                    }
                }
	        }

            //eliminate double positives (the same beat occurring in two overlapping partitions, for example) by removing beats with extremely similar timestamps
	        for(int i = 0; i < _beatList.beats.Count-1; i++) {
	            if((_beatList.beats[i+1].timeStamp - _beatList.beats[i].timeStamp) < .1) {
	                if(_beatList.beats[i + 1].energy > _beatList.beats[i].energy) {
	                    _beatList.beats.RemoveAt(i);
	                }
	                else {
	                    _beatList.beats.RemoveAt(i + 1);
	                }
	                i--;
	            }

	        }
	        return _beatList;
		}

		/// <summary>
		/// Utility function for converting float to double. Used in conjunction with System.Linq to convert an array of floats to doubles
		/// </summary>
		/// <param name="f">Float to convert.</param>
		private static double F2D(float f) {
			return (double)f;
		}
	}

}
