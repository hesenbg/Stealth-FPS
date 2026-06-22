using UnityEngine;

public class Bush : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<ObservableObject>(out ObservableObject obs))
        {
            obs.AddModifier(gameObject.name, 0f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<ObservableObject>(out ObservableObject obs))
        {
            obs.RemoveModifier(gameObject.name);
        }
    }
}
