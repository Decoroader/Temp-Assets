using UnityEngine;


public class SetHeroOnStart : MonoBehaviour
{
    public GameObject background;
    [SerializeField] private bool isUpMove;
    private bool testIsUpMove;                  // for testing
    private Vector3 bgPosition;
    [SerializeField] private float coordinateCorrector;


    private void Awake()
    {
        isUpMove = true;
        testIsUpMove = true;                     // for testing

        HeroStartSetter();
    }

    void HeroStartSetter() {
        bgPosition = background.transform.position;
        Vector3 bgSize = background.GetComponent<SpriteRenderer>().bounds.size;

        transform.position = isUpMove ? SetForUpMove(bgSize.y) : SetForSideMove(bgSize.x);
    }

    Vector3 SetForUpMove(float bgY) { return new Vector3(bgPosition.x, (-bgY / 4) + coordinateCorrector, bgPosition.z - 1); }

    Vector3 SetForSideMove(float bgX) { return new Vector3((-bgX / 4) + coordinateCorrector, bgPosition.y, bgPosition.z - 1); }

    void Update()                               // for testing
    {
        if(testIsUpMove != isUpMove)
        {
            testIsUpMove = isUpMove;
            HeroStartSetter();
        }
    }
}
