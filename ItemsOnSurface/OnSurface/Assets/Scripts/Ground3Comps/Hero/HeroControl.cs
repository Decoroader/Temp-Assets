using UnityEngine;

public class HeroControl : MonoBehaviour
{
    [SerializeField] private float speed;

    private Rigidbody heroRB;

    void Start()
    {
        speed = 5f;
        heroRB = GetComponent<Rigidbody>();
    }

    
    void FixedUpdate()
    {
        Vector3 tPosition = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);

        heroRB.MovePosition(transform.position + tPosition * Time.deltaTime * speed);
    }
}
