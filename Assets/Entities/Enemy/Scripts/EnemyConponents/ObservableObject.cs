using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObservableObject : MonoBehaviour
{
    public Transform Transform { get; private set; }
    public ObservableType Type { get; private set; }
    public int Priority { get;private set; }
    public float Observability;
    public bool HasSeen { get; private set; }

    [SerializeField] ObservableType type;

    [SerializeField] int priority;

    [SerializeField] float BaseObservability;

    [SerializeField] Dictionary<string, float> modifiers = new Dictionary<string, float>();

    private void Start()
    {
        UpdateParams();
    }

    private void Update()
    {
        Observability = GetObservability();
    }

    public float GetObservability()
    {
        if (modifiers.Count == 0 ) return BaseObservability;
        return modifiers.Values.Max();
    }

    public void AddModifier(string key, float value) => modifiers[key] = value;

    public void RemoveModifier(string key) => modifiers.Remove(key);

    public void SetObservability(float value)
    {
        Observability = value;
    }

    private void OnValidate()
    {
        UpdateParams();
    }

    public void SetHasSeen()
    {
        HasSeen = true;
    }

    void UpdateParams()
    {
        Transform = transform;
        Type = type;
        Priority = priority;
    }
}