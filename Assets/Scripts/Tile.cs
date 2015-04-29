using UnityEngine;
using System.Collections;


public class Tile : MonoBehaviour {
	public RaycastReceiver receiver;
	public Renderer tileRenderer;
	public Material selectedMat;
	public Material unselectedMat;

	private TileGenerator generator;


	void OnEnable (){
		if (receiver != null) {
			receiver.OnRaycastUp += this.OnRaycastUp;
			receiver.OnRaycastDrag += this.OnRaycastDrag;
		}
	}


	void OnDisable (){
		if (receiver != null) {
			receiver.OnRaycastUp -= this.OnRaycastUp;
			receiver.OnRaycastDrag += this.OnRaycastDrag;
		}
	}


	// Use this for initialization
	void Start () {
		this.generator = FindObjectOfType <TileGenerator> ();
	}
	
	// Update is called once per frame
	/*void Update () {
	
	}*/

	void OnRaycastUp (GameObject source){
		Debug.Log ("Selected " + this.transform.position.ToString ());

		TouchDrag td = source.GetComponent <TouchDrag> ();

		// Selected from TouchDrag.
		if (td != null)
			GameObject.Instantiate (td.prefabToCreate, this.transform.position, Quaternion.identity);
	}


	void OnRaycastDrag (GameObject source){
		TouchDrag td = source.GetComponent <TouchDrag> ();

		// Selected from TouchListener.
		/*if (td != null)
			this.tileRenderer.material = selectedMat;
		else
			this.tileRenderer.material = unselectedMat;*/
		if (td != null)
			this.generator.SelectTile (this);
	}


	public void SetSelected (){
		this.tileRenderer.material = selectedMat;
	}


	public void SetUnselected (){
		this.tileRenderer.material = unselectedMat;
	}
}
