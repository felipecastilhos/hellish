using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {

    [SerializeField] float explosionRadius = 3f;
    [SerializeField] Vector2 explosionForce = new(200f, 100f);

    Animator animator;

    // Start is called before the first frame update
    void Start() {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() { }

    void OnTriggerEnter2D(Collider2D other) {
        animator.SetTrigger("Burn");
    }

    void ExplodeBomb() {
        Collider2D playerCollider = Physics2D.OverlapCircle(
            transform.position,
            explosionRadius,
            LayerMask.GetMask("Player")
        );

        if (playerCollider) {
            playerCollider.GetComponent<Rigidbody2D>().AddForce(explosionForce);
            playerCollider.GetComponent<Hellish>().WasHit();
        }
    }

    //Used as event
    void DestroyBomb() {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected() =>
        Gizmos.DrawWireSphere(
            transform.position,
            explosionRadius
        );
}
