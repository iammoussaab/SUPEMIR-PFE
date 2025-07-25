using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Death Settings")]
    public GameObject deathEffect; // Optional: screen fade or animation
    // public AudioClip deathSound;
    // private AudioSource audioSource;
    public bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        Debug.Log("PlayerHealth: Initialized with " + currentHealth + "/" + maxHealth + " health");

        // Check if this object has a collider
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            Debug.Log("PlayerHealth: Player has collider: " + col.name + " | IsTrigger: " + col.isTrigger);
        }
        else
        {
            Debug.LogWarning("PlayerHealth: Player has no collider component!");
        }

        // audioSource = GetComponent<AudioSource>();
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        Debug.Log("Player took damage: " + amount + " | Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (isDead) return;

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        Debug.Log("Player healed: " + amount + " | Current health: " + currentHealth);
    }

    void Die()
    {
        isDead = true;
        Debug.Log("Player has died.");

        // // Play sound
        // if (audioSource && deathSound)
        //     audioSource.PlayOneShot(deathSound);

        // Trigger death effect (fade to black, camera fall, etc.)
        if (deathEffect)
            deathEffect.SetActive(true);

        // Optional: disable movement, show Game Over screen, reload scene, etc.
    }

    // Test method - call this from the inspector to test damage
    [ContextMenu("Test Take Damage")]
    public void TestTakeDamage()
    {
        Debug.Log("PlayerHealth: Testing damage...");
        TakeDamage(10);
    }
}
