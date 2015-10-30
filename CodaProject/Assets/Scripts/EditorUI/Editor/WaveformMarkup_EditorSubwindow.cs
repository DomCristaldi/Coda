using UnityEngine;
using UnityEditor;
using System.Collections;

public class WaveformMarkup_EditorSubwindow : BaseEditorSubwindow {

    public Color waveColor = Color.red;

    public double[] waveform = null;

    private Rect waveformRect = new Rect(0, 0, 200, 200);

	public WaveformMarkup_EditorSubwindow() {
        windowName = "Waveform";
    }

    public override void DoWindowContents(int unusedWindowID) {
        //base.DoWindowContents(unusedWindowID);

        Color originalHandleColor = Handles.color;

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        Handles.BeginGUI();
        GUILayout.BeginArea(waveformRect);

        if (waveform != null) {
            DrawWaveform();
        }

        GUILayout.EndArea();
        Handles.EndGUI();

        EditorGUILayout.EndScrollView();

        Handles.color = originalHandleColor;
    }

    private void DrawWaveform() {
        Handles.color = waveColor;

        for (int i = 1; i < waveform.Length - 1; i++) {
            float xScaling = .01f;
            float yScaling = 175.0f;

            Vector3 drawStartPos = new Vector3((i - 1) * xScaling,
                                               ((float)waveform[i - 1]) * yScaling,
                                               0.0f);
            Vector3 drawEndPos = new Vector3(i * xScaling,
                                             ((float)waveform[i]) * yScaling,
                                             0.0f);

            Handles.DrawLine(drawStartPos, drawEndPos);

            //Debug.DrawLine(new Vector3((i - 1) / 1000, (float)averages[i] * 10, 0), new Vector3((i) / 1000, (float)averages[i + 1] * 10, 0), Color.red);

            //Debug.DrawLine(new Vector3((i - 1) / 1000, (float)averages[i] * 10, 0), new Vector3((i) / 1000, (float)averages[i + 1] * 10, 0), Color.red);
            //Debug.DrawLine(new Vector3(Mathf.Log(i - 1), (float)averages[i] * 10, 0), new Vector3(Mathf.Log(i), (float)averages[i + 1] * 10, 0), Color.red);
        }
    }

}
