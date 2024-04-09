using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public string nickname;
    public int maxHealth = 100;
    private int currentHealth;
    public GameObject healthBar;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
    
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Player takes " + damage + " damage. Current health: " + currentHealth);
        ChangeHealthBar();
        if (currentHealth <= 0)
        {
            Die();
        }
    }


      public void ChangeHealthBar(){
        healthBar.GetComponent<UnityEngine.UI.Slider>().value=currentHealth;
    }

    void Die()
    {
        Debug.Log("Player dies!");
        // Here, handle the player's death (e.g., show a retry screen)
    }
}
