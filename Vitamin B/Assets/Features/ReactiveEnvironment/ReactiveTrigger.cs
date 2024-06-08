using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class ReactiveTrigger : MonoBehaviour
{
    public UnityEvent<Collider> onTriggerEnter;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            onTriggerEnter.Invoke(other);
    }
}