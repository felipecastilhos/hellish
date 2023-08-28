using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Hellish : MonoBehaviour
{
    [SerializeField] float velocity = 3f;
    [SerializeField] private InputActionReference movement, jump, duck;
    [SerializeField] private Animator animator;

    public Rigidbody2D myRigidBody;

    private Vector2 movementDirection;

    void Start()
    {
        Debug.Log("Staring Hellish behaviour");
        myRigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movementDirection = movement.action.ReadValue<Vector2>();
        if (movementDirection.sqrMagnitude > 0) myRigidBody.velocity = movementDirection * velocity;
    }


    private void Movement(InputAction.CallbackContext obj)
    {
    }
}
