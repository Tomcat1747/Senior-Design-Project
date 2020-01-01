using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (UnitMarkerManager))]
public class UnitMarkerManagerEditor : Editor {
    public override void OnInspectorGUI () {
        base.OnInspectorGUI ();
        UnitMarkerManager unitMarker = (UnitMarkerManager) target;

        if (GUILayout.Button ("Snap to Grid")) {
            unitMarker.SnapToGrid();
        }
    }
}