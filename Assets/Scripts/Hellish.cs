using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hellish : MonoBehaviour
{

    readonly float horizontalMovement = 5f;
    readonly float verticalMovement = 5f;

    public Rigidbody2D myRigidBody;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Staring Hellish behaviour");
    }

    // Update is called once per frame
    void Update()
    {
        MovementDetection();
    }

    void MovementDetection()
    {
        if (Input.anyKey)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            Vector2 movementDirection = new Vector2(horizontalInput, verticalInput).normalized;

            if (movementDirection.sqrMagnitude > 0) myRigidBody.velocity = movementDirection * Mathf.Max(Mathf.Abs(horizontalMovement), Mathf.Abs(verticalMovement));
        }
    }
}
