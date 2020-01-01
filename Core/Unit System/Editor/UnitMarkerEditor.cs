using UnityEditor;
using UnitDataLibrary;
using UnityEngine;

[CustomEditor(typeof(UnitMarker), true)]
public class UnitMarkerEditor : Editor
{
    ClassType currentClass;
    int currentPlayer;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // Draw the built-in inspector
        UnitMarker um = (UnitMarker)target;
        if ((currentClass != um.classType || currentPlayer != um.PlayerNumber) && um.ClassLibrary != null)
        {
            um.updateSprite();
            currentClass = um.classType;
            currentPlayer = um.PlayerNumber;
        }
    }
}