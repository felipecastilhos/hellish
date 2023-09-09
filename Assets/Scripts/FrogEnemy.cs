using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogEnemy : MonoBehaviour
{
    [SerializeField] float enemyRunSpeed = 5f;

    Rigidbody2D enemyRigibody;
    // Start is called before the first frame update
    void Start()
    {
        enemyRigibody = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Frog enemy is moving");
        enemyRigibody.velocity = new(enemyRunSpeed, 0f);   
    }
}
