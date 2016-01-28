using UnityEngine;
using UnityEditor;
using System.Collections;

using Coda;

public class AuthorBeatmap_EditorWindow : EditorWindow {

    private string _beatmapName;
    private float _beatmapBPM;
    private int _beatmapNumBeats;

    [MenuItem("Coda/Beatmap Author")]
    static void CreateAuthoringWindowUtility() {
        //AuthorBeatmap_EditorWindow window = new AuthorBeatmap_EditorWindow();
        AuthorBeatmap_EditorWindow window = GetWindow<AuthorBeatmap_EditorWindow>();
        window.ShowUtility();
    }


    void OnGUI() {
        EditorGUILayout.BeginVertical();

        if (GUILayout.Button("Create Beatmap")) {
            AnalyzerTools.BeatMapFromBPM(_beatmapName, _beatmapBPM, _beatmapNumBeats);
        }

        _beatmapName = EditorGUILayout.TextField("Name", _beatmapName);
        _beatmapBPM = EditorGUILayout.FloatField("BPM", _beatmapBPM);
        _beatmapNumBeats = EditorGUILayout.IntField("Number of Beats", _beatmapNumBeats);


        EditorGUILayout.EndVertical();
    }

}
