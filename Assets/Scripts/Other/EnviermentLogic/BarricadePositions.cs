using System;
using Unity.VisualScripting;
using UnityEngine;

public class BarricadePositions : MonoBehaviour
{
    public GameObject[] Positions;

    public bool IsRight;

    public Vector3 FindClosestPosition(Vector3 position)
    {
        GameObject Closest = null;
        float MinDistance = Mathf.Infinity;
        float CurrDistance;
        foreach (GameObject pos in Positions)
        {
            CurrDistance = Vector3.Distance(position,pos.transform.position);
            if( CurrDistance < MinDistance)
            {
                MinDistance = CurrDistance;
                Closest = pos;
            }
        }
        IsRight = Closest.tag == "BarrierPosRight";

        return Closest.transform.position;
    }




}
