using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapConfig))]
public class MapConfigEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MapConfig mapData = (MapConfig)target;

        if (GUILayout.Button("Parse Map Data"))
        {
            mapData.ParseMap();
        }
    }
}