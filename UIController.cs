using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    // create global fields
    public GameObject player;
    public Text healthIndicator;
    public Text gameoverMsg;

    // Start is called before the first frame update
    void Start()
    {
        // get references
        //player = GameObject.FindGameObjectWithTag("Player");

        //Text[] textBoxes = GetComponentsInChildren<Text>();
        //healthIndicator = textBoxes[0];
        //gameoverMsg = textBoxes[1];
        gameoverMsg.text = "";

        // subscribe to event system
        PlayerController.takeDamage += UpdateHealth;
        PlayerController.takeDamage += CheckDead;
        PlayerController.gainHealth += UpdateHealth;

        // first call to UpdateHealth
        UpdateHealth(0);
    }

    void UpdateHealth(int healthAdjust) {
        int playerHealth = player.GetComponentInChildren<PlayerController>().health;
        healthIndicator.text = "Current Health: " + playerHealth;
    }

    void CheckDead(int healthAdjust) {
        if (healthAdjust < 0) {
            int playerHealth = player.GetComponentInChildren<PlayerController>().health;
            if (playerHealth <= 0) {
                gameoverMsg.text = "Game Over";
            }
        }
    }
}
