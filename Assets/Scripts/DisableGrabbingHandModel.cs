using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;


public class DisableGrabbingHandModel : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject RightHandModel;
    public GameObject LeftHandModel;
    void Start()
    {
        UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(HideGrabbingHandModel);
        grabInteractable.selectExited.AddListener(ShowGrabbingHandModel);

    }

    public void HideGrabbingHandModel(SelectEnterEventArgs args)
    {
        if (args.interactorObject.transform.tag == "Right Hand")
        {
            RightHandModel.SetActive(false);
        }
        else if (args.interactorObject.transform.tag == "Left Hand")
        {
            LeftHandModel.SetActive(false);
        }
    }

    public void ShowGrabbingHandModel(SelectExitEventArgs args)
    {
        if (args.interactorObject.transform.tag == "Right Hand")
        {
            RightHandModel.SetActive(true);
        }
        else if (args.interactorObject.transform.tag == "Left Hand")
        {
            LeftHandModel.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
