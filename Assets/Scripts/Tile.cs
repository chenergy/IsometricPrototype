using UnityEngine;
using System.Collections;


public class Tile : MonoBehaviour {
	public Renderer tileRenderer;
	public Material selectedMat;
	public Material unselectedMat;
	public IntVector2 location;

	private bool inUse = false;
	public bool InUse {
		get { return this.inUse; }
	}


	public void SetSelected (){
		this.tileRenderer.material = selectedMat;
	}


	public void SetUnselected (){
		this.tileRenderer.material = unselectedMat;
	}


	public void SetInUse (bool inUse){
		this.inUse = inUse;
	}
}
