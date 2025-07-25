using UnityEngine;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class SpaceOutsideController : MonoBehaviour
{
    public XRLever Lever;
    public XRKnob knob;

    public float ForwardSpeed;
    public float SideSpeed;

    public GameObject gameContent;         // Assign in Inspector
    public GameObject gameFinishedUI;      // Assign in Inspector
    public string mainMenuSceneName = "MainMenu";
    public float delayBeforeMainMenu = 30f;

    private bool hasFinished = false;

    void Update()
    {
        if (hasFinished) return; // Don't move after finish

        float forwardVelocity = ForwardSpeed * (Lever.value ? 1 : 0);
        float sideVelocity = SideSpeed * (Lever.value ? 1 : 0) * Mathf.Lerp(-1, 1, knob.value);

        Vector3 velocity = new Vector3(sideVelocity, 0, forwardVelocity);
        transform.position += velocity * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasFinished) return;

        // Check if player reached the goal
        if (other.CompareTag("FinishZone"))
        {
            FinishGame();
        }
    }

    void FinishGame()
    {
        hasFinished = true;

        if (gameContent != null)
            gameContent.SetActive(false);

        if (gameFinishedUI != null)
            gameFinishedUI.SetActive(true);

        Invoke(nameof(LoadMainMenu), delayBeforeMainMenu);
    }

    void LoadMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
    // public XRLever Lever;
    // public XRKnob knob;

    // public float ForwardSpeed;
    // public float SideSpeed;
    // // Start is called once before the first execution of Update after the MonoBehaviour is created
    // void Start()
    // {

    // }

    // // Update is called once per frame
    // void Update()
    // {

    //     float ForwardVelocity = ForwardSpeed * (Lever.value ? 1 : 0);
    //     float SideVelocity = SideSpeed * (Lever.value ? 1 : 0) * (Mathf.Lerp(-1,1,knob.value));

    //     Vector3 Velocity = new Vector3 (SideVelocity, 0, ForwardVelocity);
    //     transform.position += Velocity * Time.deltaTime;

}
// }
