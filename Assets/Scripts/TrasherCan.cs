using UnityEngine;

public class TrasherCan : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<TriggerZone>().OnEnterEvent.AddListener(insidetrash);
    }

    public void insidetrash(GameObject go)
    {
        go.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {

    }
}
