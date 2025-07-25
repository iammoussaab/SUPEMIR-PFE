using UnityEngine;
using UnityEngine.Playables;

public class PlaySteps : MonoBehaviour
{
    [System.Serializable]
    public class Step
    {
        public string name;
        public float time;
        public bool hasPlayed;
    }

    public Step[] steps; // <-- Declare the array of steps in the inspector

    private PlayableDirector director;

    void Start()
    {
        director = GetComponent<PlayableDirector>();
    }

    public void PlayStepIndex(int index)
    {
        Step step = steps[index];
        if (!step.hasPlayed)
        {
            step.hasPlayed = true;
            director.Stop(); // Capitalized
            director.time = step.time;
            director.Play();
        }
    }
}
