using UnityEngine;


public class buttonPushDoor : MonoBehaviour
{

    public Animator animator;

    public string boolName = "open";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>().selectEntered.AddListener(x => ToggledDoorOpen());

    }

    public void ToggledDoorOpen()
    {
        bool IsOpen = animator.GetBool(boolName);
        animator.SetBool(boolName, !IsOpen);

    }
    // Update is called once per frame
    void Update()
    {

    }
}
