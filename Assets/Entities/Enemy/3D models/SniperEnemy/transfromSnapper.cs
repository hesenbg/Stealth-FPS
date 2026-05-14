// TransformSnapshot.cs  (put in any regular folder)
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TransformSnapshot : MonoBehaviour
{
    [System.Serializable]
    public class TransformData
    {
        public string path;
        public Vector3 localPosition;
        public Quaternion localRotation;
        public Vector3 localScale;
    }

    public List<TransformData> snapshot = new List<TransformData>();

    public void Save()
    {
        snapshot.Clear();
        foreach (Transform child in GetComponentsInChildren<Transform>(true))
        {
            if (child == transform) continue;
            snapshot.Add(new TransformData
            {
                path = GetPath(child),
                localPosition = child.localPosition,
                localRotation = child.localRotation,
                localScale = child.localScale
            });
        }
    }

    public void Apply()
    {
        foreach (var data in snapshot)
        {
            Transform t = transform.Find(data.path);
            if (t == null) continue;
            t.localPosition = data.localPosition;
            t.localRotation = data.localRotation;
            t.localScale = data.localScale;
        }
    }

    string GetPath(Transform t)
    {
        string path = t.name;
        while (t.parent != null && t.parent != transform)
        {
            t = t.parent;
            path = t.name + "/" + path;
        }
        return path;
    }
}

[CustomEditor(typeof(TransformSnapshot))]
public class TransformSnapshotEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        TransformSnapshot t = (TransformSnapshot)target;

        if (GUILayout.Button("Save Snapshot"))
        {
            Undo.RecordObject(t, "Save Snapshot");
            t.Save();
            EditorUtility.SetDirty(t);
        }

        if (GUILayout.Button("Apply Snapshot"))
        {
            Undo.RecordObject(t, "Apply Snapshot");
            t.Apply();
        }
    }
}