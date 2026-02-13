using NUnit.Framework;
using System.Collections.Generic;
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

    [SerializeField] private List<AudioClip> ShootSounds;
    [SerializeField] private List<AudioClip> headhitSound;
    [SerializeField] private List<AudioClip> bodyHitSound;

    [Header("Step Settings")]
    [SerializeField] private float walkStepInterval = 0.5f;
    [SerializeField] private float runStepInterval = 0.3f;
    [SerializeField] private float WalkSpeed;
    [SerializeField] private float RunSpeed;


    [Header("Volmues")]
    [SerializeField] float JumpVolume;
    [SerializeField] float LandVolume;
    [SerializeField] float ShootVolume;
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


    public void PlayShootSound()
    {
        if (ShootSounds == null || ShootSounds.Count == 0) return;

        int index = Random.Range(0, ShootSounds.Count);
        playerAudioSource.PlayOneShot(ShootSounds[index],ShootVolume);
    }

    public void BodyHitSound()
    {
        if (bodyHitSound == null || bodyHitSound.Count == 0) return;

        int index = Random.Range(0, bodyHitSound.Count);
        playerAudioSource.PlayOneShot(bodyHitSound[index]);
    }

    public void HeadShotHitSound()
    {
        if (headhitSound == null || headhitSound.Count == 0) return;

        int index = Random.Range(0, headhitSound.Count);
        playerAudioSource.PlayOneShot(headhitSound[index]);
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