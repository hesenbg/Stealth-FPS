using UnityEngine;

public class Destructable : MonoBehaviour
{
    [SerializeField] AudioClip SFX;
    [SerializeField] GameObject VFX;

    public void DestroyObject()
    {
        EnemyManager.instance.AlertClosestSuspicious(transform.position);
        Destroy(gameObject);
    }
}    