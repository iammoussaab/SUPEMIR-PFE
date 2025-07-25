using UnityEngine;

public class GhostAttack : MonoBehaviour
{
    public int damageAmount = 20;

    void Start()
    {
        // Check if this object has a trigger collider
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            if (!col.isTrigger)
            {
                Debug.LogWarning("GhostAttack: Collider is not set as trigger! Setting it now...");
                col.isTrigger = true;
            }
            Debug.Log("GhostAttack: Collider found and is trigger: " + col.isTrigger);
            Debug.Log("GhostAttack: Collider size: " + col.bounds.size);
            Debug.Log("GhostAttack: Collider center: " + col.bounds.center);
        }
        else
        {
            Debug.LogError("GhostAttack: No Collider component found on this GameObject!");
        }

        // Check for player in scene
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Debug.Log("GhostAttack: Found player: " + player.name);
            Collider playerCol = player.GetComponent<Collider>();
            if (playerCol != null)
            {
                Debug.Log("GhostAttack: Player has collider: " + playerCol.name);
                Debug.Log("GhostAttack: Player collider bounds: " + playerCol.bounds);
            }
            else
            {
                Debug.LogWarning("GhostAttack: Player has no collider!");
            }
        }
        else
        {
            Debug.LogError("GhostAttack: No player found in scene!");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("GhostAttack: Trigger entered by: " + other.name + " with tag: " + other.tag);

        if (other.CompareTag("Player"))
        {
            Debug.Log("GhostAttack: Player detected! Looking for PlayerHealth component...");
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            if (health != null)
            {
                Debug.Log("GhostAttack: PlayerHealth found! Dealing damage: " + damageAmount);
                health.TakeDamage(damageAmount);
            }
            else
            {
                Debug.LogWarning("GhostAttack: PlayerHealth component not found on player!");
            }
        }
    }

    // Test method - call this from the inspector or another script to test damage
    [ContextMenu("Test Damage")]
    public void TestDamage()
    {
        Debug.Log("GhostAttack: Testing damage...");
        // Find the player in the scene
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            PlayerHealth health = player.GetComponent<PlayerHealth>();
            if (health != null)
            {
                Debug.Log("GhostAttack: Test - Dealing damage: " + damageAmount);
                health.TakeDamage(damageAmount);
            }
            else
            {
                Debug.LogError("GhostAttack: Test - PlayerHealth component not found on player!");
            }
        }
        else
        {
            Debug.LogError("GhostAttack: Test - No GameObject with 'Player' tag found in scene!");
        }
    }

    // Add this to see if the ghost is moving or if there are any physics issues
    void Update()
    {
        // Log position every few seconds to see if ghost is moving
        if (Time.frameCount % 300 == 0) // Every 5 seconds at 60fps
        {
            Debug.Log("GhostAttack: Current position: " + transform.position);
        }
    }
}
