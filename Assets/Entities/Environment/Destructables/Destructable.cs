using UnityEngine;

public class Destructable : MonoBehaviour
{
    [SerializeField] AudioClip SFX;
    [SerializeField] GameObject VFX;
    [SerializeField] HearableObject hearable;

    public void DestroyObject()
    {
        EnemyManager.instance.AlertClosestOnSuspiciousEvent(transform.position,hearable);
        Destroy(gameObject);
    }
}    