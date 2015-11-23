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


    private float _minOverlapPercent = 0.05f;

    public int numPartitions = 10000;
    public float dataAbstractionOverlapPercent = 0.5f;
    [Tooltip("If this percentage of a point is above the average it is considered a Beat\nHigher numbers->false positives\nLower numbers->miss Beats")]
    public float threshold = 1f;


    public AnalysisController_EditorSubwindow() {
        windowName = "Settings";
    }


    public override void DoWindowContents(int unusedWindowID) {
        //base.DoWindowContents(unusedWindowID);

        //AnalysisController_EditorSubwindow controller = (AnalysisController_EditorSubwindow)

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);//allows for window scrolling

        EditorGUILayout.BeginVertical();
        //field to drop in music file

        EditorGUILayout.BeginHorizontal();

        //button to proceed with music processing
        if (GUILayout.Button("Analyze")) {
            if (musicToAnalyze != null) {
                ProcessAudio();//triggers audio processing
            }
            else {
                Debug.LogError("Need a music file to analyze");
            }
        }

        musicToAnalyze = (AudioClip)EditorGUILayout.ObjectField(musicToAnalyze, typeof(AudioClip), true);

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndVertical();


        EditorGUILayout.BeginVertical();

        //ADVANCED CONTROLS DROPDOWN MENU
        _advancedSettingsFoldout = EditorGUILayout.Foldout(_advancedSettingsFoldout, "Advanced Settings");
        if (_advancedSettingsFoldout) {

            numPartitions = EditorGUILayout.IntField("Number of Partitions", numPartitions);
            dataAbstractionOverlapPercent = EditorGUILayout.Slider("Raw Overlap Percent", dataAbstractionOverlapPercent, _minOverlapPercent, 1.0f);
            threshold = EditorGUILayout.Slider("Threshold", threshold, 0.5f, 1.5f);
            
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
