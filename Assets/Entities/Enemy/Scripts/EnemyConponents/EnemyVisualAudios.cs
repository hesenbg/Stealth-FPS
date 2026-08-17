using UnityEngine;

public class EnemyVisualAudios : MonoBehaviour
{
    public static EnemyVisualAudios instance; 

    [Header("Visual Effects")]
    [SerializeField] GameObject BloodEffect;
    [SerializeField] float effectDestroyTime = 2f;

    [Header("Sound Effects")]
    [SerializeField] AudioClip BodyHitSound;
    [SerializeField] AudioClip HeadHitSound;

    [Range(0f, 1f)][SerializeField] float BodyHitVolume = 1f;
    [Range(0f, 1f)][SerializeField] float HeadHitVolume = 1f;

    [SerializeField] AudioClip EnemyFootStep;

    [SerializeField] AudioClip GuardShoot;

    [SerializeField] AudioClip SniperShoot;

    public void PlayFootStep(Vector3 pos)
    {
        AudioSource.PlayClipAtPoint(EnemyFootStep, pos,1f);
    }

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void PlayBloodVFX(Vector3 WorldPos)
    {
        GameObject effect = Instantiate(BloodEffect, WorldPos, Quaternion.identity);
        Destroy(effect, effectDestroyTime);
    }

    public void PlayBodyHit(Vector3 WorldPos)
    {
        AudioSource.PlayClipAtPoint(BodyHitSound, WorldPos, BodyHitVolume);
    }

    public void PlayHeadHit(Vector3 WorldPos)
    {
        AudioSource.PlayClipAtPoint(HeadHitSound, WorldPos, HeadHitVolume);
    }

    public void PlayPistolFireSound(Vector3 pos)
    {
        AudioSource.PlayClipAtPoint(GuardShoot, pos);
    }


    public void PlaySniperFireSound(Vector3 pos)
    {
        AudioSource.PlayClipAtPoint(SniperShoot, pos);
    }

    public void PlayDeathSound(Vector3 pos)
    {

    }
}