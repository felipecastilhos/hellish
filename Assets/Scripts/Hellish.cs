using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class Hellish : MonoBehaviour {
    [SerializeField] float velocity = 3f;
    [SerializeField] float jumpSpeed = 15F;
    [SerializeField] InputActionReference movement, jump, duck, climb;
    [SerializeField] private Animator animator;

    public Rigidbody2D rigidBody;
    public PolygonCollider2D feetCollider2d;
    public CapsuleCollider2D capsuleCollider2D;

    private Vector2 movementDirection;

    private bool isJumping;
    float startingGravityScale;

    void Start() {
        Debug.Log("Starting Hellish behaviour");
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        feetCollider2d = GetComponent<PolygonCollider2D>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        startingGravityScale = rigidBody.gravityScale;
    }

    void Update() {
        Run();
        Jump();
        Climbing();
    }

    void Run() {
        movementDirection = movement.action.ReadValue<Vector2>().normalized;
        rigidBody.velocity = new Vector2(movementDirection.x * velocity, rigidBody.velocity.y);
        FlipSprite();
        RunningAnimationState();
    }

    void Jump() {
        var jumpButtonPressed = jump.action.WasPressedThisFrame();
        var canGetImpulse = feetCollider2d.IsTouchingLayers(LayerMask.GetMask("Ground"));

        Debug.Log($"canGetImpulse {canGetImpulse} ");

        if (jumpButtonPressed && canGetImpulse) {
            Debug.Log("Hellish is jumping");
            Vector2 jumpVelocity = new(rigidBody.velocity.x, jumpSpeed);
            rigidBody.velocity = jumpVelocity;
            isJumping = true;
        }

        if (canGetImpulse && rigidBody.velocity.y < Mathf.Epsilon) isJumping = false;

        JumpingAnimationState();
    }

    void Climbing() {
        var canClimb = capsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Climbing"));
        var climbButtonIsPressed = climb.action.IsPressed();
        movementDirection = movement.action.ReadValue<Vector2>();
        var isClimbing = canClimb & climbButtonIsPressed;

        if (isClimbing) {
            Debug.Log("She is not climbing");
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, movementDirection.y * velocity);
            rigidBody.gravityScale = 0;
        }
        else {
            rigidBody.gravityScale = startingGravityScale;
        }

        animator.SetBool(HellishAnimations.Climbing, isClimbing);
    }

    private void FlipSprite() {
        float velocityX = rigidBody.velocity.x;
        const float xScaleMultiplier = 3;

        if (Mathf.Abs(velocityX) > Mathf.Epsilon) {
            transform.localScale = new Vector3(Mathf.Sign(velocityX) * xScaleMultiplier, transform.localScale.y, transform.localScale.z);
        }
    }

    private void RunningAnimationState() {
        float velocityX = rigidBody.velocity.x;
        bool isRunning = Mathf.Abs(velocityX) > Mathf.Epsilon & !isJumping;

        animator.SetBool(HellishAnimations.Running, isRunning);
    }

    private void JumpingAnimationState() {
        animator.SetBool(HellishAnimations.Jumping, isJumping);
    }
}

static class HellishAnimations {
    public static readonly string Running = "isRunning";
    public static readonly string Duck = "isDucking";
    public static readonly string Jumping = "isJumping";
    public static readonly string Climbing = "isClimbing";
}