using UnityEngine;

public class CameraToPlayer : MonoBehaviour
{
    public GameObject hero;
    private Vector3 offset;
    [SerializeField] private int cameraDistancing = 9;      // please edit this value for u needs

    void Start()
    {
        SetCamera();
    }

    void LateUpdate()
    {
        transform.position = hero.transform.position + offset;
    }

    void SetCamera()
    {
        transform.position = hero.transform.position + Vector3.back * cameraDistancing; // sets the camera 9 units behind the player, can be customized
        offset = transform.position - hero.transform.position;
    }
}
