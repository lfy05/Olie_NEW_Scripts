using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
    // public fields
    public int health = 100;

    // event system
    public delegate void CollideAction(int healthAdjust);
    public static event CollideAction takeDamage;
    public static event CollideAction gainHealth;

    void UpdateHealth(int healthAdjust) {
        health += healthAdjust;
    }

    private void OnCollisionEnter(Collision collision) {
        // get tag of the other object
        string tag = collision.gameObject.tag;

        // check that tag to evoke events
        switch (tag) {

            case "Health":
                UpdateHealth(collision.gameObject.GetComponent<CollectableController>().healthEffect);
                gainHealth(collision.gameObject.GetComponent<CollectableController>().healthEffect);
                break;

            case "Damage":
                UpdateHealth(collision.gameObject.GetComponent<CollectableController>().healthEffect);
                takeDamage(collision.gameObject.GetComponent<CollectableController>().healthEffect);
                break;

            default:
                break;
        }

    }
}
