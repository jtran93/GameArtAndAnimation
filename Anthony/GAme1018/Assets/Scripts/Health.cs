using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Health : MonoBehaviour {

    public Sprite[] HeartSprites;

    int currentHealth;

    public Image HeartUI;
    private Player player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
        currentHealth = player.playerStats.health;
        HeartUI.sprite = HeartSprites[0];
        Debug.Log("current health is: " + currentHealth);
        
        
    }

}
