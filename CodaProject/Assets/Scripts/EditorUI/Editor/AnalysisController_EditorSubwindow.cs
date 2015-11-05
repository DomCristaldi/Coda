using UnityEngine;
using UnityEditor;
//using Coda;

[System.Serializable]
public class AnalysisController_EditorSubwindow : BaseEditorSubwindow {
    
    [SerializeField]
    public AudioClip musicToAnalyze = null;
    public Coda.Analyzer analyzer;

    public bool triggerAnalysis = false;

    [SerializeField]
    private bool _advancedSettingsFoldout = false;

    public override void DoWindowContents(int unusedWindowID) {
        //base.DoWindowContents(unusedWindowID);

        //AnalysisController_EditorSubwindow controller = (AnalysisController_EditorSubwindow)

        //analyzer = new Analyzer();

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

        _advancedSettingsFoldout = EditorGUILayout.Foldout(_advancedSettingsFoldout, "Advanced Settings");
        if (_advancedSettingsFoldout) {
            //EditorGUILayout.BeginVertical();
            analyzer.numPartitions = EditorGUILayout.IntField("Number of Partitions", analyzer.numPartitions);
            analyzer.dataAbstractionOverlapPercent = EditorGUILayout.FloatField("Raw Overlap Percent", analyzer.dataAbstractionOverlapPercent);
            analyzer.threshold = EditorGUILayout.FloatField("Threshold", analyzer.threshold);
            analyzer.beatDetectionOverlapPercent = EditorGUILayout.FloatField("Partitioned Overlap Percent", analyzer.beatDetectionOverlapPercent);
            //EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndScrollView();
    }

    public void AssignAnalyzer(Coda.Analyzer analyzer) {
        this.analyzer = analyzer;
    } 

    public virtual void ProcessAudio() {
        triggerAnalysis = true;

        //Debug.LogError("IMPLEMENT ME!!!!");
        //analyzer.ProcessAudio(musicToAnalyze as AudioClip);
    }

}
