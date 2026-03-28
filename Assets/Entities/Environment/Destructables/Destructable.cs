using UnityEngine;

public class Destructable : MonoBehaviour
{
    [SerializeField] AudioClip SFX;
    [SerializeField] GameObject VFX;

    public void DestroyObject()
    {
        EnemyManager.instance.AlertClosestEnemy(transform.position);

        Destroy(gameObject);
    }
}
