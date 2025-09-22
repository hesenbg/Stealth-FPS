using UnityEngine;

public class Knife : MonoBehaviour
{
    [SerializeField] float AttackDistance;
    [SerializeField] LayerMask AffectObjects;

    [SerializeField] Vector3 HitBoxhalfExtend;
    public bool Attack()
    {
        if (Physics.CheckBox(transform.position, HitBoxhalfExtend, transform.rotation, AffectObjects, QueryTriggerInteraction.Ignore))
        {
            return true;    
        }
        return false;
    }

}
