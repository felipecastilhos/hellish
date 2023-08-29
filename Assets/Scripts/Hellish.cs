using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class Hellish : MonoBehaviour {
    [SerializeField] float velocity = 3f;
    [SerializeField] float jumpSpeed = 15F;
    [SerializeField] InputActionReference movement, jump, duck;
    [SerializeField] private Animator animator;

    public Rigidbody2D rigidBody;

    private Vector2 movementDirection;

    void Start() {
        Debug.Log("Staring Hellish behaviour");
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update() {
        Run();
        Jump();
    }

    void Run() {
        movementDirection = movement.action.ReadValue<Vector2>();

        if (movementDirection.sqrMagnitude > 0) {
            rigidBody.velocity = movementDirection * velocity;
        }
        FlipSprite();
        RunningAnimationState();
    }


    void Jump() {
        var isJumping = jump.action.WasPressedThisFrame();

        if (isJumping) {
            Debug.Log("Hellish is jumping");
            Vector2 jumpVelocity = new(rigidBody.velocity.x, jumpSpeed);
            rigidBody.velocity = jumpVelocity;
        }

        JumpingAnimationState();
    }

    private void FlipSprite() {
        float velocityX = rigidBody.velocity.x;
        const float xScaleMultiplier = 5; //Idk why I need this to the sprite don't stretch

        if (Mathf.Abs(velocityX) > Mathf.Epsilon) {
            transform.localScale = new Vector3(Mathf.Sign(velocityX) * xScaleMultiplier, transform.localScale.y, transform.localScale.z);
        }
    }

    private void RunningAnimationState() {
        float velocityX = rigidBody.velocity.x;
        bool isRunning = Mathf.Abs(velocityX) > Mathf.Epsilon;

        animator.SetBool(HellishAnimations.Running, isRunning);
    }

    private void JumpingAnimationState() {
        float velocityY = rigidBody.velocity.y;
        var isJumping = Mathf.Abs(velocityY) > Mathf.Epsilon;
        animator.SetBool(HellishAnimations.Jumping, isJumping);
    }


}

static class HellishAnimations {
    public static readonly string Running = "isRunning";
    public static readonly string Duck = "isDucking";
    public static readonly string Jumping = "isJumping";
}
