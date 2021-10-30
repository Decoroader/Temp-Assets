using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBehav : MonoBehaviour
{
    //public GameObject ground;
    //public ItemSpawner spawner;

    private Rigidbody itemRb;
    //private Vector3 groundPos;

    void Start()
    {
        //groundPos = ground.transform.position;
        itemRb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision ground)
    {
        if (ground.gameObject.CompareTag("Ground"))
        {
            itemRb.isKinematic = true;
        }
    }
    //private void Update()
    //{
    //    if (groundPos.y - 1 > transform.position.y) { 
    //        spawner.CurrentItemsList.Remove(gameObject);
    //        spawner.SourceItemsList.Add(gameObject); 
    //    }
    //}
}
