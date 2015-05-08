using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class ConstructionController : MonoBehaviour
{
	private enum ConstructionState {
		SELECTING, PLACING, EDITING
	}

	public TileController tc;
	public GameObject selector;
	public BuildingMenu buildingMenu;
	public TouchDrag_Button[] constructionButtons;

	private ConstructionState state = ConstructionState.SELECTING;
	private Building selectedBuilding;
	private List <Building> placedBuildings = new List<Building> ();




	void OnEnable (){
		foreach (TouchDrag_Button td in this.constructionButtons) {
			td.OnPointerDownEvent += this.OnPointerDownEvent;
			td.OnPointerUpEvent += this.OnPointerUpEvent;
			td.OnDragEvent += this.OnDragEvent;
		}
	}


	void OnDisable (){
		foreach (TouchDrag_Button td in this.constructionButtons) {
			td.OnPointerDownEvent -= this.OnPointerDownEvent;
			td.OnPointerUpEvent -= this.OnPointerUpEvent;
			td.OnDragEvent -= this.OnDragEvent;
		}
	}


	void Start (){
		this.selector.SetActive (false);
		this.buildingMenu.Close ();
	}


	void OnPointerDownEvent (PointerEventData p, A_TouchDrag td) {
		TouchDrag_Button tdb = (TouchDrag_Button)td;

		if (tdb != null) {
			// Instantiate object.
			GameObject objectToCreate = GameObject.Instantiate (tdb.prefabToCreate, Vector3.zero, Quaternion.identity) as GameObject;

			// Select new building and set base tile to (0, 0).
			Building b = objectToCreate.GetComponent <Building> ();
			if (b != null) {
				this.SelectBuilding (b);
				b.SetBaseTile (this.tc.GetTile (new IntVector2 (0, 0)));
				b.StartMove ();
				this.buildingMenu.Open (b);
			}

			// Disable Collider.
			/*Collider c = objectToCreate.GetComponent <Collider> ();
			if (c != null)
				c.enabled = false;*/

			// Disable all further construction, except for current button.
			foreach (A_TouchDrag button in this.constructionButtons){
				if (button != td)
					button.eventTrigger.enabled = false;
			}

			// Show which tiles are being used.
			this.ShowUsedTiles();

			// Set state to placing.
			this.state = ConstructionState.PLACING;
		}
	}


	void OnPointerUpEvent (PointerEventData p, A_TouchDrag td) {
		// Disable touch drag on pointer up.
		TouchDrag_Button tdb = (TouchDrag_Button)td;
		tdb.eventTrigger.enabled = false;
	}


	void OnDragEvent (PointerEventData p, A_TouchDrag td){
		Tile targetTile = this.RaycastHitTile (p);

		// Building is being selected, and it has a target tile.
		if (targetTile != null && this.selectedBuilding != null) {
			// Only move if base tiles of building has changed.
			if (targetTile != this.selectedBuilding.BaseTile) {
				// Hide current building tiles.
				this.HideBuildingUsedTiles (this.selectedBuilding);

				// Move building to the tile location.
				this.selectedBuilding.transform.position = targetTile.transform.position;

				// Update base tile.
				this.selectedBuilding.SetBaseTile (targetTile);
			
				// Show selected tiles.
				this.ShowUsedTiles ();
			}
		}
	}


	public void OnClickPlaceEvent () {
		if (this.CanBuildingBePlaced (this.selectedBuilding)) {
			// Hide all used tiles.
			this.HideUsedTiles ();

			// Re-enable touchdrag buttons.
			foreach (TouchDrag_Button tdb in this.constructionButtons) {
				tdb.eventTrigger.enabled = true;
			}

			// Add the placed building to the list if necessary.
			if (!this.placedBuildings.Contains (this.selectedBuilding)) {
				this.placedBuildings.Add (this.selectedBuilding);
				foreach (IntVector2 v in this.selectedBuilding.UsedTileLocations){
					this.tc.SetTileInUse (v);
				}
			}

			// Save before reseting.
			Building temp = this.selectedBuilding;

			// End click event.
			this.EndClickEvent ();

			// Reset object.
			temp.PlaceDown ();
		} else {
			Debug.Log ("cannot place building in location");
		}
	}
	
	
	public void OnClickCancelEvent (){
		// Hide all used tiles.
		this.HideUsedTiles ();

		// Re-enable touchdrag buttons.
		foreach (TouchDrag_Button tdb in this.constructionButtons) {
			tdb.eventTrigger.enabled = true;
		}

		// Save before reseting.
		Building temp = this.selectedBuilding;

		// End click event.
		this.EndClickEvent ();

		// Reset object.
		temp.Reset ();
	}


	void EndClickEvent (){
		// Nullify selected building.
		this.DeselectBuilding ();
		
		// Turn off building menu.
		this.buildingMenu.Close ();
		
		// Set state to selecting.
		this.state = ConstructionState.SELECTING;
	}


	public void OnClickRemoveEvent (){

	}


	public void OnBuildingSelectEvent (Building b){
		if (this.state == ConstructionState.SELECTING) {
			this.SelectBuilding (b);
		}
	}


	public void OnBuildingDeselectEvent (){
		if (this.state == ConstructionState.SELECTING) {
			this.DeselectBuilding ();
		}
	}


	public void SelectBuilding (Building b) {
		this.selectedBuilding = b;
		this.buildingMenu.Open (b);
		this.ShowUsedTiles ();
		this.BindSelector (b);
	}
	
	
	
	public void DeselectBuilding () {
		this.selectedBuilding = null;
		this.buildingMenu.Close ();
		this.HideUsedTiles ();
		this.ReleaseSelector ();
	}


	// Cast a ray to the world at the current pointer location.
	Tile RaycastHitTile (PointerEventData p){
		// Convert pointer screen point to ray.
		Ray ray = Camera.main.ScreenPointToRay (p.position);
		RaycastHit hit;

		// Check raycast collision with a tile component.
		if (Physics.Raycast (ray, out hit, 100)) {
			if (hit.collider != null) {
				Tile t = hit.collider.transform.GetComponent <Tile>();

				if (t != null) {
					return t;
				}
			}
		}

		return null;
	}
	

	bool CanBuildingBePlaced (Building b){
		foreach (IntVector2 v in b.UsedTileLocations) {
			Tile t = this.tc.GetTile (v);

			if (t != null) {
				if (this.tc.IsTileInUse (t))
					return false;
			} else {
				return false;
			}
		}
		
		return true;
	}


	void ShowUsedTiles (){
		if (this.selectedBuilding != null) 
			this.ShowBuildingUsedTiles (this.selectedBuilding);
		
		
		foreach (Building build in this.placedBuildings) {
			this.ShowBuildingUsedTiles (build);
		}
	}


	void HideUsedTiles (){
		if (this.selectedBuilding != null) 
			this.HideBuildingUsedTiles (this.selectedBuilding);
		
		
		foreach (Building build in this.placedBuildings) {
			this.HideBuildingUsedTiles (build);
		}
	}


	void ShowBuildingUsedTiles (Building b){
		foreach (IntVector2 v in b.UsedTileLocations) {
			Tile t = this.tc.GetTile (v);
			
			if (t != null)
				this.tc.SelectTile (t);
		}
	}


	void HideBuildingUsedTiles (Building b) {
		foreach (IntVector2 v in b.UsedTileLocations) {
			Tile t = this.tc.GetTile (v);
			
			if (t != null)
				this.tc.DeselectTile (t);
		}
	}

	
	void BindSelector (Building b){
		this.selector.transform.parent = b.transform;
		this.selector.transform.localPosition = Vector3.up * 1.5f;
		this.selector.SetActive (true);
	}
	
	
	void ReleaseSelector (){
		this.selector.transform.parent = this.transform;
		this.selector.SetActive (false);
	}
}

