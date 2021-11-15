using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPlacer : MonoBehaviour
{
    public GameObject[] jamPrefabs;
    public GameObject[] obstaclePrefabs;

    public GameObject surface;

    [SerializeField] private int numberObj;
    [SerializeField] private float maxObjectSize;

    private List<GameObject> ListObjectsForPlace;
    private List<Renderer> SurfaceChildrenRendererList = new List<Renderer>();
    private Bounds boundsSurface;
    private GameObject[] itemPrefabs;

    void Start()
    {
        maxObjectSize = 2;
        itemPrefabs = obstaclePrefabs;

        SurfaceChildrenRendererList = GetChildrenRendererList(surface);
        ListObjectsForPlace = GetListOfObjectForPlace(itemPrefabs);
        boundsSurface = GetAllBounds(surface);
        numberObj = ListObjectsForPlace.Count;

        Placement(numberObj);
    }
    void Placement(int numbObj)
    {
        float discretZ = boundsSurface.size.z / (numbObj);
        Vector3 tPos;
        int it = 0;
        while (it < numbObj)
        {
            GameObject currObj = ListObjectsForPlace[it];
            
            float sizeZObj = currObj.GetComponent<Renderer>().bounds.size.z;
            float tX = Random.Range(boundsSurface.min.x, boundsSurface.max.x);
            float tZ = Random.Range(discretZ * (it + .5f) - sizeZObj, discretZ * (it + .5f) + sizeZObj);
            tPos = new Vector3(tX, boundsSurface.max.y + maxObjectSize, boundsSurface.min.z + tZ);

            if (Physics.Raycast(tPos, Vector3.down, out RaycastHit hit))
            {
                SetToSurface(currObj, hit.point);
                if (IsObjInBoundsOfSurface(currObj, SurfaceChildrenRendererList))
                    it++;
            }
        }
    }

    void SetToSurface(GameObject objForPlace, Vector3 positionForPlaceObject)
    {
        Bounds currObjBounds = objForPlace.GetComponent<Renderer>().bounds;

        positionForPlaceObject += Vector3.up * (currObjBounds.size.y / 2);      // the profile.y of the object is taken into account in brackets
        objForPlace.transform.position = positionForPlaceObject;
    }
   
    bool IsObjInBoundsOfSurface(GameObject currObj, List<Renderer> ListSurfacePartsRenderer)
    {
        Bounds currObjBounds = currObj.GetComponent<Renderer>().bounds;
        
        foreach (Renderer withSurfPart in ListSurfacePartsRenderer) {
            Bounds surfPartBounds = withSurfPart.GetComponent<Renderer>().bounds;
            if (currObjBounds.Intersects(surfPartBounds))
                if (!((currObjBounds.min.x >= surfPartBounds.min.x) && (currObjBounds.min.z >= surfPartBounds.min.z) &&
                      (currObjBounds.max.x <= surfPartBounds.max.x) && (currObjBounds.max.z <= surfPartBounds.max.z)))
                    return false;
        }

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
            tempObjectList.Add(Instantiate(A_itemPrefabs[it], Vector3.up * (15 + it * 2), A_itemPrefabs[it].transform.rotation));
        return tempObjectList;
    }
    Bounds GetAllBounds(GameObject parentObj)
    {
        Bounds tempBounds = new Bounds();
        foreach (Renderer currChildRend in SurfaceChildrenRendererList)
            tempBounds.Encapsulate(currChildRend.bounds);
        return tempBounds;
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(boundsSurface.center, boundsSurface.size);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(boundsSurface.center, 0.1f);
    }
}