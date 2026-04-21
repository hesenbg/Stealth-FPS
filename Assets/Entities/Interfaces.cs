using UnityEngine;
public class Interfaces :MonoBehaviour
{
}
public enum ObservableType { Hostile, Clue}
public interface IObservable
{
    public void SetTransform(Transform _transform)
    {
        transform = _transform;
    }
    Transform transform { get; set; }
    ObservableType type { get; set; }
    int Priority { get; set; } // lower number of priority means it is more important( enum layers)
    float observability { get; set; } // value between 1 and 0. vision cones awarness speed* observability
}