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
		
		void Update () {

		}
	}

}
