using UnityEngine;
using UnityEditor;

[System.Serializable]
public class AnalysisController_EditorSubwindow : BaseEditorSubwindow {
    
    [SerializeField]
    public AudioClip musicToAnalyze = null;
    public Analyzer analyzer;

    public override void DoWindowContents(int unusedWindowID) {
        //base.DoWindowContents(unusedWindowID);

        analyzer = new Analyzer();

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        /*
        for (int i = 0; i < 50; ++i) {
            GUILayout.Button("Test");
        }
        */

        musicToAnalyze = (AudioClip)EditorGUILayout.ObjectField(musicToAnalyze, typeof(AudioClip), true);

        if (GUILayout.Button("Analyze")) {
            if (musicToAnalyze != null) {
                ProcessAudio();
            }
            else {
                Debug.LogError("Need a music file to analyze");
            }
        }


        EditorGUILayout.EndScrollView();
    }

    public virtual void ProcessAudio() {
        //Debug.LogError("IMPLEMENT ME!!!!");
        analyzer.ProcessAudio(musicToAnalyze as AudioClip);
    }

}
