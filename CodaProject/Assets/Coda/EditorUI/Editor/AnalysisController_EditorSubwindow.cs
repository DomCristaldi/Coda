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

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);//allows for window scrolling

        EditorGUILayout.BeginVertical();
        //field to drop in music file
        musicToAnalyze = (AudioClip)EditorGUILayout.ObjectField(musicToAnalyze, typeof(AudioClip), true);

        //button to proceed with music processing
        if (GUILayout.Button("Analyze")) {
            if (musicToAnalyze != null) {
                ProcessAudio();//triggers audio processing
            }
            else {
                Debug.LogError("Need a music file to analyze");
            }
        }

        EditorGUILayout.EndVertical();


        EditorGUILayout.BeginVertical();

        //ADVANCED CONTROLS DROPDOWN MENU
        _advancedSettingsFoldout = EditorGUILayout.Foldout(_advancedSettingsFoldout, "Advanced Settings");
        if (_advancedSettingsFoldout) {
            analyzer.numPartitions = EditorGUILayout.IntField("Number of Partitions", analyzer.numPartitions);
            analyzer.dataAbstractionOverlapPercent = EditorGUILayout.FloatField("Raw Overlap Percent", analyzer.dataAbstractionOverlapPercent);
            analyzer.threshold = EditorGUILayout.FloatField("Threshold", analyzer.threshold);
            analyzer.beatDetectionOverlapPercent = EditorGUILayout.FloatField("Partitioned Overlap Percent", analyzer.beatDetectionOverlapPercent);
        }
        EditorGUILayout.EndVertical();


        EditorGUILayout.EndScrollView();
    }

    /// <summary>
    /// Assign a reference to the Analyzer that this Subwindow sets values for
    /// </summary>
    /// <param name="analyzer">Analyzer instance from outside this class</param>
    public void AssignAnalyzer(Coda.Analyzer analyzer) {
        this.analyzer = analyzer;
    } 

    public virtual void ProcessAudio() {
        triggerAnalysis = true;//signal to say we are ready to begin processing audio
                                //(don't do it here because this is just controls)

        //Debug.LogError("IMPLEMENT ME!!!!");
        //analyzer.ProcessAudio(musicToAnalyze as AudioClip);
    }

}
