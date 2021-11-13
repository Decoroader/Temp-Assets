using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPlacer : MonoBehaviour
{
    public List<GameObject> ListObjectsForPlace;
    public List<GameObject> ListPlacedObjects;

    public GameObject[] itemPrefabs;
    
    public GameObject hero;
    public GameObject surface;

    [SerializeField]private int numberJam;
    [SerializeField] private float maxObjectSize;

    private List<Renderer> SurfaceChildrenRendererList = new List<Renderer>();
    private Bounds boundsOverallSurface;
//#if UNITY_EDITOR
    [SerializeField] private bool objectReplacing;
    private bool tempSwitcher;
//#endif
    void Start()
    {
        numberJam = 9;
        maxObjectSize = 3;  
        objectReplacing = tempSwitcher = true;

        GetSurfaceDataObjectsForPlace();

        PlacerFirst(numberJam);
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

            PlacerFirst(numberJam);
        }
    }
#endif
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
    // maxObjectSize should be castomized?


    void PlacerFirst(int numbObj)
    {
        while (numbObj > 0)
        {
            RaycastHit hit;

            if (Physics.Raycast(GetCoordinate(boundsOverallSurface), Vector3.down, out hit))
            {
                if (!hit.collider.CompareTag("ObjectForSet"))
                {
                    GameObject currObj = ListObjectsForPlace[numbObj - 1];
                    SetToSurface(currObj, hit.point);
                    currObj.GetComponent<Rigidbody>().isKinematic = true;
                    if (!DoesIntersectsBoundsTwoObj(currObj, ListPlacedObjects))
                        numbObj--;
                    else
                        currObj.transform.position += Vector3.up * 11;
                }
            }
        }
    }
    void SetToSurface(GameObject objForPlace, Vector3 positionForPlaceObject)
    {
        Bounds currObjBounds = objForPlace.GetComponent<Renderer>().bounds;

        positionForPlaceObject += Vector3.up * (currObjBounds.size.y / 2);      // the profile.y of the object is taken into account in brackets
        objForPlace.transform.position = positionForPlaceObject;
    }

    bool DoesIntersectsBoundsTwoObj(GameObject currObj, List<GameObject> ListIntersectionWithThese)
    {
        Bounds currObjBounds = currObj.GetComponent<Renderer>().bounds;
        foreach (GameObject withThis in ListIntersectionWithThese)
            if (currObjBounds.Intersects(withThis.GetComponent<Renderer>().bounds))
                return true;

        ListIntersectionWithThese.Add(currObj);
        return false;
    }
    Vector3 GetCoordinate(Bounds bounds)
    {
        float tX = Random.Range(bounds.min.x, bounds.max.x);
        float tZ = Random.Range(bounds.min.z, bounds.max.z);
        return new Vector3(tX, bounds.max.y + maxObjectSize, tZ);
    }
    
    List<Renderer> GetChildrenRendererList(GameObject parentObj)
    {
        List<Renderer> TempRendererList = new List<Renderer>();
        foreach (Transform child in parentObj.transform)
            TempRendererList.Add(child.GetComponent<Renderer>());
        return TempRendererList;
    }
    List<GameObject> GetListOfObjectForPlace(GameObject[] A_itemPrefabs)
    // this method should be another, depends from rules
    {
        List<GameObject> tempObjectList = new List<GameObject>();

        for (int it = 0; it < A_itemPrefabs.Length; it++)
            tempObjectList.Add(Instantiate(A_itemPrefabs[it], Vector3.up * (15 + it*2), Quaternion.identity));
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