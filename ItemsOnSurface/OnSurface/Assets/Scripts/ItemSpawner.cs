using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
	public GameObject[] itemPrefabs;
	public List<GameObject> CurrentItemsList = new List<GameObject>();
	public List<GameObject> SourceItemsList = new List<GameObject>();

	[SerializeField] private int numer_sourceItems;
	[SerializeField] private int numer_currentItems;
	private float distanceCameraXBound;
	private float distanceCameraZBound;
	private Camera cameraMain;
	private Vector3 cameraPos;
	private const float addHeight = 2f;
	private const float addWidth = 2f;
	private float screenWidth;
	private float screenHeight;


	void Start()
	{
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
			GameObject tempItem = Instantiate(itemPrefabs[itemIndex], cameraPos - Vector3.back * 21, Random.rotation);
			SourceItemsList.Add(tempItem);
		}
	}

	void FirstItemsGeneration()
	{
		for (int ic = 0; ic < numer_currentItems; ic++)
		{
			GameObject tempItem = SourceItemsList[Random.Range(0, SourceItemsList.Count - 1)];
			FirstSetRandPos_Screen(tempItem);
			

			CurrentItemsList.Add(tempItem);
			SourceItemsList.Remove(tempItem);
		}
	}

	void FirstSetRandPos_Screen(GameObject tItem)
	{
		float tX = XYGenerator(cameraPos.x - distanceCameraXBound + addWidth, cameraPos.x + distanceCameraXBound - addWidth);
		float tZ = XYGenerator(cameraPos.z - distanceCameraZBound + addHeight, cameraPos.z + distanceCameraZBound - addHeight);
		//Debug.Log("tX " + tX + " | tZ " + tZ);
		tItem.transform.position = new Vector3(tX, 5, tZ);
	}
	float XYGenerator(float minR, float maxR)
	{
		return Random.Range(minR, maxR);
	}

	IEnumerator SpawnNPlaceItems()
	{
		yield return new WaitForEndOfFrame();
		cameraPos = cameraMain.transform.position;

		Vector3 worldBasePosition = cameraMain.ScreenToWorldPoint(new Vector3(screenWidth, screenHeight, cameraPos.y));
		distanceCameraXBound = Mathf.Abs(cameraPos.x - worldBasePosition.x);
		distanceCameraZBound = Mathf.Abs(cameraPos.z - worldBasePosition.z);

		FillSourceItems();
		FirstItemsGeneration();
	}

}
