using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class Analyzer_EditorWindow : EditorWindow {

    Analyzer analyzer;

    public List<BaseEditorSubwindow> subwindowList;
    [SerializeField]
    AnalysisController_EditorSubwindow analysisControlWindow;
    [SerializeField]
    WaveformMarkup_EditorSubwindow waveformMarkupWindow;

    private Rect controlsPos = new Rect(0, 0, 200, 200);
    private Rect waveformPos = new Rect(200, 0, 500, 300);

    [MenuItem("Coda/Analyzer")]
    private static void OpenWindow() {
        Analyzer_EditorWindow window = GetWindow<Analyzer_EditorWindow>();
        window.Show();
    }

	void OnEnable() {

        HandleWindowInstantiation();

        analyzer = new Analyzer();
    }

    void OnDisable() {
        analysisControlWindow.SaveSettings();
    }

    void OnGUI() {

        HandleWindowInstantiation();

        
        HandleDrawingSubwindow(analysisControlWindow,
                               waveformMarkupWindow);

        if (analysisControlWindow.triggerAnalysis == true) {
            analysisControlWindow.triggerAnalysis = false;
            waveformMarkupWindow.waveform = analyzer.ProcessAudio(analysisControlWindow.musicToAnalyze);
        }

        waveformMarkupWindow.DrawWindowDebug();
        //if (waveformMarkupWindow.IsInSubwindow())

        Repaint();
    }

    private void HandleWindowInstantiation() {
        if (analysisControlWindow == null) {
            analysisControlWindow = ScriptableObject.CreateInstance<AnalysisController_EditorSubwindow>();
            analysisControlWindow.Setup(controlsPos);
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
