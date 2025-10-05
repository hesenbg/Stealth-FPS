using UnityEngine;
using static UnityEngine.UI.Image;

public class Wonder : MonoBehaviour
{
    [SerializeField] Vector3[] Tracks;
    public enum WonderType { Walk , Still}
    public WonderType Current;
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
        switch (Current) {
            case WonderType.Walk:
                Walk();
                break;
            case WonderType.Still:
                Still();
                break;

        }
    }

    void Walk()
    {
        CheckHasTrack();
        Base.UpdateRotation(Tracks[CurrentTrackIndex],RotationSpeed);
    }

    bool isStillCoroutineRunning = false;

    void Still()
    {
        if (!isStillCoroutineRunning)
            StartCoroutine(StillRoutine());
    }
    [SerializeField] float CheckDuration;
    [SerializeField] float RotationDegree;

    System.Collections.IEnumerator StillRoutine()
    {
        isStillCoroutineRunning = true;

        while (Current == WonderType.Still)
        {
            yield return RotateByAngle(-RotationDegree);

            yield return new WaitForSeconds(CheckDuration);

            yield return RotateByAngle(RotationDegree*2);

            yield return new WaitForSeconds(CheckDuration);

            yield return RotateByAngle(-RotationDegree);

            yield return new WaitForSeconds(1f);
        }

        isStillCoroutineRunning = false;
    }

    // Smooth rotation coroutine
    System.Collections.IEnumerator RotateByAngle(float angle)
    {
        float targetY = transform.eulerAngles.y + angle;
        float elapsed = 0f;
        float duration = 0.5f; // adjust rotation speed here

        float startY = transform.eulerAngles.y;

        while (elapsed < duration)
        {
            float newY = Mathf.LerpAngle(startY, targetY, elapsed / duration);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, newY, transform.eulerAngles.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // aplly the rotation
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, targetY, transform.eulerAngles.z);
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
        if (Tracks == null || Tracks.Length == 0 || Current== WonderType.Still) return;

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
