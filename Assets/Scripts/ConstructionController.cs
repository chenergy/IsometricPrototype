using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class ConstructionController : MonoBehaviour
{
	public TileController tc;
	public GameObject selector;

	private GameObject objectToCreate;
	private Tile targetTile;
	private Building selectedBuilding;

	private List <Building> placedBuildings = new List<Building> ();


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


	void Start (){
		this.selector.SetActive (false);
	}


	void OnPointerDownEvent (PointerEventData p, A_TouchDrag td) {
		TouchDrag_Button tdb = (TouchDrag_Button)td;

		if (tdb != null) {
			// Instantiate object.
			this.objectToCreate = GameObject.Instantiate (tdb.prefabToCreate, Vector3.zero, Quaternion.identity) as GameObject;

			// Disable Collider.
			Collider c = this.objectToCreate.GetComponent <Collider> ();
			if (c != null)
				c.enabled = false;
		}
	}


	void OnPointerUpEvent (PointerEventData p, A_TouchDrag td) {
		if (this.RaycastHasHitTile (p)) {
			if (this.objectToCreate != null) {
				Building b = this.objectToCreate.GetComponent <Building>();

				if (b != null) {
					//Debug.Log (string.Format ("Tile: {0}", this.targetTile.name));

					if (this.CanBuildingBePlaced (b, this.targetTile)) {
						// Zero object on target tile.
						this.objectToCreate.transform.position = this.targetTile.transform.position;

						// Set tiles to in-use.
						this.tc.SetTileInUse (this.targetTile.location);

						// Set tiles that are used by building to building.
						List<Tile> buildingTiles = new List<Tile>();
						buildingTiles.Add (this.targetTile);

						foreach (IntVector2 v in b.localTiles){
							IntVector2 loc = this.targetTile.location + v;
							this.tc.SetTileInUse (loc);
							buildingTiles.Add (this.tc.GetTile (loc));
						}

						// Assign to the building.
						b.AssignUsedTiles (buildingTiles);

						// Enable Collider.
						Collider c = this.objectToCreate.GetComponent <Collider> ();
						if (c != null)
							c.enabled = true;

						this.placedBuildings.Add (b);
					} else {
						// If object was created, destroy it.
						if (this.objectToCreate != null)
							GameObject.Destroy (this.objectToCreate);
					}
				}
			}
		} else {
			Debug.Log ("Cannot find tile");

			// If object was created, destroy it.
			if (this.objectToCreate != null)
				GameObject.Destroy (this.objectToCreate);
		}

		this.tc.ClearTiles ();

		this.objectToCreate = null;
		this.targetTile = null;
	}


	void OnDragEvent (PointerEventData p, A_TouchDrag td){
		if (this.RaycastHasHitTile (p)) {
			if (this.objectToCreate != null) {
				this.objectToCreate.transform.position = this.targetTile.transform.position;

				this.tc.ClearTiles ();

				Building b = this.objectToCreate.GetComponent <Building>();
				if (b != null && this.targetTile != null)
					this.ShowUsableTiles (b, this.targetTile);
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
				Tile t = hit.collider.transform.GetComponent <Tile>();

				if (t != null) {
					this.targetTile = t;
					return true;
				}
			}
		}

		this.targetTile = null;

		return false;
	}


	bool CanBuildingBePlaced (Building b, Tile t){
		IntVector2 tileLocation = t.location;

		if (this.tc.IsTileInUse (t))
			return false;

		foreach (IntVector2 v in b.localTiles) {
			IntVector2 tileLoc = v + tileLocation;
			Tile tile = this.tc.GetTile (tileLoc);

			//Debug.Log (tileLoc.ToVector3().ToString());

			if (tile != null) {
				if (this.tc.IsTileInUse (tile))
					return false;
			} else
				return false;
		}
		
		return true;
	}


	void ShowUsableTiles (Building b, Tile t){
		IntVector2 tileLocation = t.location;

		foreach (IntVector2 v in b.localTiles) {
			IntVector2 tileLoc = v + tileLocation;
			Tile tile = this.tc.GetTile (tileLoc);

			if (tile != null)
				this.tc.SelectTile (tile);
		}

		foreach (Building build in this.placedBuildings) {
			foreach (Tile tile in build.usedTiles){
				if (tile != null)
					this.tc.SelectTile (tile);
			}
		}
	}



	public void SelectBuilding (Building b) {
		this.selectedBuilding = b;

		this.selector.transform.position = b.transform.position + Vector3.up * 1.5f;
		this.selector.SetActive (true);
	}


	public void DeselectBuilding () {
		this.selectedBuilding = null;

		this.selector.SetActive (false);
	}
}

