using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth;
    public float currentHealth;
    public int defense;
    public HealthBar healthBar;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        DamageNum.Create(transform.position, ((int)damage).ToString(), DamageNum.colors.orange);

        if (currentHealth <= 0 && maxHealth >= 0) 
        {
            Destroy(gameObject); // TODO: have option for respawning
            return;
        }

        healthBar.SetCurrentHealth(currentHealth);
    }

    public void Heal(float health) 
    {
        float amountHealed = health;

        if ((currentHealth + health) > maxHealth)
        {
            amountHealed = maxHealth - currentHealth;
            currentHealth = maxHealth;
        }
        else 
        {
            currentHealth += health;
        }

        DamageNum.Create(transform.position, ((int)amountHealed).ToString(), DamageNum.colors.green);

        healthBar.SetCurrentHealth(currentHealth);
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }
}
