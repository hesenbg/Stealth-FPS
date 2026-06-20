using UnityEngine;
public enum HearableType { Weak, Moderate, Strong }

public class HearableObject : MonoBehaviour
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