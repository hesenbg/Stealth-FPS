using UnityEngine;
abstract public class BaseNade : MonoBehaviour
{
    public abstract float EffectRadius { get; set; }

    [SerializeField] Mesh NadeMesh;

    abstract public void Effect();




}