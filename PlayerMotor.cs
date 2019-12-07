using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    // motor swtich
    [HideInInspector] public bool isDead = false;

    // player transform
    Transform playerTfm;

    // animation handling
    Animator playerAnmtr;

    // other parameters
    public float Speed;
    public float RunningSpeed;

    // Start is called before the first frame update
    void Start(){
        // get references
        playerTfm = GetComponent<Transform>();
        playerAnmtr = GetComponent<Animator>();

        // subscribe to event
        PlayerController.takeDamage += CheckDead;
    }

    // Update is called once per frame
    void Update()
    {
        // get input from axis
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // produce new Vector3
        Vector3 direction = new Vector3(horizontalInput, 0, verticalInput);

        // check if direction is (0, 0, 0)
        if (direction != new Vector3(0, 0, 0) && isDead == false) {
            
            if (Input.GetKey("left shift")){
                /* player is running */
                UpdateAnimator(false, false, true, false);

                // update transform
                playerTfm.position += direction * (RunningSpeed / 10f);

            } else {
                /* player is just walking */
                // update animation
                UpdateAnimator(false, true, false, false);

                // update transform
                playerTfm.position += direction * (Speed / 10f);       // 100f is a speed adjustment factor. Use for a more reasonable editor input value.
            }
            
        } else if (isDead == false) {
            // direction factor is 0. Stop animation and go back to idle
            UpdateAnimator(true, false, false, false);
        }
    }

   void CheckDead(int healthAdjust) {
        if (GetComponentInChildren<PlayerController>().health <= 0) {
            UpdateAnimator(false, false, false, true);
            isDead = true;
        }
    }

    void UpdateAnimator(bool idle, bool walking, bool running, bool dead) {
        playerAnmtr.SetBool("isIdle", idle);
        playerAnmtr.SetBool("isWalking", walking);
        playerAnmtr.SetBool("isRunning", running);
        playerAnmtr.SetBool("isDead", dead);
        
    }
}
