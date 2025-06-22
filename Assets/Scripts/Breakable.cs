using UnityEngine;
using System.Collections.Generic;

public class Breakable : MonoBehaviour
{

    public List<GameObject> breakableItems; // List of breakable objects


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (var item in breakableItems)
        {
            item.SetActive(false); // Deactivate all breakable items at the start
        }
        
    }

    public void Break()
    {
        foreach(var item in breakableItems)
        {
            item.SetActive(true); // Activate all breakable items when broken
            item.transform.parent = null; // Remove parent to allow free movement
        }
        Debug.Log("Breakable items activated");
        gameObject.SetActive(false); // Optionally deactivate the original object
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
