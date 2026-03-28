using UnityEngine;

public class Destructable : MonoBehaviour
{
    [SerializeField] AudioClip SFX;
    [SerializeField] GameObject VFX;

    public void DestroyObject()
    {
        //AudioSource.PlayClipAtPoint(SFX,transform.position);
        if(VFX!=null)
            //Instantiate(VFX);

        EnemyManager.instance.AlertClosestEnemy(transform.position);

        Destroy(gameObject);
    }
}
