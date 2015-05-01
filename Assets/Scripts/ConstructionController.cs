using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ConstructionController : MonoBehaviour
{
	public TileController tc;

	private GameObject objectToCreate;
	private Tile targetTile;


	void OnEnable (){
		foreach (TouchDrag_Button td in FindObjectsOfType <TouchDrag_Button>()) {
			td.OnPointerDownEvent += this.OnPointerDownEvent;
			td.OnPointerUpEvent += this.OnPointerUpEvent;
			td.OnDragEvent += this.OnDragEvent;
		}
	}


	void OnDisable (){
		foreach (TouchDrag_Button td in FindObjectsOfType <TouchDrag_Button>()) {
			td.OnPointerDownEvent -= this.OnPointerDownEvent;
			td.OnPointerUpEvent -= this.OnPointerUpEvent;
			td.OnDragEvent -= this.OnDragEvent;
		}
	}


	void OnPointerDownEvent (PointerEventData p, A_TouchDrag td) {
		TouchDrag_Button tdb = (TouchDrag_Button)td;

		if (tdb != null) 
			this.objectToCreate = GameObject.Instantiate (tdb.prefabToCreate, Vector3.zero, Quaternion.identity) as GameObject;
	}


	void OnPointerUpEvent (PointerEventData p, A_TouchDrag td){
		if (this.RaycastHasHitTile (p)) {
			if (this.objectToCreate != null)
				this.objectToCreate.transform.position = this.targetTile.transform.position;
		} else {
			Debug.Log ("Cannot find tile");

			if (this.objectToCreate != null)
				GameObject.Destroy (this.objectToCreate);

			this.tc.ClearTiles ();
		}

		this.objectToCreate = null;
		this.targetTile = null;
	}


	void OnDragEvent (PointerEventData p, A_TouchDrag td){
		if (this.RaycastHasHitTile (p)) {
			if (this.objectToCreate != null) {
				this.objectToCreate.transform.position = this.targetTile.transform.position;
			}
		}
	}


	// Cast a ray to the world at the current pointer location.
	bool RaycastHasHitTile (PointerEventData p){
		// Convert pointer screen point to ray.
		Ray ray = Camera.main.ScreenPointToRay (p.position);
		RaycastHit hit;

		// Check raycast collision with a tile component.
		if (Physics.Raycast (ray, out hit, 100)) {
			if (hit.collider != null) {
				if (hit.collider.transform.parent != null) {
					Tile t = hit.collider.transform.parent.GetComponent <Tile>();

					if (t != null) {
						this.targetTile = t;
						return true;
					}
				}
			}
		}

		this.targetTile = null;

		return false;
	}
}

