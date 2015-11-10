using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Linq;

namespace Coda {

	public class WaveformMarkup_EditorSubwindow : BaseEditorSubwindow {

	    public Color waveColor = Color.red;
	    public Color beatColor = Color.green;

	    public double[] waveform = null;
	    public BeatMap beatmap = null;

	    private Rect waveformRect = new Rect(0, 0, 200, 200);
        private Rect markupRect = new Rect(0, 200, 200, 200);

        private int markupWindowHeight = 100;

	    private Rect selectionBrush;
	    private float brushPosition = 0.0f;

		public WaveformMarkup_EditorSubwindow() {
	        windowName = "Waveform";
	    }

        /// <summary>
        /// Override to draw everything specific to this UI element
        /// </summary>
        /// <param name="unusedWindowID"></param>
	    public override void DoWindowContents(int unusedWindowID) {
	        //base.DoWindowContents(unusedWindowID);

	        //Debug.Log(waveformRect.x);

	        waveformRect.Set(waveformRect.x,
	                         waveformRect.y,
	                         subwindowRect.width,
	                         subwindowRect.height - markupWindowHeight);

            markupRect.Set(0.0f, waveformRect.y + waveformRect.height,
                           subwindowRect.width, markupWindowHeight);

	        Color originalHandleColor = Handles.color;

	        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

	        Handles.BeginGUI();
	        GUILayout.BeginArea(waveformRect);

	        if (waveform != null) {
	            DrawWaveform(waveformRect);
	        }

            GUILayout.EndArea();

            GUILayout.BeginArea(markupRect);

	        if(beatmap != null) {
	            DrawBeats(markupRect);
	        }


	        GUILayout.EndArea();

            DrawOutline();


            Handles.EndGUI();

	        EditorGUILayout.EndScrollView();

            //Debug.Log(GUIUtility.GUIToScreenPoint(Event.current.mousePosition));
            //IsInSubwindow(Event.current.mousePosition);
            //DrawWindowDebug();
        
            //Debug.Log(GUIUtility.ScreenToGUIPoint(Event.current.mousePosition));
	        Handles.color = originalHandleColor;

	        //Debug.Log(GUIUtility.GUIToScreenPoint(Event.current.mousePosition));
	        //IsInSubwindow(Event.current.mousePosition);
	        //DrawWindowDebug();
	        
	        //Debug.Log(GUIUtility.ScreenToGUIPoint(Event.current.mousePosition));

	    }

        /// <summary>
        /// Draws the waveform as lines connecting all positions in the audio file
        /// </summary>
	    private void DrawWaveform(Rect drawArea) {


	        Handles.color = waveColor;

	        float xScaling = drawArea.width / waveform.Length;
	        //float yScaling = waveformRect.height / waveform.Length;
	        //float maxVal = (float) waveform.ToList<double>().Max<double>();
	        //Debug.LogFormat("{0} | {1}", maxVal, waveformRect.height);

	        float yScaling = 600.0f;
	        float yOffset = drawArea.height / 2.0f;



	        for (int i = 1; i < waveform.Length - 1; i++) {



	            Vector3 drawStartPos = new Vector3((i - 1) * xScaling,
	                                               (((float)waveform[i - 1]) * yScaling) + yOffset,
	                                               0.0f);
	            Vector3 drawEndPos = new Vector3(i * xScaling,
	                                             (((float)waveform[i]) * yScaling) + yOffset,
	                                             0.0f);

	            Handles.DrawLine(drawStartPos, drawEndPos);

	            //Debug.DrawLine(new Vector3((i - 1) / 1000, (float)averages[i] * 10, 0), new Vector3((i) / 1000, (float)averages[i + 1] * 10, 0), Color.red);

	            //Debug.DrawLine(new Vector3((i - 1) / 1000, (float)averages[i] * 10, 0), new Vector3((i) / 1000, (float)averages[i + 1] * 10, 0), Color.red);
	            //Debug.DrawLine(new Vector3(Mathf.Log(i - 1), (float)averages[i] * 10, 0), new Vector3(Mathf.Log(i), (float)averages[i + 1] * 10, 0), Color.red);
	        }

            Handles.color = Color.black;

            Handles.DrawLine(new Vector2(drawArea.x, drawArea.y + (drawArea.height / 2.0f)),
                             new Vector2(drawArea.x + drawArea.width, drawArea.y + (drawArea.height / 2.0f)));

	    }


        /// <summary>
        /// Draws vertical lines for the locations of beats
        /// </summary>
	    private void DrawBeats(Rect drawArea) {

	        Handles.color = beatColor;

	        float xScaling = markupRect.width;// / waveform.Length;
	        float yScaling = 600.0f;
            //float yOffset = markupRect.height / 2.0f;
            float yOffset = 0.0f;
	        float totalLength = beatmap.songLength;

	        for (int i = 1; i < beatmap.beats.Count; i++) {
	            

	            Vector3 drawStartPos = new Vector3((float)beatmap.beats[i].timeStamp/totalLength * xScaling,
	                                               yOffset,
	                                               0.0f);
	            Vector3 drawEndPos = new Vector3((float)beatmap.beats[i].timeStamp / totalLength * xScaling,
	                                             yOffset + yScaling,
	                                             0.0f);

	            Handles.DrawLine(drawStartPos, drawEndPos);

	            //Debug.DrawLine(new Vector3((i - 1) / 1000, (float)averages[i] * 10, 0), new Vector3((i) / 1000, (float)averages[i + 1] * 10, 0), Color.red);

	            //Debug.DrawLine(new Vector3((i - 1) / 1000, (float)averages[i] * 10, 0), new Vector3((i) / 1000, (float)averages[i + 1] * 10, 0), Color.red);
	            //Debug.DrawLine(new Vector3(Mathf.Log(i - 1), (float)averages[i] * 10, 0), new Vector3(Mathf.Log(i), (float)averages[i + 1] * 10, 0), Color.red);
	        }
	    }

        private void DrawOutline() {
            //lines to draw for perspective
            Handles.color = Color.black;

            //top horizontal line
            Handles.DrawLine(waveformRect.position,
                             new Vector2(waveformRect.position.x + waveformRect.width, waveformRect.position.y));

            //middle horizontal line
            Handles.DrawLine(new Vector2(waveformRect.position.x, waveformRect.position.y + (waveformRect.height / 2.0f)),
                             new Vector2(waveformRect.position.x + waveformRect.width, waveformRect.position.y + (waveformRect.height / 2.0f)));

            //bottom horizontal line
            Handles.DrawLine(new Vector2(waveformRect.position.x, waveformRect.position.y + waveformRect.height - 0.5f),
                             new Vector2(waveformRect.position.x + waveformRect.width, waveformRect.position.y + waveformRect.height - 0.5f));

        }
    }

}
