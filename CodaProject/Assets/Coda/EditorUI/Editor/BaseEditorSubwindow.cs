using UnityEngine;
using UnityEditor;
using System.Collections;

[System.Serializable]
public abstract class BaseEditorSubwindow : ScriptableObject {

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
        //Debug.Log("Blown up");//debug for when Unity destroys an instance of this Subwindow
    }                                     

    /// <summary>
    /// Handle Setup here
    /// </summary>
    /// <param name="windowDimensions"></param>
    public virtual void Setup(Rect windowDimensions) {//Can't pass values through Scriptable Object's constructor, need to make a Setup class
        subwindowRect = windowDimensions;

        scrollPos = new Vector2(scrollPosX, scrollPosY);
    }

    /// <summary>
    /// Serialize this Subwindow
    /// </summary>
    public virtual void SaveSettings() {
        //scrollPosX = scrollPos.x;
        //scrollPosY = scrollPos.y;
    }

    /// <summary>
    /// Handle drawing logic
    /// </summary>
    /// <param name="id"></param>
    /// <param name="windowName"></param>
    public virtual void DrawSubwindow(int id, string windowName) {
        //Debug.Log("Drawing Subwindows");

        subwindowRect = GUILayout.Window(id, subwindowRect, DoWindowContents, windowName);
        

    }

    /// <summary>
    /// Implement what to draw in the window here (glorified Update loop)
    /// </summary>
    /// <param name="unusedWindowID"></param>
    public abstract void DoWindowContents(int unusedWindowID); 

    /// <summary>
    /// Check if supplied position is in this subwindow
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public virtual bool IsInSubwindow(Vector2 position) {
        //Debug.LogFormat("Pos: {0}, Rect: {1}", position, subwindowRect);
        if (subwindowRect.Contains(position)) {
            return true;
        }
        return false;
    }

/// <summary>
/// DEBUG: Draw the borders of a subwindow (adjusted for the current window)
/// </summary>
    public virtual void DrawWindowDebug() {
        DrawWindowDebug(thisSubwindowRect);
    }

/// <summary>
/// DEBUG: Draw the borders of a subwindow
/// </summary>
/// <param name="drawRect"></param>
    public virtual void DrawWindowDebug(Rect drawRect) {
        Color originalHandleColor = Handles.color;
        Handles.color = debugColor;

        //use AA Poly Line so we can modify thickness
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
