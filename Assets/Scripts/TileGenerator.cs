using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileGenerator : MonoBehaviour
{
	public GameObject prefab;
	public int xLength = 0;
	public int zLength = 0;

	private List<Vector3> tileLocations;
	private List<Tile> tiles;


	void OnEnable (){
		foreach (TouchDrag td in FindObjectsOfType <TouchDrag>()) {
			td.OnSelectNoneEvent += this.ClearTiles;
		}
	}


	void OnDisable (){
		foreach (TouchDrag td in FindObjectsOfType <TouchDrag>()) {
			td.OnSelectNoneEvent -= this.ClearTiles;
		}
	}


	// Use this for initialization
	void Start ()
	{
		this.tileLocations = new List<Vector3> ();
		this.tiles = new List<Tile> ();

		for (int i = 0; i < this.xLength; i++) {
			for (int j = 0; j < this.zLength; j++) {
				this.tileLocations.Add (new Vector3 (i, 0, j));
			}
		}

		this.CreateTiles ();
	}
	
	// Update is called once per frame
	/*void Update ()
	{
	
	}*/

	private void CreateTiles (){
		foreach (Vector3 pos in this.tileLocations) {
			GameObject newGobj = GameObject.Instantiate (prefab, pos, Quaternion.identity) as GameObject;
			newGobj.transform.parent = this.transform;
			newGobj.name = newGobj.transform.position.ToString();
			this.tiles.Add (newGobj.GetComponent <Tile>());
		}
	}


	public void SelectTile (Tile tile){
		this.ClearTiles ();

		tile.SetSelected ();
	}


	public void ClearTiles (){
		foreach (Tile t in this.tiles) {
			t.SetUnselected ();
		}
	}
}
