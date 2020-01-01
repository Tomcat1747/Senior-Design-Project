using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UnitGenerator))]
public class CustomUnitGeneratorGUI : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        UnitGenerator unitGenerator = (UnitGenerator)target;

        if (GUILayout.Button("Snap to Grid")) {
            unitGenerator.SnapToGrid();
        }
    }
}
