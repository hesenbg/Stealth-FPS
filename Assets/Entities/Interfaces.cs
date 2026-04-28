using UnityEngine;
public enum ObservableType { Hostile, Clue}
public interface IObservable
{
    public bool HasSeen {  get;  set; }
    public Transform Transform { get; set; }
    public ObservableType Type { get; set; }
    public int Priority { get; set; } // lower number of priority means it is more important( enum layers)
    public float Observability { get; set; } // value between 1 and 0. vision cones awarness speed* observability
}