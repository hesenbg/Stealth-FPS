using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    public static PlayerSoundManager instance { get; private set; }

    [Header("Sound Sources")]
    [SerializeField] private AudioSource playerAudioSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip footStep1;
    [SerializeField] private AudioClip footStep2;

    [SerializeField] private AudioClip JumpOff;
    [SerializeField] private AudioClip Land;

    [Header("Step Settings")]
    [SerializeField] private float walkStepInterval = 0.5f;
    [SerializeField] private float runStepInterval = 0.3f;
    [SerializeField] private float WalkSpeed;
    [SerializeField] private float RunSpeed;


    [Header("Volmues")]
    [SerializeField] float JumpVolume;
    [SerializeField] float LandVolume;

    [SerializeField] float StepVolume;

    float stepTimer;
    int stepIndex; 

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public void PlayWalk()
    {
        HandleFootsteps(WalkSpeed, walkStepInterval);
    }

    public void PlayRun()
    {
        HandleFootsteps(RunSpeed, runStepInterval);
    }

    public void PlayJump()
    {
        playerAudioSource.PlayOneShot(JumpOff,JumpVolume);
    }

    public void PlayLand()
    {
        playerAudioSource.PlayOneShot(Land,LandVolume);
    }

    void HandleFootsteps(float speed, float baseInterval)
    {
        if (speed <= 0.1f)
        {
            stepTimer = 0f;
            return;
        }

        stepTimer += Time.deltaTime;

        float interval = baseInterval / Mathf.Max(speed, 0.1f);

        if (stepTimer >= interval)
        {
            PlayNextStep();
            stepTimer = 0f;
        }
    }

    void PlayNextStep()
    {
        playerAudioSource.PlayOneShot((stepIndex == 0) ? footStep1 : footStep2,StepVolume) ;
        stepIndex = 1 - stepIndex; // toggles 0 <-> 1
    }
}