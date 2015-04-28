using UnityEngine;
using System.Collections;

public class TileGenerator : MonoBehaviour
{
	public GameObject prefab;

	private Vector3[] tileLocations;

	// Use this for initialization
	void Start ()
	{
		this.tileLocations = new Vector3[] {
			new Vector3 (0,0,0),
			new Vector3 (1,0,0),
			new Vector3 (0,0,1),
			new Vector3 (1,0,1)
		};

		this.CreateTiles ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void CreateTiles (){
		foreach (Vector3 pos in this.tileLocations) {
			GameObject.Instantiate (prefab, pos, Quaternion.identity);
		}
	}
}

