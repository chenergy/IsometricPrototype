﻿using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Collider))]
public class Tile : MonoBehaviour {
	public Renderer tileRenderer;
	public Material selectedMat;
	public Material unselectedMat;
	public IntVector2 location;

	private bool inUse = false;
	public bool InUse {
		get { return this.inUse; }
	}


	public void SetSelected (bool selected){
		if (selected)
			this.tileRenderer.material = selectedMat;
		else
			this.tileRenderer.material = unselectedMat;
	}


	/*public void SetUnselected (){
		this.tileRenderer.material = unselectedMat;
	}*/


	public void SetInUse (bool inUse){
		this.inUse = inUse;
	}
}
