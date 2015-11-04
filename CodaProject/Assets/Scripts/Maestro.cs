using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Coda {

	[RequireComponent(typeof(AudioSource))]
	public class Maestro : MonoBehaviour {

	    public static Maestro current = null;

	    private AudioSource _audio;

		public delegate void OnBeatDelegate();
		public OnBeatDelegate onBeat;

		List<MusicBehaviour> listeners;

	    void Awake() {
	        if (current == null) {
	            current = this;
	        }
	        _audio = GetComponent<AudioSource>();
			onBeat = OnBeat;
			listeners = new List<MusicBehaviour>();
	    }

		void Start () {
	        _audio.Play();
		}
		
		void Update () {

		}

		void OnBeat () {

		}

		public void Subscribe (MusicBehaviour listener) {
			if (!listeners.Contains(listener)) {
				listeners.Add(listener);
				onBeat += listener.OnBeat;
			}
		}

		public void Unsubscribe (MusicBehaviour listener) {
			if (listeners.Contains(listener)) {
				listeners.Remove(listener);
				onBeat -= listener.OnBeat;
			}
		}
	}

}
