using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteriorItemsControl : MonoBehaviour
{
	public GameObject[] itemPrefabs;
	public List<GameObject> CurrentItems = new List<GameObject>();
	public List<GameObject> SourceItems = new List<GameObject>();

	[SerializeField] private int numer_sourceItems;
	[SerializeField] private int numer_currentItems;
	private float distanceCameraXBound;
	private float distanceCameraYBound; 
	private Camera cameraMain;
    private Vector3 cameraPos;
	private const float addHeight = .5f;
	private const float addWidth = .5f;
	private Vector3 itemSize;
	private float screenWidth;
	private float screenHeight;
	private bool goUp;
	

	void Start()
	{
		goUp = true	;
		numer_sourceItems = 20;
		numer_currentItems = 10;
		screenWidth = Screen.width;
		screenHeight = Screen.height;
		cameraMain = Camera.main;
		
		StartCoroutine(SpawnNPlaceItems());
	}

	void FillSourceItems()
	{
		for (int item = 0; item < numer_sourceItems; item++)
		{
			int itemIndex = Random.Range(0, itemPrefabs.Length - 1);
			GameObject tempItem = Instantiate(itemPrefabs[itemIndex], cameraPos - Vector3.back * 11, Random.rotation);
			SourceItems.Add(tempItem);
		}
	}

	void FirstItemsGeneration()
    {
		for (int ic = 0; ic < numer_currentItems; ic++)
		{
			GameObject tempItem = SourceItems[Random.Range(0, SourceItems.Count - 1)];
			itemSize = tempItem.GetComponent<MeshRenderer>().bounds.size;				// do inc this to the each generator?
			if (goUp)
				FirstSetRandPos_Screen(tempItem, itemSize.z / 2);
			else
				SetRandYPos_Screen(tempItem, itemSize.z / 2);

			CurrentItems.Add(tempItem);
			SourceItems.Remove(tempItem);
		}
	}

	void SetRandXPos_Screen(GameObject tItem, float itemZPos)
    {
		float tX = XYGenerator(cameraPos.x - distanceCameraXBound, cameraPos.x + distanceCameraXBound);
		tItem.transform.position = new Vector3(tX, cameraPos.y + distanceCameraYBound + addHeight, -itemZPos);  
	}
	void SetRandYPos_Screen(GameObject tItem, float itemZPos)
    {
		float tY = XYGenerator(cameraPos.y - distanceCameraYBound, cameraPos.y + distanceCameraYBound);
		tItem.transform.position = new Vector3(cameraPos.x + distanceCameraXBound + addWidth, tY, -itemZPos); 
	}
	void SetRandXPos_Bg(GameObject tItem, float itemZPos)
	{
		float tX = XYGenerator(cameraPos.x - distanceCameraXBound * 2, cameraPos.x + distanceCameraXBound * 2);
		tItem.transform.position = new Vector3(tX, cameraPos.y + distanceCameraYBound + addHeight, -itemZPos);
	}
	void SetRandYPos_Bg(GameObject tItem, float itemZPos)
	{
		float tY = XYGenerator(cameraPos.y - distanceCameraYBound * 2, cameraPos.y + distanceCameraYBound * 2);
		tItem.transform.position = new Vector3(cameraPos.x + distanceCameraXBound + addWidth, tY, -itemZPos);
	}
	void FirstSetRandPos_Screen(GameObject tItem, float itemZPos)
	{
		float tY = XYGenerator(cameraPos.y - distanceCameraYBound, cameraPos.y + distanceCameraYBound);
		float tX = XYGenerator(cameraPos.x - distanceCameraXBound, cameraPos.x + distanceCameraXBound);
		tItem.transform.position = new Vector3(tX, tY, -itemZPos);
	}
	float XYGenerator(float minR, float maxR)
    {
		return Random.Range(minR, maxR);
	}

	IEnumerator SpawnNPlaceItems()
    {
		yield return new WaitForEndOfFrame();
		cameraPos = cameraMain.transform.position;

		Vector3 worldBasePosition = cameraMain.ScreenToWorldPoint(new Vector3(screenWidth, screenHeight, cameraPos.z));
		distanceCameraXBound = cameraPos.x - worldBasePosition.x;
		distanceCameraYBound = cameraPos.y - worldBasePosition.y;

		FillSourceItems();
		FirstItemsGeneration();
	}
}
