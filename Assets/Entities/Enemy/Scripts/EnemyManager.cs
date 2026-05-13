using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;


[CustomEditor(typeof(EnemyManager))]
public class YourClassNameEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EnemyManager t = (EnemyManager)target;
        if (GUILayout.Button("Generate Cover Positions"))
        {
            EnemyManager.instance.CoverPositions = t.GenerateCoverPos(EnemyManager.instance.numberofcovers
                , EnemyManager.instance.range, EnemyManager.instance.nem.position);
        }
    }
}
public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    EnemyStateMachine[] enemies;

    [SerializeField] public List<Vector3> CoverPositions;
    [SerializeField] LayerMask CoverLayers;

    GameObject[] CoverObjects;

    [SerializeField] GameObject coverParent;

    [SerializeField] float CoverDistance; // cover pos's distance from its object (value between 1 and 0 is good)

    public Transform nem;

    public int numberofcovers;
    public float range;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        CoverObjects = new GameObject[coverParent.transform.childCount];
        for (int i = 0; i < coverParent.transform.childCount; i++)
            CoverObjects[i] = coverParent.transform.GetChild(i).gameObject;
    }

    // helper functions
    void CheckEnemies()
    {
        enemies = GetComponentsInChildren<EnemyStateMachine>();
    }

    public List<Vector3> GenerateCoverPos(int NumberOfPositions, float Range, Vector3 Origin)
    {
        List<Vector3> coverposs = new List<Vector3>();

        foreach(GameObject cover  in CoverObjects)
        {
            NavMeshHit hit;

            if (coverposs.Count >= NumberOfPositions) break;

            if (Vector3.Distance(cover.transform.position, Origin) > Range) continue;

            Vector3 DirectionToPos = (cover.transform.position - Origin).normalized;

            Vector3 CoverSize = GetMeshSize(cover);
            
            Vector3 CoverSurface = new Vector3(CoverSize.x*DirectionToPos.x,0,CoverSize.z*DirectionToPos.z);

            if(!NavMesh.SamplePosition(cover.transform.position + CoverSurface,out hit,1.5f, NavMesh.AllAreas)) continue;

            if(!NavMesh.FindClosestEdge(hit.position, out hit, NavMesh.AllAreas)) continue;

            coverposs.Add(hit.position*CoverDistance);
        }
        return coverposs;
    }

    Vector3 GetMeshSize(GameObject meshObject)
    {
        Renderer renderer = meshObject.GetComponent<Renderer>();
        return renderer.bounds.size;
    }

    private EnemyStateMachine CheckEnemyCloseDirect(Vector3 pos, float Range)
    {
        CheckEnemies();
        EnemyStateMachine closest = null;
        float closestDist = float.MaxValue;

        foreach (EnemyStateMachine sm in enemies)
        {
            float navDist = GetNavMeshDistance(sm.transform.position, pos);

            if (navDist < sm.context.enemyAIData.CurrentAwarenessState.AudioDetetctionRange+ Range && navDist < closestDist)
            {
                closest = sm;
                closestDist = navDist;
            }
        }
        return closest;
    }

    private EnemyStateMachine CheckEnemyCloseAngle(Vector3 pos, Vector3 dir)
    {
        CheckEnemies();
        EnemyStateMachine closest = null;
        float smallestDist = float.MaxValue;

        foreach (EnemyStateMachine sm in enemies)
        {
            Vector3 toEnemy = sm.transform.position - pos;
            float perpDist = Vector3.Cross(dir, toEnemy).magnitude;

            if (perpDist < smallestDist && perpDist < sm.context.enemyAIData.CurrentAwarenessState.AudioDetetctionRange)
            {
                smallestDist = perpDist;
                closest = sm;
            }
        }

        Debug.Log(smallestDist);
        return closest;
    }

    private static float GetNavMeshDistance(Vector3 from, Vector3 to)
    {
        NavMeshPath path = new NavMeshPath();
        if (!NavMesh.CalculatePath(from, to, NavMesh.AllAreas, path))
            return float.MaxValue;
        if (path.status == NavMeshPathStatus.PathInvalid)
            return float.MaxValue;

        float distance = 0f;
        Vector3[] corners = path.corners;
        for (int i = 1; i < corners.Length; i++)
            distance += Vector3.Distance(corners[i - 1], corners[i]);
        return distance;
    }

    // action functions
    public void AlertClosestOnSuspiciousEvent(Vector3 pos, HearableObject hearable)
    {
        EnemyStateMachine closest = CheckEnemyCloseDirect(pos, hearable.Range);
        if (closest == null) return;
        closest.context.events.FireSusEvent(new EventData(pos,GetDirection(pos)));
    }

    public void AlertClosestOnGunFire(Vector3 pos, Vector3 dir) 
    {
        EnemyStateMachine closest = CheckEnemyCloseAngle(pos, dir);
        if (closest == null) return;
        closest.context.events.FireSusEvent(new EventData(GetDirection(pos)));
    }

    public void AlertClosestAllies(Vector3 pos, int NumberOfAllies)
    {

    }

    public Vector3 GetDirection(Vector3 pos)
    {
        return (pos- transform.position).normalized;
    }

    private void OnDrawGizmos()
    {
        // draw the generated cover POSs
        for(int i =0;i< CoverPositions.Count; i++)
        {
            Gizmos.DrawWireSphere(CoverPositions[i], 0.5f);
        }
    }
}