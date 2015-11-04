using UnityEngine;
using UnityEditor;
using System.Collections;

[System.Serializable]
public class BaseEditorSubwindow : ScriptableObject {

    [SerializeField]
    public string windowName;

    [SerializeField]
    public Rect subwindowRect;
    protected Rect thisSubwindowRect {
        get { return (new Rect(0, 0, subwindowRect.width, subwindowRect.height)); }
    }

    [SerializeField]
    public float scrollPosX;
    [SerializeField]
    public float scrollPosY;

    public Vector2 scrollPos;

    protected Color debugColor = Color.green;
    protected float debugSize = 5.0f;

    protected virtual void OnDestroy() {
        Debug.Log("Blown up");
    }                                     

    public virtual void Setup(Rect windowDimensions) {
        subwindowRect = windowDimensions;

        scrollPos = new Vector2(scrollPosX, scrollPosY);
    }

    public virtual void SaveSettings() {
        scrollPosX = scrollPos.x;
        scrollPosY = scrollPos.y;
    }

    public virtual void DrawSubwindow(int id, string windowName) {
        //Debug.Log("Drawing Subwindows");

        subwindowRect = GUILayout.Window(id, subwindowRect, DoWindowContents, windowName);
        

    }


    public virtual void DoWindowContents(int unusedWindowID) {

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        //GUI.DragWindow();
        /*
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        for (int i = 0; i < 50; ++i) {
            GUILayout.Button("Test");
        }

        EditorGUILayout.EndScrollView();
        */
        //GUI.DragWindow();

        EditorGUILayout.EndScrollView();

    }

    public virtual bool IsInSubwindow(Vector2 position) {
        //Debug.LogFormat("Pos: {0}, Rect: {1}", position, subwindowRect);
        if (subwindowRect.Contains(position)) {
            return true;
        }
        return false;
    }

/// <summary>
/// Draw the borders of a subwindow (adjusted for the current window)
/// </summary>
    public virtual void DrawWindowDebug() {
        DrawWindowDebug(thisSubwindowRect);
    }

/// <summary>
/// Draw the borders of a subwindow
/// </summary>
/// <param name="drawRect"></param>
    public virtual void DrawWindowDebug(Rect drawRect) {
        Color originalHandleColor = Handles.color;
        Handles.color = debugColor;

        Handles.DrawAAPolyLine(debugSize,//size
                                //positions
                               drawRect.position,
                               new Vector2(drawRect.position.x + drawRect.width, drawRect.position.y),
                               new Vector2(drawRect.position.x + drawRect.width, drawRect.position.y + drawRect.height),
                               new Vector2(drawRect.position.x, drawRect.position.y + drawRect.height),
                               drawRect.position);


        Handles.color = originalHandleColor;
    }
}
