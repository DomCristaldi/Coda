using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class Analyzer_EditorWindow : EditorWindow {

    public List<BaseEditorSubwindow> subwindowList;
    [SerializeField]
    AnalysisController_EditorSubwindow testWindow;

    private Rect controlsPos = new Rect(0, 0, 200, 200);

    [MenuItem("Coda/Analyzer")]
    private static void OpenWindow() {
        Analyzer_EditorWindow window = GetWindow<Analyzer_EditorWindow>();
        window.Show();
    }

	void OnEnable() {

        
        if (testWindow == null) {
            Debug.Log("created new");

            testWindow = ScriptableObject.CreateInstance<AnalysisController_EditorSubwindow>();
            testWindow.Setup(controlsPos);
        }


    }

    void OnDisable() {
        testWindow.SaveSettings();
    }

    void OnGUI() {

        if (testWindow == null) {
            testWindow = ScriptableObject.CreateInstance<AnalysisController_EditorSubwindow>();
            testWindow.Setup(controlsPos);
        }

        HandleDrawingSubwindow(testWindow);


        /*
        if (GUILayout.Button("CreateInstance")) {
            testWindow = ScriptableObject.CreateInstance<BaseEditorSubwindow>();
            testWindow.Setup(new Rect(100, 100, 200, 200));
        }

        if (testWindow == null) {
            testWindow = CreateInstance<BaseEditorSubwindow>();
            testWindow.Setup(new Rect(100, 100, 200, 200));
        }
        else {
            HandleDrawingSubwindow(testWindow);
        }
        */

        /*
        if (testWindow != null) {
            Debug.Log("good to go");
            HandleDrawingSubwindow(testWindow);
        }
        else {
            Debug.Log("Must create");
        }
        */
    }

    void HandleDrawingSubwindow(BaseEditorSubwindow subWin) {
        BeginWindows();

        subWin.DrawSubwindow(1, "Controls");

        EndWindows();
    }
}
