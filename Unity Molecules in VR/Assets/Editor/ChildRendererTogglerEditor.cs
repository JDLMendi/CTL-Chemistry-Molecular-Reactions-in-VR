using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ChildRendererToggler))]
public class ChildRendererTogglerEditor : Editor
{
    // This function is called to draw the custom Inspector GUI.
    public override void OnInspectorGUI()
    {
        // Draw the default Inspector fields (like the script field).
        DrawDefaultInspector();

        // Get a reference to the script we are creating an editor for.
        ChildRendererToggler script = (ChildRendererToggler)target;

        // Add a space for better layout.
        EditorGUILayout.Space();

        // Create a button with the label "Toggle Child Renderers".
        // If this button is pressed in the Inspector...
        if (GUILayout.Button("Toggle Child Renderers"))
        {
            // ...call the public function on our script.
            script.ToggleAllChildRenderers();
        }
    }
}