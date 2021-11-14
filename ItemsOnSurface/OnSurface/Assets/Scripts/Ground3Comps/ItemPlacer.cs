using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPlacer : MonoBehaviour
{
    public List<GameObject> ListObjectsForPlace;
    public List<GameObject> ListPlacedObjects;

    public GameObject[] jamPrefabs;
    public GameObject[] obstaclePrefabs;

    public GameObject hero;
    public GameObject surface;

    [SerializeField] private int numberJam;
    [SerializeField] private float maxObjectSize;

    private List<Renderer> SurfaceChildrenRendererList = new List<Renderer>();
    private Bounds boundsOverallSurface;
    private GameObject[] itemPrefabs;

    //#if UNITY_EDITOR
    [SerializeField] private bool objectReplacing;
    private bool tempSwitcher;
    //#endif
    void Start()
    {
        numberJam = 9;
        maxObjectSize = 2;
        objectReplacing = tempSwitcher = true;
        itemPrefabs = obstaclePrefabs;
        GetSurfaceDataObjectsForPlace();
        StartCoroutine(DelayNPlace(numberJam));
    }
#if UNITY_EDITOR
    private void Update()
    {
        if (objectReplacing != tempSwitcher)
        {
            tempSwitcher = objectReplacing;
            int tsh = 0;
            foreach (GameObject curObj in ListObjectsForPlace)
            {
                curObj.transform.position = Vector3.up * (15 + tsh);
                tsh += 2;
            }
            ListPlacedObjects.Clear();
            StartCoroutine(DelayNPlace(numberJam));
        }
    }
#endif
    IEnumerator DelayNPlace(int numbObj)
    {
        yield return null;
        yield return null;
        if (numbObj > ListObjectsForPlace.Count)
            numbObj = ListObjectsForPlace.Count;
        if (numbObj < 0)
            numbObj = 0;

        float discretZ = boundsOverallSurface.size.z / (numbObj);
        Vector3 tPos;
        RaycastHit hit;
        int it = 0;
        while (it < numbObj)
        {
            GameObject currObj = ListObjectsForPlace[it];
            if (currObj == null) {
                Debug.LogError("One of the objects to be placed from the List is null, placement was interrupted.");
                break;
            }
            float sizeZObj = currObj.GetComponent<Renderer>().bounds.size.z;
            float tX = Random.Range(boundsOverallSurface.min.x, boundsOverallSurface.max.x);
            float tZ = Random.Range(discretZ * (it + .5f) - sizeZObj, discretZ * (it + .5f) + sizeZObj);
            tPos = new Vector3(tX, boundsOverallSurface.max.y + maxObjectSize, boundsOverallSurface.min.z + tZ);

            if (Physics.Raycast(tPos, Vector3.down, out hit))
            {
                SetToSurface(currObj, hit.point);
                currObj.GetComponent<Rigidbody>().isKinematic = true;
                if (IsPlacingCorrectness(currObj, ListPlacedObjects, SurfaceChildrenRendererList))
                    it++;
            }
        }
    }

    void GetSurfaceDataObjectsForPlace()
    {
        SurfaceChildrenRendererList = GetChildrenRendererList(surface);
        boundsOverallSurface = GetAllBounds(surface);
        ListObjectsForPlace = GetListOfObjectForPlace(itemPrefabs);
    }
    Bounds GetAllBounds(GameObject parentObj)
    {
        Bounds tempBounds = new Bounds();       // is zero dimensions
        foreach (Renderer currChildRend in SurfaceChildrenRendererList)
            tempBounds.Encapsulate(currChildRend.bounds);
        return tempBounds;
    }

    // TODO 

    void SetToSurface(GameObject objForPlace, Vector3 positionForPlaceObject)
    {
        Bounds currObjBounds = objForPlace.GetComponent<Renderer>().bounds;

        positionForPlaceObject += Vector3.up * (currObjBounds.size.y / 2);      // the profile.y of the object is taken into account in brackets
        objForPlace.transform.position = positionForPlaceObject;
    }
   
    bool IsPlacingCorrectness(GameObject currObj, List<GameObject> ListIntersectionWithThese, List<Renderer> ListSurfacePartsRenderer)
    {
        Bounds currObjBounds = currObj.GetComponent<Renderer>().bounds;
        if (currObjBounds.Intersects(hero.GetComponent<Renderer>().bounds))
            return false;

        foreach (GameObject withThis in ListIntersectionWithThese)
            if (currObjBounds.Intersects(withThis.GetComponent<Renderer>().bounds))
                return false;

        foreach (Renderer withSurfPart in ListSurfacePartsRenderer) {
            Bounds surfPartBounds = withSurfPart.GetComponent<Renderer>().bounds;
            if (currObjBounds.Intersects(surfPartBounds))
                if (!((currObjBounds.min.x >= surfPartBounds.min.x) && (currObjBounds.min.z >= surfPartBounds.min.z) &&
                    (currObjBounds.max.x <= surfPartBounds.max.x) && (currObjBounds.max.z <= surfPartBounds.max.z)))
                    return false;
        }

        ListIntersectionWithThese.Add(currObj);
        return true;
    }

    List<Renderer> GetChildrenRendererList(GameObject parentObj)
    {
        List<Renderer> TempRendererList = new List<Renderer>();
        foreach (Transform child in parentObj.transform)
            TempRendererList.Add(child.GetComponent<Renderer>());
        return TempRendererList;
    }
    List<GameObject> GetListOfObjectForPlace(GameObject[] A_itemPrefabs)
    {
        List<GameObject> tempObjectList = new List<GameObject>();

        for (int it = 0; it < A_itemPrefabs.Length; it++)
        {
            A_itemPrefabs[it].GetComponent<Rigidbody>().isKinematic = true;
            tempObjectList.Add(Instantiate(A_itemPrefabs[it], Vector3.up * (15 + it * 2), A_itemPrefabs[it].transform.rotation));
        }
        return tempObjectList;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(boundsOverallSurface.center, boundsOverallSurface.size);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(boundsOverallSurface.center, 0.1f);
    }
}