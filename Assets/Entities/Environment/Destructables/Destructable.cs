using UnityEngine;

public class Destructable : MonoBehaviour
{
    [SerializeField] AudioClip SFX;
    [SerializeField] GameObject VFX;

    public void DestroyObject()
    {
        Debug.Log(EnemyManager.instance.CheckEnemyCloseDirect(transform.position));

        EnemyManager.instance.CheckEnemyCloseDirect(transform.position).GetComponentInChildren<EnemyEvents>().FireSusEvent();

        Destroy(gameObject);
    }
}   