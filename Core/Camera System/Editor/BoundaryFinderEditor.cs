using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BoundaryFinder), true)] // Tells editor which type it belongs to at run-time
public class BoundaryFinderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // Draw the built-in inspector
        BoundaryFinder u = (BoundaryFinder)target; // Get the Unit_2 of the target being inspected.

        if (GUILayout.Button("Set Boundaries"))
        {
            u.getBoundary();
        }
    }

}