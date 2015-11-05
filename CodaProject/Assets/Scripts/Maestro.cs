using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Coda {

	[RequireComponent(typeof(AudioSource))]
	public class Maestro : MonoBehaviour {

	    public static Maestro current = null;

		public TextAsset beatmapFile;
		public BeatMap beatmap;
	    private AudioSource _audio;

		public delegate void OnBeatDelegate();
		public OnBeatDelegate onBeat;

		List<MusicBehaviour> listeners;

		private bool audioClipExists;

	    void Awake() {
	        if (current == null) {
	            current = this;
	        }
			audioClipExists = true;
	        _audio = GetComponent<AudioSource>();
			if (_audio.clip == null) {
				audioClipExists = false;
				Debug.LogError("Maestro: No Audio Clip!");
			}
			else {
				if ("BeatMap_" + _audio.clip.name.Replace(".mp3", "") != beatmapFile.name) {
					Debug.LogWarning("Maestro: Audio Clip and Beatmap File mismatch!");
				}
			}
			onBeat = OnBeat;
			listeners = new List<MusicBehaviour>();
			beatmap = BeatMapReader.ReadBeatMap(beatmapFile);
	    }

		void Start () {
			if (audioClipExists) {
	        	_audio.Play();
			}
		}
		
		void Update () {
			if (audioClipExists) {

			}
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
