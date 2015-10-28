using UnityEngine;
using UnityEditor;
using System.Collections;

[System.Serializable]
public class BaseEditorSubwindow : ScriptableObject {

    [SerializeField]
    public string windowName;

    [SerializeField]
    public Rect subwindowRect;

    [SerializeField]
    public float scrollPosX;
    [SerializeField]
    public float scrollPosY;

    public Vector2 scrollPos;

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

}
