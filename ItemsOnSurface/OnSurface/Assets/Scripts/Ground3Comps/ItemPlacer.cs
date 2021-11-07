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

    private GameObject tempItem;

    private List<Renderer> SurfaceChildrenRendererList = new List<Renderer>();
    private Bounds boundsOverallSurface;


    void Start()
    {

        SurfaceChildrenRendererList = GetChildrenRendererList(surface);
        boundsOverallSurface = GetAllBounds(surface);

        ListObjectsForPlace = GetListOfObjectForPlace(itemPrefabs);
        PlacerFirst(5);
    }

    Bounds GetAllBounds(GameObject parentObj)
    {
        Bounds tempBounds = new Bounds();       // is zero dimensions
        foreach (Renderer currChildRend in SurfaceChildrenRendererList)
            tempBounds.Encapsulate(currChildRend.bounds);
        return tempBounds;
    }
    
    // TODO 
    
    
    void PlacerFirst(int numbObj)
    {
        Vector3 tempPos;
        while (numbObj > 0)
        {
            tempPos = GetCoordinate(boundsOverallSurface);
            Renderer surfaceObjMesh = CoordonateCorrectnessDetector(tempPos);

            if (surfaceObjMesh)
            {
                SetToSurface(ListObjectsForPlace[numbObj - 1], tempPos, surfaceObjMesh);
                if (!DoesIntersectsBoundsTwoObj(ListObjectsForPlace[numbObj - 1], ListPlacedObjects))
                    numbObj--;
            }
        }
    }
    void SetToSurface(GameObject objForPlace, Vector3 positionForPlaceObject, Renderer surfaceMesh)
    {
        Bounds currObjBounds = objForPlace.GetComponent<Renderer>().bounds;
        float tY = surfaceMesh.bounds.max.y + (currObjBounds.center.y - currObjBounds.min.y);       // the profile of the object is taken into account in brackets
        positionForPlaceObject += Vector3.up * tY;
        objForPlace.transform.position = positionForPlaceObject;
    }

    bool DoesIntersectsBoundsTwoObj(GameObject currObj, List<GameObject> IntersectionWithThese)
    {
        Bounds currObjBounds = currObj.GetComponent<Renderer>().bounds;
        foreach (GameObject withThis in IntersectionWithThese)
            if (currObjBounds.Intersects(withThis.GetComponent<Renderer>().bounds))
                return true;
        IntersectionWithThese.Add(currObj);
        return false;
    }
    Vector3 GetCoordinate(Bounds bounds)
    {
        float tX = Random.Range(bounds.min.x, bounds.max.x);
        float tZ = Random.Range(bounds.min.z, bounds.max.z);
        return new Vector3(tX, bounds.center.y, tZ);
    }
    Renderer CoordonateCorrectnessDetector(Vector3 currPos)
    {
        foreach (var currMesh in SurfaceChildrenRendererList)
        {
            if (currMesh.bounds.Contains(currPos))
                return currMesh;
        }
        return null;
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
            tempObjectList.Add(Instantiate(A_itemPrefabs[it], Vector3.up * (15 + it*5), Quaternion.identity));
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