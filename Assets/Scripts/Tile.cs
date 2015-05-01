using UnityEngine;
using System.Collections;


public class Tile : MonoBehaviour {
	public Renderer tileRenderer;
	public Material selectedMat;
	public Material unselectedMat;


	public void SetSelected (){
		this.tileRenderer.material = selectedMat;
	}


	public void SetUnselected (){
		this.tileRenderer.material = unselectedMat;
	}
}
