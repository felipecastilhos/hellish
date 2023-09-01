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
    public PolygonCollider2D feetCollider2d;
    public CapsuleCollider2D capsuleCollider2D;

    private Vector2 movementDirection;

    private bool isJumping;

    void Start() {
        Debug.Log("Staring Hellish behaviour");
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        feetCollider2d = GetComponent<PolygonCollider2D>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }

    void Update() {
        Run();
        Jump();
    }

void Run() {
    movementDirection = movement.action.ReadValue<Vector2>().normalized;

    if (isJumping) {
        rigidBody.velocity = new Vector2(movementDirection.x  * velocity, rigidBody.velocity.y);
    } else {
        rigidBody.velocity = new Vector2(movementDirection.x * velocity, 0);
    }

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
        }

        JumpingAnimationState();
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
        float velocityY = rigidBody.velocity.y;
        Debug.Log($"[JumpingAnimationState] {velocityY}");
        isJumping = Mathf.Abs(velocityY) > Mathf.Epsilon;
        animator.SetBool(HellishAnimations.Jumping, isJumping);
    }
}

static class HellishAnimations {
    public static readonly string Running = "isRunning";
    public static readonly string Duck = "isDucking";
    public static readonly string Jumping = "isJumping";
}