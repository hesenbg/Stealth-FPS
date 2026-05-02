using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Navmeshdistancecheck : MonoBehaviour
{
    [SerializeField] Transform a;
    [SerializeField] Transform b;

    public void CheckDistance()
    {
        //Debug.Log(EnemyManager.GetNavMeshDistance(a.position, b.position));
    }
}

[CustomEditor(typeof(Navmeshdistancecheck))]
public class NavmeshdistancecheckEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Navmeshdistancecheck t = (Navmeshdistancecheck)target;

        if (GUILayout.Button("Check Distance"))
            t.CheckDistance();
    }
}
