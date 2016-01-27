using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Coda {

	/// <summary>
	/// Analyzer editor window. Controls for setting up the Analyzer for songs and outputing beatmaps to file.
	/// </summary>
	public class Analyzer_EditorWindow : EditorWindow {

        private Analyzer _analyzer;

	    //public List<BaseEditorSubwindow> subwindowList;

	    [SerializeField]
	    private AnalysisController_EditorSubwindow _analysisControlWindow;
	    [SerializeField]
	    private WaveformMarkup_EditorSubwindow _waveformMarkupWindow;

        private int _controlsWidth = 400;

        private Rect _controlsPos = new Rect(0, 0, 400, 200);//Analysis Controller Subwindow location & dimensions
        private Rect _waveformPos = new Rect(400, 0, 500, 300);//Waveform Markup Subwindow location & dimensions

        private AudioClip _prevAudioClip;

        private string _filePath;//string that acts as a holder for any file path we may need



		/// <summary>
		/// Opens the analyzer window from the Coda dropdown menu.
		/// </summary>
	    [MenuItem("Coda/Analyzer")]
	    private static void OpenWindow() {
	        Analyzer_EditorWindow window = GetWindow<Analyzer_EditorWindow>();
            window.name = "Audio Analyzer";
	        window.Show();
	    }

		void OnEnable() {
            _analyzer = new Analyzer();

	        HandleWindowInstantiation();//make sure we have windows for our GUI

            _controlsPos = new Rect(0, 0, _controlsWidth, 200);
            _waveformPos = new Rect(_controlsWidth, 0, 
                                    position.width - _controlsWidth >= 0 ? position.width - _controlsWidth : 0, 300);
	    }

	    void OnDisable() {
	        _analysisControlWindow.SaveSettings();//TODO: implement proper serialization of Editor Subwindows
	    }

        //UPDATE LOOP FOR UNITY EDITOR
	    void OnGUI() {

            int waveformWidth = (int) (position.width - _controlsWidth);

            _controlsPos = new Rect(0, 0, _controlsWidth, 200);
            _waveformPos = new Rect(_controlsWidth, 0,
                                    waveformWidth, 300);

	        HandleWindowInstantiation();//make sure the UI exists

            _waveformMarkupWindow.Setup(_waveformPos);


            //read in xml beatmap file if it exists for the supplied audio file
			if (_analysisControlWindow.musicToAnalyze != _prevAudioClip) {//only load if new

                //ANALYZER SONG IF ONE IS ASSIGNED IN THE EDITOR GUI
				if (_analysisControlWindow.musicToAnalyze != null) {

					_filePath = WaveformSerializer.filePath + "/Waveform_" + _analysisControlWindow.musicToAnalyze.name + ".xml";//location of possibly saved Wavefrom

					if (System.IO.File.Exists(_filePath)) {//retrieve the Waveform if we computed one in the past
						Waveform newWave = WaveformSerializer.ReadWaveformData(_filePath);
						_waveformMarkupWindow.waveform = newWave.data;
					}
					else {
						_waveformMarkupWindow.waveform = null;
					}

					_filePath = BeatMapSerializer.filePath + "/BeatMap_" + _analysisControlWindow.musicToAnalyze.name + ".xml";// location of possibly saved Beatmap
					
                    if (System.IO.File.Exists(_filePath)) {//load Beatmap for current song if it could be found
						BeatMap newMap = BeatMapSerializer.BeatMapReader.ReadBeatMap(_filePath);
						_waveformMarkupWindow.beatmap = newMap;
					}
					else {
						_waveformMarkupWindow.beatmap = null;
					}
				}

                //NO SONG WAS ASSIGNED
				else {
					_waveformMarkupWindow.waveform = null;
					_waveformMarkupWindow.beatmap = null;
				}

				_prevAudioClip = _analysisControlWindow.musicToAnalyze;//audio clip from previous frame
			}
	        

            //DRAW SUBWINDOWS
	        HandleDrawingSubwindow(_analysisControlWindow,
	                               _waveformMarkupWindow);

            /*
            //waveformMarkupWindow.DrawWindowDebug();//shows borders of Subwinows

            if (waveformMarkupWindow.IsInSubwindow(Event.current.mousePosition)) {//check if we can click inside a subwindow
                //Debug.LogFormat("waveform has it");
            }
            */

            //USER HAS TRIGGERED ANALYSIS VIA ANALYZER CONTROLLER SUBWINDOW
	        if (_analysisControlWindow.triggerAnalysis == true) {
	            _analysisControlWindow.triggerAnalysis = false;

                //ANALYSIS
                //feed Analyzer the user-defined audio file to get audio as frequency data
	            double[] waveformData = _analyzer.ProcessAudio(_analysisControlWindow.musicToAnalyze, 
                                                               _analysisControlWindow.numPartitions,
                                                               _analysisControlWindow.dataAbstractionOverlapPercent);
                //feed in frequency data to get a Beatmap
	            BeatMap beats = _analyzer.AnalyzeData(waveformData,
                                                      _analysisControlWindow.musicToAnalyze, 
                                                      _analysisControlWindow.threshold);

                //SERIALIZATOIN
				WaveformToFile(waveformData, _analysisControlWindow.musicToAnalyze.name);
	            BeatMapToFile(beats, _analysisControlWindow.musicToAnalyze.name);

                //DRAWING
	            _waveformMarkupWindow.waveform = waveformData;
	            _waveformMarkupWindow.beatmap = beats;
	        }

	        Repaint();//force GUI to draw every update even if not clicked on
	    }

		/// <summary>
		/// Writes waveform to xml file.
		/// </summary>
		/// <param name="data">Raw data from FFT.</param>
		/// <param name="name">Xml file name.</param>
		private void WaveformToFile(double[] data, string name) {
			Waveform newWave = new Waveform(data, name);
			WaveformSerializer.WriteWaveformData(newWave);
		}

		/// <summary>
		/// Writes beatmap to xml file.
		/// </summary>
		/// <param name="beats">Raw data from FFT.</param>
		/// <param name="name">Xml file name.</param>
	    private void BeatMapToFile(BeatMap beats, string name) {
	        //BeatMap map = new BeatMap(name, beats.songLength);

	        //this is a test using a dummy object
	        //map.AddBeat(1, 3.0f, 5.0f);
			BeatMapSerializer.BeatMapWriter.WriteBeatMap(beats);
	    }

		/// <summary>
		/// Writes a beatmap to xml file from BPM and song length.
		/// </summary>
		/// <param name="name">Xml file name.</param>
		/// <param name="bpm">Beats per minute.</param>
		/// <param name="numBeats">Number of beats.</param>
		private void BeatMapFromBPM(string name, float bpm, int numBeats) {
			float beatStep = 60f / bpm;
			float length = (float)numBeats * beatStep;
			BeatMap beats = new BeatMap(name, length);
			for (float i = 0f; i < length; i += beatStep) {
				beats.AddBeat(i, 1f, 1);
			}
			BeatMapSerializer.BeatMapWriter.WriteBeatMap(beats);
			Debug.LogFormat("Coda: Created beatmap {0} with BPM of {1} and running time of {2} seconds ({3} beats long).", name, bpm, length, numBeats);
		}

		/// <summary>
		/// Handles the analyzer window instantiation.
		/// </summary>
	    private void HandleWindowInstantiation() {

	        if (_analysisControlWindow == null) {
	            _analysisControlWindow = ScriptableObject.CreateInstance<AnalysisController_EditorSubwindow>();
	            _analysisControlWindow.Setup(_controlsPos);
                _analysisControlWindow.AssignAnalyzer(_analyzer);
	        }

	        if (_waveformMarkupWindow == null) {
	            _waveformMarkupWindow = ScriptableObject.CreateInstance<WaveformMarkup_EditorSubwindow>();
	            _waveformMarkupWindow.Setup(_waveformPos);
	        }
	    }

		/// <summary>
		/// Handles drawing the specifies subwindows.
		/// </summary>
		/// <param name="subWindows">List of all sub windows to draw.</param>
	    private void HandleDrawingSubwindow(params BaseEditorSubwindow[] subWindows) {
	        BeginWindows();

	        for (int i = 0; i < subWindows.Length; ++i) {

	            subWindows[i].DrawSubwindow(i + 1, subWindows[i].windowName);

	        }

	        EndWindows();
	    }
	}

}
