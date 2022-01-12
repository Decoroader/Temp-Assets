using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject hero;

    private Vector3 currentHeroPosition;
    private Vector3 prevHeroPosition;
    private float shiftX;
    private float shiftZ;

    void Start()
    {
        currentHeroPosition = hero.transform.position;
        prevHeroPosition = currentHeroPosition;
    }

    void Update()
    {
        currentHeroPosition = hero.transform.position;
        shiftX = currentHeroPosition.x - prevHeroPosition.x;
        shiftZ = currentHeroPosition.z - prevHeroPosition.z;
        prevHeroPosition = currentHeroPosition;

        transform.position = transform.position + Vector3.right * shiftX + Vector3.forward * shiftZ;
    }
}
