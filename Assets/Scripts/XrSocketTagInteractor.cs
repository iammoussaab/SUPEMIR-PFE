using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class XrSocketTagInteractor : UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor
{

    public string Taginteractor;


    public override bool CanHover(UnityEngine.XR.Interaction.Toolkit.Interactables.IXRHoverInteractable interactable)
    {
        return base.CanHover(interactable) && interactable.transform.gameObject.tag == Taginteractor;
    }

    public override bool CanSelect(UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable interactable)
    {
        return base.CanSelect(interactable) && interactable.transform.gameObject.tag == Taginteractor;
    }

}
