using UnityEngine;
using UnityEditor;

[System.Serializable]
public class AnalysisController_EditorSubwindow : BaseEditorSubwindow {
    
    [SerializeField]
    public Object musicToAnalyze = null;

    public override void DoWindowContents(int unusedWindowID) {
        //base.DoWindowContents(unusedWindowID);

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        /*
        for (int i = 0; i < 50; ++i) {
            GUILayout.Button("Test");
        }
        */

        musicToAnalyze = (Object)EditorGUILayout.ObjectField(musicToAnalyze, typeof(Object), true);

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
        Debug.LogError("IMPLEMENT ME!!!!");

    }

}
