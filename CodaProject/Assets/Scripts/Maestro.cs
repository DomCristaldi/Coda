using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Coda {

	[RequireComponent(typeof(AudioSource))]
	public class Maestro : MonoBehaviour {

	    public static Maestro current = null;

		public TextAsset beatmapFile;
		private BeatMap beatmap;
	    private AudioSource _audio;

		public bool loopAudio;

		public delegate void OnBeatDelegate();
		public OnBeatDelegate onBeat;

		List<MusicBehaviour> listeners;

		private bool audioClipExists;

		private double _beatTimer;
		private Beat _nextBeat;
		private int _beatIndex;
		private bool _songEnded;

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
					Debug.LogWarning("Maestro: Audio Clip and Beatmap File name mismatch!");
				}
			}
			onBeat = OnBeat;
			listeners = new List<MusicBehaviour>();
			beatmap = BeatMapReader.ReadBeatMap(beatmapFile);
	    }

		void Start () {
			if (audioClipExists) {
	        	_audio.Play();
				if (!StartTracking()) {
					audioClipExists = false;
					Debug.LogError("Maestro: Beatmap has zero beats!");
				}
			}
		}
		
		void Update () {
			if (audioClipExists) {
				if (TrackBeats()) {
					onBeat();
				}
			}
		}

		bool StartTracking () {
			if (beatmap == null || beatmap.beats.Count == 0) {
				return false;
			}
			_songEnded = false;
			_nextBeat = beatmap.beats[0];
			_beatTimer = 0.0;
			_beatIndex = 0;
			return true;
		}

		bool TrackBeats () {
			if (!(_songEnded && !loopAudio)) {
				_beatTimer += Time.deltaTime;
			}
			else {
				return false;
			}
			if (_songEnded) {
				if (_beatTimer >= (double)beatmap.songLength) {
					_audio.Stop();
					StartTracking();
					_audio.Play();
				}
			}
			else if (_nextBeat.timeStamp <= _beatTimer) {
				_beatIndex++;
				if (_beatIndex == beatmap.beats.Count) {
					_songEnded = true;
				}
				else {
					_nextBeat = beatmap.beats[_beatIndex];
				}
				return true;
			}
			return false;
		}

		void OnBeat () {
			Debug.Log("Beat.");
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
