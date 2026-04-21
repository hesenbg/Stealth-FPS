using UnityEngine;

public class Destructable : MonoBehaviour
{
    [SerializeField] AudioClip SFX;
    [SerializeField] GameObject VFX;

    public void DestroyObject()
    {
        EnemyManager.instance.AlertCLosestEnemy(transform.position);
        Destroy(gameObject);
    }
}    