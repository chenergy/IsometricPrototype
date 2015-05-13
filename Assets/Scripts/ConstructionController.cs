using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public enum ConstructionState {
	SELECTING, PLACING, EDITING
}

public class ConstructionController : MonoBehaviour
{
	public TileController tc;
	public GameObject selector;
	public BuildingMenu buildingMenu;
	public TouchDrag_Button[] constructionButtons;

	private ConstructionState state = ConstructionState.SELECTING;
	public ConstructionState State {
		get { return this.state; }
	}
	
	private Building selectedBuilding;
	private List <Building> placedBuildings = new List<Building> ();


	void OnEnable (){
		foreach (TouchDrag_Button td in this.constructionButtons) {
			td.OnPointerDownEvent += this.TouchDrag_OnPointerDownEvent;
			td.OnPointerUpEvent += this.TouchDrag_OnPointerUpEvent;
			td.OnDragEvent += this.TouchDrag_OnDragEvent;
		}
	}


	void OnDisable (){
		foreach (TouchDrag_Button td in this.constructionButtons) {
			td.OnPointerDownEvent -= this.TouchDrag_OnPointerDownEvent;
			td.OnPointerUpEvent -= this.TouchDrag_OnPointerUpEvent;
			td.OnDragEvent -= this.TouchDrag_OnDragEvent;
		}
	}


	void Start (){
		this.selector.SetActive (false);
		this.buildingMenu.Close ();
	}


	void TouchDrag_OnPointerDownEvent (PointerEventData p, A_TouchDrag td) {
		if (this.state == ConstructionState.SELECTING) {
			// Disable all further construction, except for current button.
			foreach (A_TouchDrag button in this.constructionButtons){
				if (button != td)
					button.eventTrigger.enabled = false;
			}
			
			// Create the building passed by the touch drag button.
			TouchDrag_Button tdb = (TouchDrag_Button)td;
			if (tdb != null) {
				this.CreateBuilding (tdb.prefabToCreate);
			}
		}
	}
	
	
	void TouchDrag_OnDragEvent (PointerEventData p, A_TouchDrag td){
		if (this.state == ConstructionState.PLACING) {
			Tile targetTile = this.RaycastHitTile (p);

			// Building is being selected, and it has a target tile.
			if (targetTile != null && this.selectedBuilding != null) {
				// Only move if base tiles of building has changed.
				if (targetTile != this.selectedBuilding.BaseTile) {
					// Drag the selected building to the tile.
					this.MoveBuilding (this.selectedBuilding, targetTile);
				}
			}
		}
	}
	
	
	void TouchDrag_OnPointerUpEvent (PointerEventData p, A_TouchDrag td) {
		if (this.state == ConstructionState.PLACING) {
			// Disable touch drag on pointer up.
			TouchDrag_Button tdb = (TouchDrag_Button)td;
			tdb.eventTrigger.enabled = false;
			
			// Drop the building.
			this.DropBuilding (this.selectedBuilding);
		}
	}
	
	
	public void EventTrigger_OnPointerDownEvent (PointerEventData p) {
		// In placement mode, the selected building needs to be dragged. 
		if (this.state == ConstructionState.PLACING) {
			if (this.selectedBuilding != null) {
				this.DisableBuildingCollider (this.selectedBuilding);
			}
		}
	}
	
	
	public void EventTrigger_OnDragEvent (PointerEventData p){
		// In placement mode, drag the building being placed.
		if (this.state == ConstructionState.PLACING) {
			Tile targetTile = this.RaycastHitTile (p);

			// Building is being selected, and it has a target tile.
			if (targetTile != null && this.selectedBuilding != null) {
				// Only move if base tiles of building has changed.
				if (targetTile != this.selectedBuilding.BaseTile) {
					// Drag the selected building to the tile.
					this.MoveBuilding (this.selectedBuilding, targetTile);
				}
			}
		}
	}
	
	
	public void EventTrigger_OnPointerUpEvent (PointerEventData p, bool hasBeenDragged){
		// In placement mode.
		if (this.state == ConstructionState.PLACING) {
			if (hasBeenDragged) {
				// The selected building needs a collider to be selected again. 
				this.EnableBuildingCollider (this.selectedBuilding);
			}
		} 
		// In selecting mode.
		else if (this.state == ConstructionState.SELECTING) {
			if (!hasBeenDragged) {
				// Select a target building or deselect it.
				Building b = this.RaycastHitBuilding (p);
				if (b != null) 
					this.SelectBuilding (b);
				else 
					this.DeselectBuilding ();
			}
		}
		
		/*
		if (hasBeenDragged) {
			// In placement mode, the selected building needs a collider to be selected again. 
			if (this.state == ConstructionState.PLACING) {
				this.EnableBuildingCollider (this.selectedBuilding);
			}
		} else {
			// In selection mode, select a target building or deselect it.
			if (this.state == ConstructionState.SELECTING) {
				Building b = this.RaycastHitBuilding (p);
				if (b != null) 
					this.SelectBuilding (b);
				else 
					this.DeselectBuilding ();
			}
		}
		*/
	}
	
	
	public void Placing_OnClickPlaceEvent () {
		if (this.state == ConstructionState.PLACING) {
			this.PlaceBuilding (this.selectedBuilding);
		}
	}
	
	
	public void Placing_OnClickCancelEvent (){
		if (this.state == ConstructionState.PLACING){
			// Re-enable touchdrag buttons.
			foreach (TouchDrag_Button tdb in this.constructionButtons) {
				tdb.eventTrigger.enabled = true;
			}
			
			this.ResetBuilding ();
		}
	}

	
	public void Selecting_OnClickMoveEvent () {
		if (this.state == ConstructionState.SELECTING) {
			this.LiftBuilding (this.selectedBuilding);
		}
	}
	
	
	public void Selecting_OnClickCancelEvent () {
		//this.state = ConstructionState.SELECTING;
		this.HideAllUsedTiles ();
		
		// Nullify selected building.
		this.DeselectBuilding ();
		
		//this.buildingMenu.Show (this.selectedBuilding, this.state);
		this.buildingMenu.Close ();
	}
	
	
	void CreateBuilding (GameObject prefab) {
		// Instantiate object.
		GameObject objectToCreate = GameObject.Instantiate (prefab, Vector3.zero, Quaternion.identity) as GameObject;
		
		// Create a new building from touchdrag data.
		Building b = objectToCreate.GetComponent <Building> ();
		if (b != null) {
			// Set state to placing.
			this.state = ConstructionState.PLACING;
			
			// Select new building and set base tile to (0, 0).
			this.SelectBuilding (b);
			b.SetBaseTile (this.tc.GetTile (new IntVector2 (0, 0)));
			
			// Show which tiles are being used.
			this.ShowAllUsedTiles();
			
			// Open menu for the building.
			this.buildingMenu.Show (b, this.state);
		}
	}
	
	
	void LiftBuilding (Building b){
		this.state = ConstructionState.PLACING;
		
		// Disable all further construction.
		foreach (A_TouchDrag button in this.constructionButtons){
			button.eventTrigger.enabled = false;
		}
		
		// Set the given building's used tiles as not used.
		foreach (IntVector2 v in b.UsedTileLocations){
			this.tc.SetTileInUse (v, false);
		}
		
		// Show used tiles.
		this.ShowAllUsedTiles ();
		
		// Show the placing building menu.
		this.buildingMenu.Show (b, this.state);
	}
	
	
	
	void SelectBuilding (Building b) {
		this.selectedBuilding = b;
		this.buildingMenu.Show (b, this.state);
		this.BindSelector (b);
	}
	
	
	void DeselectBuilding () {
		this.selectedBuilding = null;
		this.buildingMenu.Close ();
		this.HideAllUsedTiles ();
		this.ReleaseSelector ();
	}
	
	
	void MoveBuilding (Building b, Tile t){
		// Hide current building tiles.
		this.HideBuildingUsedTiles (b);

		// Move building to the tile location.
		b.transform.position = t.transform.position;

		// Update base tile.
		b.SetBaseTile (t);
	
		// Show selected tiles.
		this.ShowAllUsedTiles ();
	}
	
	
	void DropBuilding (Building b) {
		// Enable the building's collider so that it can be selected again.
		if (b != null) {
			Collider c = b.GetComponent <Collider> ();
			if (c != null)
				c.enabled = true;
		}
	}
	
	
	void PlaceBuilding (Building b){
		if (this.CanBuildingBePlaced (b)) {
			// Hide all used tiles.
			this.HideAllUsedTiles ();

			// Re-enable touchdrag buttons.
			foreach (TouchDrag_Button tdb in this.constructionButtons) {
				tdb.eventTrigger.enabled = true;
			}

			// Add the placed building to the list if necessary.
			if (!this.placedBuildings.Contains (b)) {
				this.placedBuildings.Add (b);
			}
			
			// Set tiles as in-use.
			foreach (IntVector2 v in b.UsedTileLocations){
				this.tc.SetTileInUse (v, true);
			}
			
			// Set state to selecting.
			this.state = ConstructionState.SELECTING;

			// Reset object.
			//temp.PlaceDown ();
			this.selectedBuilding.PlaceDown ();
			
			// Turn off building menu.
			this.buildingMenu.Show (b, this.state);
		} else {
			Debug.Log ("cannot place building in location");
		}
	}
	
	
	void ResetBuilding (){
		// Reset the selector parent.
		this.ReleaseSelector ();
		
		// Set state to selecting.
		this.state = ConstructionState.SELECTING;

		// Hide all used tiles.
		this.HideAllUsedTiles ();
		
		// Reset object.
		this.selectedBuilding.Reset ();
			
		// Turn off building menu. Close menu if object has been deleted.
		if (this.selectedBuilding != null) {
			this.buildingMenu.Show (this.selectedBuilding, this.state);
			this.BindSelector (this.selectedBuilding);
		} else {
			this.buildingMenu.Close ();
		}
	}
	
	
	void EnableBuildingCollider (Building b) {
		// Enable the building's collider so that raycast can find it.
		if (b != null) {
			Collider c = b.GetComponent <Collider> ();
			if (c != null)
				c.enabled = true;
		}
	}
	
	
	void DisableBuildingCollider (Building b) {
		// Disable the building's collider in order to get tile collision beneath.
		Collider c = b.GetComponent <Collider> ();
		if (c != null)
			c.enabled = false;
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


	void ShowAllUsedTiles (){
		if (this.selectedBuilding != null) 
			this.ShowBuildingUsedTiles (this.selectedBuilding);
		
		
		foreach (Building build in this.placedBuildings) {
			this.ShowBuildingUsedTiles (build);
		}
	}


	void HideAllUsedTiles (){
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
	
	
	// Cast a ray to the world at the current pointer location.
	Building RaycastHitBuilding (PointerEventData p){
		// Convert pointer screen point to ray.
		Ray ray = Camera.main.ScreenPointToRay (p.position);
		RaycastHit hit;
		
		// Check raycast collision with a tile component.
		if (Physics.Raycast (ray, out hit, 100)) {
			if (hit.collider != null) {
				Building building = hit.collider.transform.GetComponent <Building>();
				
				if (building != null) {
					return building;
				}
			}
		}
		return null;
	}
}

