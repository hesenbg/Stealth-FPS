using Unity.AI.Navigation;
using UnityEngine;
public class Destructable : MonoBehaviour
{
    public bool IsDestroyed;
    [SerializeField] GameObject FlameDestructionEffect;
    [SerializeField] Light Source;
    public enum DestructableType  {Light,Solid};
    [SerializeField] DestructableType ObjectType;
    [SerializeField] NavMeshSurface meshSurface;

    private void Start()
    {
        //meshSurface = GameObject.Find("Terrain").GetComponent<NavMeshSurface>();
    }
    private void FixedUpdate()
    {
        if (IsDestroyed)
        {

        }
    }
    public void DestroyObject()
    {
        if (!IsDestroyed)
        {
            EffectSound(ObjectType);
            Destroy(gameObject);
        }
    }

    void EffectSound(DestructableType Type)
    {
        if(Type == DestructableType.Solid)
        {
            SoundManager.Instance.PlayGlassShatter(transform.position);
        }
        if(Type == DestructableType.Light)
        {
            Source.range = 0;
            //Instantiate(FlameDestructionEffect, transform.position, transform.rotation);
            SoundManager.Instance.PlayerLightbulbShatter(transform.position);
        }
    }
}
