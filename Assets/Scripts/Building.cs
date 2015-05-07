using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Building : MonoBehaviour
{
	public IntVector2[] localTiles;

	private Tile baseTile;
	public Tile BaseTile {
		get { return this.baseTile; }
	}

	/*private List<IntVector2> usedTiles = new List<IntVector2> ();
	public List<IntVector2> UsedTiles {
		get { return this.usedTiles; }
	}*/
	private IntVector2[] usedTileLocations;
	public IntVector2[] UsedTileLocations {
		get { return this.usedTileLocations; }
	}


	void Awake (){
		this.usedTileLocations = new IntVector2[this.localTiles.Length];

		for (int i = 0; i < this.localTiles.Length; i++) {
			this.usedTileLocations[i] = localTiles[i];
		}
	}

	// Use this for initialization
	/*void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}*/

	public void SetBaseTile (Tile t){
		this.baseTile = t;

		for (int i = 0; i < this.localTiles.Length; i++) {
			this.usedTileLocations[i] = t.location + localTiles[i];
		}
	}

	/*public void SetUsedTiles (List<Tile> tiles){
		this.usedTiles = tiles;
	}*/
}

