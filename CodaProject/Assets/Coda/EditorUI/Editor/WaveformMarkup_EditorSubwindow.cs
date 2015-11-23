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

	    private Rect _waveformRect = new Rect(0, 0, 200, 200);//area for the Waveform
        private Rect _beatmapRect = new Rect(0, 200, 200, 200);//area for the Beatmap

        private int beatmapWindowHeight = 100;

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

            //DETERMINE POSITIONS AND DIMENSIONS OF WAVEFORM AND BEATMAP WINDOWS
	        _waveformRect.Set(_waveformRect.x,
	                         _waveformRect.y,
	                         subwindowRect.width,
	                         subwindowRect.height - beatmapWindowHeight);

            _beatmapRect.Set(0.0f, _waveformRect.y + _waveformRect.height,
                           subwindowRect.width, beatmapWindowHeight);


            
	        Color originalHandleColor = Handles.color;



	        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

	        Handles.BeginGUI();
	        
            //DRAW WAVEFORM
            GUILayout.BeginArea(_waveformRect);

	        if (waveform != null) {
	            DrawWaveform(_waveformRect);
	        }

            GUILayout.EndArea();


            //DRAW BEATMAP
            GUILayout.BeginArea(_beatmapRect);

	        if(beatmap != null) {
	            DrawBeats(_beatmapRect);
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

            //MATH FOR SCALING WAVEFORM FOR DRAWING IN WINDOW VIEW
	        float xScaling = drawArea.width / waveform.Length;
	        //float yScaling = waveformRect.height / waveform.Length;
	        //float maxVal = (float) waveform.ToList<double>().Max<double>();
	        //Debug.LogFormat("{0} | {1}", maxVal, waveformRect.height);

	        float yScaling = 600.0f;
	        float yOffset = drawArea.height / 2.0f;


            //DRAW THE WAVEFORM
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

            //MATH FOR SCALING THE BEATMPA ACROSS WINDOW VIEW
	        float xScaling = _beatmapRect.width;// / waveform.Length;
	        float yScaling = 600.0f;
            //float yOffset = _beatmapRect.height / 2.0f;
            float yOffset = 0.0f;
	        float totalLength = beatmap.songLength;


            //DRAW BEATMAP
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

        //DRAW SOME BLACK LINES FOR POINT OF REFERENCE
        private void DrawOutline() {
            
            Handles.color = Color.black;

            //DIVIDE UP WAVEFORM

            //top horizontal line
            Handles.DrawLine(_waveformRect.position,
                             new Vector2(_waveformRect.position.x + _waveformRect.width, _waveformRect.position.y));

            //middle horizontal line
            Handles.DrawLine(new Vector2(_waveformRect.position.x, _waveformRect.position.y + (_waveformRect.height / 2.0f)),
                             new Vector2(_waveformRect.position.x + _waveformRect.width, _waveformRect.position.y + (_waveformRect.height / 2.0f)));

            //bottom horizontal line
            Handles.DrawLine(new Vector2(_waveformRect.position.x, _waveformRect.position.y + _waveformRect.height - 0.5f),
                             new Vector2(_waveformRect.position.x + _waveformRect.width, _waveformRect.position.y + _waveformRect.height - 0.5f));//- .0.5f so we can actually see the line
                                                                                                                                                   //otherwise it's just below the window view

        }
    }

}
