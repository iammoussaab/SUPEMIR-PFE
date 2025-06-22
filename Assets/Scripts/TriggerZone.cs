using UnityEngine;
using UnityEngine.Events;

public class TriggerZone : MonoBehaviour
{
    public string TargetTag;
    public UnityEvent<GameObject> OnEnterEvent;
    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == TargetTag)
        {
            OnEnterEvent.Invoke(other.gameObject);
        }

    }
}
