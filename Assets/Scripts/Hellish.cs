using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class Hellish : MonoBehaviour {
    [SerializeField] float velocity = 3f;
    [SerializeField] float jumpSpeed = 15F;
    [SerializeField] Vector2 knockBackForce = new(1f, 6f);
    [SerializeField] float stunTime = 1f;
    [SerializeField] InputActionReference movement, jump, duck, climb;
    [SerializeField] private Animator animator;

    public Rigidbody2D rigidBody;
    public BoxCollider2D feetCollider;
    public CapsuleCollider2D capsuleCollider;

    private Vector2 movementDirection;

    private bool isJumping;
    private bool isStunned;
    float startingGravityScale;

    void Start() {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        feetCollider = GetComponent<BoxCollider2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        startingGravityScale = rigidBody.gravityScale;
    }

    void Update() {
        if (!isStunned) {
            Run();
            Jump();
            Climbing();
            WasHit();
        }
    }

    void Run() {
        movementDirection = movement.action.ReadValue<Vector2>().normalized;
        rigidBody.velocity = new Vector2(movementDirection.x * velocity, rigidBody.velocity.y);
        FlipSprite();
        RunningAnimationState();
    }

    void Jump() {
        var jumpButtonPressed = jump.action.WasPressedThisFrame();
        var canGetImpulse = feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));


        if (jumpButtonPressed && canGetImpulse) {
            Vector2 jumpVelocity = new(rigidBody.velocity.x, jumpSpeed);
            rigidBody.velocity = jumpVelocity;
            isJumping = true;
        }

        if (canGetImpulse && rigidBody.velocity.y < Mathf.Epsilon) isJumping = false;

        JumpingAnimationState();
    }

    void Climbing() {
        var canClimb = capsuleCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"));
        var climbButtonIsPressed = climb.action.IsPressed();
        movementDirection = movement.action.ReadValue<Vector2>();
        var isClimbing = canClimb & climbButtonIsPressed;

        if (isClimbing) {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, movementDirection.y * velocity);
            rigidBody.gravityScale = 0;
        }
        else {
            rigidBody.gravityScale = startingGravityScale;
        }

        animator.SetBool(HellishAnimations.Climbing, isClimbing);

    }

    private void WasHit() {
        var wasHit = capsuleCollider.IsTouchingLayers(LayerMask.GetMask("Enemy"));
        if (wasHit) {
            rigidBody.velocity = knockBackForce * new Vector2(-transform.localScale.x, 1f);
            HitAnimationState();
            StartCoroutine(StunHer());
        }
        else {
            HitAnimationState(false);
        }
    }

    IEnumerator StunHer() { 
        isStunned = true;
        yield return new WaitForSeconds(stunTime);
        isStunned = false;
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

    private void HitAnimationState(bool isHit = true) {
        animator.SetBool(HellishAnimations.IsHit, isHit);
    }
}

static class HellishAnimations {
    public static readonly string Running = "isRunning";
    public static readonly string Duck = "isDucking";
    public static readonly string Jumping = "isJumping";
    public static readonly string Climbing = "isClimbing";
    public static readonly string IsHit = "isHit";

}