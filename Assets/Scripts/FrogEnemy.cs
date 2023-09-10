using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogEnemy : MonoBehaviour {
    [SerializeField] float enemyRunSpeed = 5f;

    Rigidbody2D enemyRigidbody;
    BoxCollider2D boxCollider2D;

    // Start is called before the first frame update
    void Start() {
        enemyRigidbody = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update() { }

    void FixedUpdate() {
        JumpAround();
    }

    private void JumpAround() {
        if (isFacingLeft()) {
            enemyRigidbody.velocity = new(-enemyRunSpeed, 0f);
        }
        else {
            enemyRigidbody.velocity = new(enemyRunSpeed, 0f);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        FlipSprite();
        RunBack();
    }


    private void RunBack() {
        enemyRigidbody.velocity = new(enemyRigidbody.velocity.x * -1, 0f);
    }

    private bool isFacingLeft() =>
         transform.localScale.x < 0;


    private void FlipSprite() {
        float velocityX = enemyRigidbody.velocity.x;

        float direction = Mathf.Sign(velocityX);

        transform.localScale = new Vector3(direction * -1, 1f, 1f);
    }
}
