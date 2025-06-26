using UnityEngine;
using UnityEngine.XR.Content.Interaction;

public class SpaceOutsideController : MonoBehaviour
{
    public XRLever Lever;
    public XRKnob knob;

    public float ForwardSpeed;
    public float SideSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        float ForwardVelocity = ForwardSpeed * (Lever.value ? 1 : 0);
        float SideVelocity = SideSpeed * (Lever.value ? 1 : 0) * (Mathf.Lerp(-1,1,knob.value));

        Vector3 Velocity = new Vector3 (SideVelocity, 0, ForwardVelocity);
        transform.position += Velocity * Time.deltaTime;

    }
}
