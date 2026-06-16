using UnityEngine;

public class Destructable : MonoBehaviour
{
    [SerializeField] AudioClip SFX;
    [SerializeField] GameObject VFX;
    [SerializeField] HearableObject hearable;
    [SerializeField] GameObject brokenGlasses;

    public void DestroyObject()
    {
        EnemyManager.instance.AlertClosestOnSuspiciousEvent(transform.position,hearable);
        Instantiate(brokenGlasses);
        Destroy(gameObject);
    }
}    