using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Building : MonoBehaviour
{
	public IntVector2[] localTiles;
	
	public List<Tile> usedTiles = new List<Tile> ();

	// Use this for initialization
	/*void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}*/

	public void AssignUsedTiles (List<Tile> tiles){
		this.usedTiles = tiles;
	}
}

