using UnityEngine;
using UnityEditor;
/*
[CustomEditor(typeof(Unit_2), true)] // Tells editor which type it belongs to at run-time
public class UnitEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // Draw the built-in inspector
        Unit_2 u = (Unit_2)target; // Get the Unit_2 of the target being inspected.
        u.GetCurrentClass();

        if (GUILayout.Button("Show All Available Classes"))
        {
            Unit_2.PrintClasses();
        }
        if (GUILayout.Button("Show Current Class"))
        {
            u.GetUnitClass();
        }
    }

}
*/