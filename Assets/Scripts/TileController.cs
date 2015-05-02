using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class TileController : MonoBehaviour
{
	public GameObject prefab;
	public int xLength = 0;
	public int zLength = 0;

	private List<IntVector2> tileLocations;
	Dictionary <IntVector2, Tile> locToTile = new Dictionary<IntVector2, Tile> ();


	// Use this for initialization
	void Start ()
	{
		this.tileLocations = new List<IntVector2> ();

		for (int i = 0; i < this.xLength; i++) {
			for (int j = 0; j < this.zLength; j++) {
				this.tileLocations.Add (new IntVector2 (i, j));
			}
		}

		this.CreateTiles ();
	}


	private void CreateTiles (){
		foreach (IntVector2 ipos in this.tileLocations) {
			Vector3 pos = ipos.ToVector3 ();
			GameObject newGobj = GameObject.Instantiate (prefab, pos, Quaternion.identity) as GameObject;
			Tile t = newGobj.GetComponent <Tile> ();
			t.transform.parent = this.transform;
			t.name = ipos.ToString ();
			t.location = ipos;
			this.locToTile.Add (ipos, t);
		}
	}


	/*public void SelectTile (Tile tile){
		tile.SetSelected ();
	}*/


	public void ClearTiles (){
		foreach (Tile t in this.locToTile.Values) {
			t.SetUnselected ();
		}
	}


	//public void SelectTiles (
}

