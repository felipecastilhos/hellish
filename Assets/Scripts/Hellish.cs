using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Hellish : MonoBehaviour
{
    [SerializeField] float velocity = 3f;
    [SerializeField] InputActionReference movement, jump, duck;
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
        Run();
    }

    void Run()
    {
        movementDirection = movement.action.ReadValue<Vector2>();
        if (movementDirection.sqrMagnitude > 0) myRigidBody.velocity = movementDirection * velocity;
        FlipSprite();
    }

    void FlipSprite()
    {
        float velocityX = myRigidBody.velocity.x;
        const float xScaleMultiplier = 5; //Idk why I need this to the sprite don't stretch
        if (Mathf.Abs(velocityX) > Mathf.Epsilon)
        {
            transform.localScale = new Vector3(Mathf.Sign(velocityX) * xScaleMultiplier, transform.localScale.y, transform.localScale.z);
        }
    }

}
