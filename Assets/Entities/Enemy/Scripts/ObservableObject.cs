using UnityEngine;
public class ObservableObject : MonoBehaviour,IObservable
{
    public Transform Transform { get;  set; }
    public ObservableType Type { get; set; }
    public int Priority { get; set; }
    public float Observability { get; set; }
    public bool HasSeen { get  ; set ; }

    [SerializeField] ObservableType type;
    [SerializeField] int priority;
    [SerializeField] float observability;

    private void Start()
    {
        UpdateParams();
    }

    private void OnValidate()
    {
        UpdateParams();
    }

    public void SetHasSeen()
    {
        HasSeen = true;
    }

    public bool GetHasSeen()
    {
        return HasSeen;
    }

    void UpdateParams()
    {
        Transform = transform;
        Type = type;
        Priority = priority;
        Observability = observability;
    }
}