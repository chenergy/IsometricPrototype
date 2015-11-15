using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Building : MonoBehaviour
{
	public string buildingName = "";
	public IntVector2[] localTiles;

	private Tile baseTile;
	public Tile BaseTile {
		get { return this.baseTile; }
	}
	
	private Tile lastBaseTile = null;

	private IntVector2[] usedTileLocations;
	public IntVector2[] UsedTileLocations {
		get { return this.usedTileLocations; }
	}


	void Awake (){
		this.usedTileLocations = new IntVector2[this.localTiles.Length];

		for (int i = 0; i < this.localTiles.Length; i++) {
			this.usedTileLocations[i] = localTiles[i];
		}
		
		if (this.GetComponent <Collider> () != null)
			this.GetComponent <Collider> ().enabled = false;
	}


	public void SetBaseTile (Tile t){
		this.baseTile = t;

		this.transform.rotation = t.transform.rotation;
		this.transform.position = t.transform.position;

		for (int i = 0; i < this.localTiles.Length; i++) {
			this.usedTileLocations[i] = t.location + localTiles[i];
		}
	}
	
	
	public void LiftUp (){
		this.transform.parent = null;
	}
	

	public void PlaceDown (){
		this.lastBaseTile = this.baseTile;
		
		this.transform.parent = this.baseTile.transform;

		if (this.GetComponent <Collider> () != null)
			this.GetComponent <Collider> ().enabled = true;
	}


	public void Reset (){
		if (this.lastBaseTile != null) {
			this.transform.position = this.lastBaseTile.transform.position;
			this.transform.parent = this.lastBaseTile.transform;
			
			this.SetBaseTile (this.lastBaseTile);

			if (this.GetComponent <Collider> () != null)
				this.GetComponent <Collider> ().enabled = true;
		} else {
			GameObject.DestroyImmediate (this.gameObject);
		}
	}
}

