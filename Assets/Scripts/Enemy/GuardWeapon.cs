using UnityEngine;
using static UnityEngine.UI.Image;

///  base logic for enemy object's shooting with raycast
///  raycasts every certain amount of time, and decreses player's health via healthmanager script in player gameobject
public class GuardWeapon : MonoBehaviour
{
    [SerializeField] float RayLength;
    [SerializeField] LayerMask HitMask;
    [SerializeField] float ShootDelay;
    [SerializeField] float ShootDelayValue;
    public bool IsShooting= false;
    HealthManager HealthManager;

    [SerializeField] Transform Origin;
    private void Start()
    {
        HealthManager = GameObject.Find("Player").GetComponent<HealthManager>();
    }
    private void Update()
    {
        if (IsShooting)
        {
            if (ShootDelayValue < ShootDelay)
            {
                ShootDelayValue += Time.deltaTime;
            }
            else
            {
                Shoot();
                ShootDelayValue = 0;
            }
        }
    }

    private void Shoot()
    {
        HealthManager.GotDamage(15);

        Ray ray = new Ray(Origin.transform.position,transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, RayLength, HitMask,QueryTriggerInteraction.Ignore))
        {
            hit.collider.gameObject.GetComponent<HealthManager>().GotDamage(30);
        }
        
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(Origin.transform.position, transform.forward * RayLength);
    }
}
