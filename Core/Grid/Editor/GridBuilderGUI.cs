using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(ICellGridBuilder), true)]
public class GridGeneratorHelper : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        ICellGridBuilder gridGenerator = (ICellGridBuilder)target;

        if (GUILayout.Button("Generate Grid")) {
            ClearNodes(gridGenerator);
            gridGenerator.GenerateGrid();
        }
        if (GUILayout.Button("Clear Grid")) {
            ClearNodes(gridGenerator);
        }
    }

    void ClearNodes(ICellGridBuilder gridGenerator) {
        var children = new List<GameObject>();
        foreach (Transform cell in gridGenerator.CellsParent) {
            children.Add(cell.gameObject);
        }
        foreach (Transform cell in gridGenerator.DefaultParent) {
            children.Add(cell.gameObject);
        }
        children.ForEach(c => DestroyImmediate(c));
    }

}
