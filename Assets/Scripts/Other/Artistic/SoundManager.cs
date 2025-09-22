using UnityEngine;
using UnityEngine.UIElements;

public class SoundManager : MonoBehaviour 
{
    public static SoundManager Instance { get;  set; }

    [Header("Audio Sources")]
    [SerializeField] AudioSource DestructablesAS;
    [SerializeField] AudioSource PlayerAS;
    [SerializeField] AudioSource EnemyAS;
    [SerializeField] AudioSource PlayerGun;

    [Header("Audio Clips")]
    [SerializeField] AudioClip GlassShatter;
    [SerializeField] AudioClip LightbulbShatter;
    [SerializeField] AudioClip PlayerFootStep;
    [SerializeField] AudioClip PlayerGunShot;
    [SerializeField] AudioClip PlayerGunReload;
    [SerializeField] AudioClip HSindicator;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public void PlayerLightbulbShatter(Vector3 position)
    {
        AudioSource.PlayClipAtPoint(LightbulbShatter, position);
        ParentEnemy.Instance.TriggerClosestEnemy(position);
    }
    public void PlayGlassShatter(Vector3 position)
    {
        AudioSource.PlayClipAtPoint(GlassShatter, position);
        ParentEnemy.Instance.TriggerClosestEnemy(position);
    }
    public void PlayPlayerFootSteps(Vector3 position)
    {
        PlayerAS.transform.position = position;
        if (!PlayerAS.isPlaying)
        {
            PlayerAS.PlayOneShot(PlayerFootStep);
        }
        ParentEnemy.Instance.TriggerClosestEnemy(position);     
    }
    public void PlayHeadShotIndicator(Vector3 position)
    {
        AudioSource.PlayClipAtPoint(HSindicator, position,1);
    }

    public void PlayGunShot(Vector3 poition)
    {
        AudioSource.PlayClipAtPoint(PlayerGunShot, poition,0.7f);
    }
    
    public void PlayReload(Vector3 position)
    {
        AudioSource.PlayClipAtPoint(PlayerGunReload, position);
    }

}
