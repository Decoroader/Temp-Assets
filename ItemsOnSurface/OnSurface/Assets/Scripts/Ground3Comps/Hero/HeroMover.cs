using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMover : MonoBehaviour
{
    private Rigidbody heroRB;
    [SerializeField] private float speed;
    void Start()
    {
        speed = .05f;
        heroRB = GetComponent<Rigidbody>();
        heroRB.AddForce(Vector3.forward * speed, ForceMode.Impulse);
    }

    void Update()
    {
        if (heroRB.velocity.x != 0)
            heroRB.velocity -= Vector3.right * heroRB.velocity.x;
        if (heroRB.velocity.z != speed)
            heroRB.AddForce(Vector3.forward * speed, ForceMode.Impulse);
        
        if (transform.eulerAngles != Vector3.zero)
            transform.eulerAngles = Vector3.zero;
    }
}
