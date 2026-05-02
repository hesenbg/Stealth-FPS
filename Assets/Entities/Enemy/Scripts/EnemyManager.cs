using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.PlayerSettings;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    EnemyStateMachine[] enemies;
    private void Awake()
    {
        instance = this;
    }

    // helper functions
    void CheckEnemies()
    {
        enemies = GetComponentsInChildren<EnemyStateMachine>();
    }

    private EnemyStateMachine CheckEnemyCloseDirect(Vector3 pos)
    {
        CheckEnemies();
        EnemyStateMachine closest = null;
        float closestDist = float.MaxValue;

        foreach (EnemyStateMachine sm in enemies)
        {
            float navDist = GetNavMeshDistance(sm.transform.position, pos);

            if (navDist < sm.context.enemyAIData.Range && navDist < closestDist)
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

            if (perpDist < smallestDist && perpDist < sm.context.enemyAIData.BulletHearMaxAngle)
            {
                smallestDist = perpDist;
                closest = sm;
            }
        }

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
    public void AlertClosestSuspicious(Vector3 pos)
    {
        EnemyStateMachine closest = CheckEnemyCloseDirect(pos);
        if (closest == null) return;
        closest.context.events.FireSusEvent(pos);
    }

    public void AlertClosestGunFire(Vector3 pos, Vector3 dir) //
    {
        EnemyStateMachine closest = CheckEnemyCloseAngle(pos, dir);
        if (closest == null) return;
        closest.context.events.FireClueFound(pos);
    }

    public void AlertClosestAllies()
    {

    }
}