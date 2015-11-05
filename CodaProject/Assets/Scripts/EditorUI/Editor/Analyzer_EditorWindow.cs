using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Coda {

	public class Analyzer_EditorWindow : EditorWindow {

		private AudioClip _prevAudioClip;
	    Analyzer analyzer;

	    public List<BaseEditorSubwindow> subwindowList;
	    [SerializeField]
	    AnalysisController_EditorSubwindow analysisControlWindow;
	    [SerializeField]
	    WaveformMarkup_EditorSubwindow waveformMarkupWindow;

	    private Rect controlsPos = new Rect(0, 0, 250, 200);
	    private Rect waveformPos = new Rect(250, 0, 500, 300);

	    [MenuItem("Coda/Analyzer")]
	    private static void OpenWindow() {
	        Analyzer_EditorWindow window = GetWindow<Analyzer_EditorWindow>();
	        window.Show();
	    }

		void OnEnable() {
            analyzer = new Analyzer();


	        HandleWindowInstantiation();

	    }

	    void OnDisable() {
	        analysisControlWindow.SaveSettings();
	    }

	    void OnGUI() {

	        HandleWindowInstantiation();

			if (analysisControlWindow.musicToAnalyze != _prevAudioClip) {
				if (analysisControlWindow.musicToAnalyze != null) {
					string filePath = WaveformSerializer.filePath + "/Waveform_" + analysisControlWindow.musicToAnalyze.name + ".xml";
					if (System.IO.File.Exists(filePath)) {
						Waveform newWave = WaveformSerializer.ReadWaveformData(filePath);
						waveformMarkupWindow.waveform = newWave.data;
					}
					else {
						waveformMarkupWindow.waveform = null;
					}
					filePath = BeatMapper.filePath + "/BeatMap_" + analysisControlWindow.musicToAnalyze.name + ".xml";
					if (System.IO.File.Exists(filePath)) {
						BeatMap newMap = BeatMapReader.ReadBeatMap(filePath);
						waveformMarkupWindow.beatmap = newMap;
					}
					else {
						waveformMarkupWindow.beatmap = null;
					}
				}
				else {
					waveformMarkupWindow.waveform = null;
					waveformMarkupWindow.beatmap = null;
				}
				_prevAudioClip = analysisControlWindow.musicToAnalyze;
			}
	        
	        HandleDrawingSubwindow(analysisControlWindow,
	                               waveformMarkupWindow);


        //waveformMarkupWindow.DrawWindowDebug();
        //if (waveformMarkupWindow.IsInSubwindow())
        if (waveformMarkupWindow.IsInSubwindow(Event.current.mousePosition)) {
            Debug.LogFormat("waveform has it");
        }
	        if (analysisControlWindow.triggerAnalysis == true) {
	            analysisControlWindow.triggerAnalysis = false;
	            double[] data = analyzer.ProcessAudio(analysisControlWindow.musicToAnalyze);
	            BeatMap beats = analyzer.AnalyzeData(data, analysisControlWindow.musicToAnalyze);
				WaveformToFile(data, analysisControlWindow.musicToAnalyze.name);
	            BeatMapToFile(beats, analysisControlWindow.musicToAnalyze.name);
	            waveformMarkupWindow.waveform = data;
	            waveformMarkupWindow.beatmap = beats;
	        }

	        //waveformMarkupWindow.DrawWindowDebug(waveformMarkupWindow.subwindowRect);
	        //if (waveformMarkupWindow.IsInSubwindow())

	        Repaint();
	    }

		private void WaveformToFile(double[] data, string name) {
			Waveform newWave = new Waveform(data, name);
			WaveformSerializer.WriteWaveformData(newWave);
		}

	    private void BeatMapToFile(BeatMap beats, string name) {
	        //BeatMap map = new BeatMap(name, beats.songLength);

	        //this is a test using a dummy object
	        //map.AddBeat(1, 3.0f, 5.0f);
	        BeatMapWriter.WriteBeatMap(beats);
	    }

	    private void HandleWindowInstantiation() {

	        if (analysisControlWindow == null) {
	            analysisControlWindow = ScriptableObject.CreateInstance<AnalysisController_EditorSubwindow>();
	            analysisControlWindow.Setup(controlsPos);
                analysisControlWindow.AssignAnalyzer(analyzer);
	        }

	        if (waveformMarkupWindow == null) {
	            waveformMarkupWindow = ScriptableObject.CreateInstance<WaveformMarkup_EditorSubwindow>();
	            waveformMarkupWindow.Setup(waveformPos);
	        }
	    }

	    private void HandleDrawingSubwindow(params BaseEditorSubwindow[] subWindows) {
	        BeginWindows();

	        for (int i = 0; i < subWindows.Length; ++i) {

	            subWindows[i].DrawSubwindow(i + 1, subWindows[i].windowName);

	        }

	        EndWindows();
	    }
	}

}
