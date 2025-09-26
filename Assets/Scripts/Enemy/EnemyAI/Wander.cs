using UnityEngine;
using static UnityEngine.UI.Image;

public class Wander : MonoBehaviour
{
    [SerializeField] Vector3[] Tracks;

    int CurrentTrackIndex;
    int MaxTrackIndex;
    Vector3 CurrentTrack;
    bool HasTrack;

    [SerializeField] float Speed;
    Vector3 Velocity;
    [SerializeField] float RotationSpeed;

    BaseAI Base;

    private void Start()
    {
        CurrentTrackIndex = 0;
        MaxTrackIndex = Tracks.Length;
        HasTrack = false;
        Velocity = Vector3.zero;

        Base = GetComponent<BaseAI>();
    }

    public void UpdateWander()  // defoult state that goes through certain positions(track - position)
    {
        CheckHasTrack();
        Base.UpdateRotation(Tracks[CurrentTrackIndex],RotationSpeed);
    }

    void GetTrackIndex()  // gets the next index from list that will be the position
    {
        if (CurrentTrackIndex < MaxTrackIndex - 1)
            CurrentTrackIndex++;
        else
            CurrentTrackIndex = 0;

        CurrentTrack = Tracks[CurrentTrackIndex];
        HasTrack = true;
    }

    void CheckHasTrack()  // forward enemy to the next track and checks it of reached
    {
        if (!HasTrack)
            GetTrackIndex();
        else
            HasTrack = TrackPositionDirect(CurrentTrack);
    }


    bool TrackPositionDirect(Vector3 targetPosition) // gives velocity based on given position
    {
        Vector3 direction = new Vector3(targetPosition.x - transform.position.x, 0, targetPosition.z - transform.position.z);
        Velocity = direction.normalized;
        Base.rb.linearVelocity = new Vector3(Velocity.x * Speed, 0, Velocity.z * Speed);

        return direction.magnitude >= 0.2f;
    }


    private void OnDrawGizmos()
    {
        if (Tracks == null || Tracks.Length == 0) return;

        Gizmos.color = Color.green;

        foreach (var track in Tracks)
        {
            Vector3 pos = new Vector3(track.x, transform.position.y, track.z);
            Gizmos.DrawSphere(pos, 0.3f);
        }

        for (int i = 0; i < Tracks.Length; i++)
        {
            Vector3 current = new Vector3(Tracks[i].x, transform.position.y, Tracks[i].z);
            Vector3 next = new Vector3(Tracks[(i + 1) % Tracks.Length].x, transform.position.y, Tracks[(i + 1) % Tracks.Length].z);
            Gizmos.DrawLine(current, next);
        }
    }

}
