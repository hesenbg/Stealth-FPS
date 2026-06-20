using System.Collections;
using UnityEngine;
public class SafeLogic : MonoBehaviour, Interactable
{
    [SerializeField] GameObject Door;
    [SerializeField] float Angle;
    bool HasOpened;
    BoxCollider Collider;

    private void Start()
    {
        Collider = GetComponent<BoxCollider>();
    }
    public void OnInteract()
    {
        if (!HasOpened) //&& LootableItemInventory.Instance.keyCount>0
        {
            

            LootableItemInventory.Instance.DecreaseKeyCount();
            HasOpened = true;
            Collider.enabled = false;
            StartCoroutine(OpenSafe());
        }
    }
    IEnumerator OpenSafe()
    {
        Quaternion start = Door.transform.localRotation;
        Quaternion end = Quaternion.Euler(0, -Angle, 0);
        float t = 0;

        while (t < 1f)
        {
            t += Time.deltaTime * 2f;
            Door.transform.localRotation = Quaternion.Lerp(start, end, t);
            yield return null;
        }

        Door.transform.localRotation = end;
    }
}