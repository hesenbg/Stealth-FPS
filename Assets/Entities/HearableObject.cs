using UnityEngine;
public class HearableObject : MonoBehaviour, IHearable
{
    [SerializeField] HearableType type;
    [SerializeField] float range;

    public Transform Transform { get; set ; }
    public HearableType Type { get ; set ; }
    public float Range { get; set; }

    private void Start()
    {
        Range = range;
        Transform = transform;
        Type = type;
    }
}