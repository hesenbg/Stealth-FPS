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

        if (GUILayout.Button("Preview Peek Spot"))
        {
            if (t.PeekThreatSource != null && t.PeekOriginSource != null)
            {
                t.CoverObjects = new GameObject[t.coverParent.transform.childCount];
                for (int i = 0; i < t.coverParent.transform.childCount; i++)
                    t.CoverObjects[i] = t.coverParent.transform.GetChild(i).gameObject;

                t.FindPeekSpot(
                    t.PeekThreatSource.position,
                    t.PeekOriginSource.position,
                    t.PeekRange,
                    out t.GizmoPeekPos,
                    out t.GizmoPeekDirection
                );
            }
            else
            {
                Debug.LogWarning("Assign PeekThreatSource and PeekOriginSource to preview.");
            }
        }
    }
}
public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    EnemyStateMachine[] enemies;

    [SerializeField] public List<Vector3> CoverPositions;
    [SerializeField] LayerMask CoverLayers;

    public GameObject[] CoverObjects;

    [SerializeField] public GameObject coverParent;

    [SerializeField] float CoverDistance; // cover pos's distance from its object (value between 1 and 0 is good)

    public Transform nem;

    public int numberofcovers;
    public float range;

    [Header("Peek Gizmo Preview")]
    public Transform PeekThreatSource;
    public Transform PeekOriginSource;
    public float PeekRange = 10f;

    [HideInInspector] public Vector3 GizmoPeekPos;
    [HideInInspector] public Vector3 GizmoPeekDirection;
    [HideInInspector] public bool GizmoPeekAvalibe;
    [HideInInspector] public Vector3 GizmoBackwardsFromPlayer;
    [HideInInspector] public Vector3 GizmoPeekSide;
    [HideInInspector] public Vector3 GizmoPeekPositionSearchPosition;

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

    public bool FindPeekSpot(Vector3 ThreatPos, Vector3 OriginPos, float Range, out Vector3 PeekPos, out Vector3 PeekDirection)
    {
        PeekPos = Vector3.zero;
        PeekDirection = Vector3.zero;
        bool Avalibe = false;

        GameObject Cover = null;
        float ClosestDistance = float.MaxValue;

        // get closest cover
        for (int i = 0; i < CoverObjects.Length; i++)
        {
            float dist = Vector3.Distance(CoverObjects[i].transform.position, OriginPos);
            if (dist < ClosestDistance)
            {
                Cover = CoverObjects[i];
                ClosestDistance = dist;
            }
        }

        // check if cover is closer than our range
        if (Cover == null || ClosestDistance > Range) return Avalibe;

        Collider CoverCollider = Cover.GetComponent<Collider>();
        if (CoverCollider == null) return Avalibe;

        // check if cover is in front of the enemy // hasnt done
        Vector3 DirectionToThreat = (ThreatPos- OriginPos).normalized;

        Vector3 DirectionToCover  = (CoverCollider.transform.position - OriginPos).normalized;

        float Dot = Vector3.Dot(DirectionToThreat, DirectionToCover);

        if (!(Dot > 0.5f))
            return Avalibe;

        // snap to closest local axis of cover object
        Vector3 BackwardsFromPlayer = (Cover.transform.position - ThreatPos).normalized;
        Vector3[] localAxes = {
            Cover.transform.forward,
            -Cover.transform.forward,
            Cover.transform.right,
            -Cover.transform.right
        };

        Vector3 snapped = localAxes[0];
        float bestDot = float.MinValue;
        for (int i = 0; i < localAxes.Length; i++)
        {                                                                                
            float dot = Vector3.Dot(BackwardsFromPlayer, localAxes[i]);                  
            if (dot > bestDot)                                                           
            {                                                                            
                bestDot = dot;                                                           
                snapped = localAxes[i];                                                  
            }                                                                            
        }                                                                                
        BackwardsFromPlayer = snapped;                                                   
        GizmoBackwardsFromPlayer = BackwardsFromPlayer;                                  
                                                                                         
        // check if enemy is the right or left side of the cover                         
        Vector3 CoverCenter = CoverCollider.bounds.center;                               
                                                                                         
        Vector3 CoverToOrigin = (OriginPos - CoverCenter);                               
                                                                                         
        float sideDot = Vector3.Dot(CoverToOrigin, Cover.transform.right);               
                                                                                         
        Vector3 PeekSide = sideDot > 0 ? Cover.transform.right : -Cover.transform.right; 

        GizmoPeekSide = PeekSide;                                                        

        //

        Vector3 extents = CoverCollider.bounds.extents;

        Vector3 absPeekSide = new Vector3(Mathf.Abs(PeekSide.x), Mathf.Abs(PeekSide.y), Mathf.Abs(PeekSide.z));

        // find the offset
        float PeekPosOffsetFromCenter = Vector3.Dot(extents, absPeekSide)*0.8f;   // float multipiler

        Vector3 PeekPositionSearchPosition = CoverCenter + BackwardsFromPlayer*2 +  PeekSide * PeekPosOffsetFromCenter - BackwardsFromPlayer/2 ;

        GizmoPeekPositionSearchPosition = PeekPositionSearchPosition;

        if (!NavMesh.SamplePosition(PeekPositionSearchPosition, out NavMeshHit hit, 1f, NavMesh.AllAreas)) return Avalibe; 

        if (!NavMesh.FindClosestEdge(hit.position, out hit, NavMesh.AllAreas)) return Avalibe;

        PeekPos = hit.position+ BackwardsFromPlayer*0.5f;

        // peek direction is depend of the enemy's local transform axix. just need to retunr either right or left and maybe magnitute

        Vector3 forward = (ThreatPos - PeekPos).normalized;

        Vector3 right = Vector3.Cross(Vector3.up, forward);

        PeekDirection = (Vector3.Dot(PeekSide, right) > 0 ? right : -right) * 0.85f;  // float multipiler 
        Avalibe = true;

        return Avalibe;
    }


    public List<Vector3> GenerateCoverPos(int NumberOfPositions, float Range, Vector3 Origin)
    {
        List<Vector3> coverposs = new List<Vector3>();
        foreach (GameObject cover in CoverObjects)
        {
            NavMeshHit hit;
            if (coverposs.Count >= NumberOfPositions) break;

            if (Vector3.Distance(cover.transform.position, Origin) > Range) continue;

            Vector3 DirectionToPos = (cover.transform.position - Origin).normalized;

            Vector3 CoverSize = GetMeshSize(cover);

            Vector3 CoverSurface = new Vector3(CoverSize.x * DirectionToPos.x, 0, CoverSize.z * DirectionToPos.z);

            if (!NavMesh.SamplePosition(cover.transform.position + CoverSurface.normalized, out hit, 1.5f, NavMesh.AllAreas)) continue;

            if (!NavMesh.FindClosestEdge(hit.position, out hit, NavMesh.AllAreas)) continue;

            coverposs.Add(hit.position * CoverDistance);
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

            if (navDist < sm.context.enemyAIData.CurrentAwarenessState.AudioDetetctionRange + Range && navDist < closestDist)
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
        closest.context.events.FireSusEvent(new EventData(pos, GetDirection(pos)));
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
        return (pos - transform.position).normalized;
    }



    private void OnDrawGizmos()
    {
        // draw the generated cover POSs
        for (int i = 0; i < CoverPositions.Count; i++)
        {
            Gizmos.DrawWireSphere(CoverPositions[i], 0.5f);
        }

        // draw peek spot result
        if (GizmoPeekAvalibe)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(GizmoPeekPos, 0.3f);
            Gizmos.DrawRay(GizmoPeekPos, GizmoPeekDirection.normalized );
        }
        else
        {
            Gizmos.color = Color.red;
            if (GizmoPeekPos != Vector3.zero)
                Gizmos.DrawWireSphere(GizmoPeekPos, 0.3f);
        }

        //// BackwardsFromPlayer — direction away from threat snapped to cover axis
        //Gizmos.color = Color.blue;
        //Gizmos.DrawRay(GizmoPeekPositionSearchPosition, GizmoBackwardsFromPlayer * 2f);
        //
        //// PeekSide — which side of the cover the enemy peeks from
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawRay(GizmoPeekPositionSearchPosition, GizmoPeekSide * 2f);
        //
        //// PeekPositionSearchPosition — raw world point before NavMesh snap
        //Gizmos.color = Color.cyan;
        //Gizmos.DrawWireSphere(GizmoPeekPositionSearchPosition, 0.2f);
    }
}