using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class EventTriggerOverlay : MonoBehaviour
{
	public TouchDrag_Select touch;
	public CameraController cameraCtrl;
	public ConstructionController constructionCtrl;
	public NavigationController navigationCtrl;
	
	//private Ship ship;
	
	private bool hasBeenDragged = false;
	private bool foundBuilding = false;


	void OnEnable (){
		touch.OnPointerUpEvent += this.OnPointerUpEvent;
		touch.OnPointerDownEvent += this.OnPointerDownEvent;
		touch.OnDragEvent += this.OnDragEvent;
	}


	void OnDisable (){
		touch.OnPointerUpEvent -= this.OnPointerUpEvent;
		touch.OnPointerDownEvent -= this.OnPointerDownEvent;
		touch.OnDragEvent -= this.OnDragEvent;
	}
	
	
	/*public void Init (Ship ship){
		this.ship = ship;
	}*/


	void OnPointerDownEvent (PointerEventData p, A_TouchDrag source){
		this.hasBeenDragged = false;
		this.foundBuilding = false;
		
		// Convert pointer screen point to ray.
		Ray ray = Camera.main.ScreenPointToRay (p.position);
		RaycastHit hit;
		
		if (Physics.Raycast (ray, out hit, 1000)) {
			if (this.RaycastHitBuilding (hit)) {
				Building building = hit.collider.transform.GetComponent <Building>(); 
				this.constructionCtrl.EventTrigger_OnPointerDownEvent (p);
				this.foundBuilding = true;
			} else if (this.RaycastHitGround (hit)){
				if (this.cameraCtrl.Mode != ViewMode.DECK_VIEW)
					this.navigationCtrl.MoveShip (hit.point);
			}
		}
	}
	
	
	void OnPointerUpEvent (PointerEventData p, A_TouchDrag source){
		this.constructionCtrl.EventTrigger_OnPointerUpEvent (p, this.hasBeenDragged);
		
		this.hasBeenDragged = false;
		this.foundBuilding = false;
	}


	void OnDragEvent (PointerEventData p, A_TouchDrag source){
		this.hasBeenDragged = true;
		
		if (this.foundBuilding) {
			if (this.constructionCtrl.State == ConstructionState.PLACING) {
				this.constructionCtrl.EventTrigger_OnDragEvent (p);
			} else {
				this.cameraCtrl.Move (p);
			}
		} else {
			this.cameraCtrl.Move (p);
		}
	}


	// Check if building was hit by raycast.
	bool RaycastHitBuilding (RaycastHit hit){
		// Check raycast collision with a building component.
		if (hit.collider != null) {
			if (hit.collider.transform.GetComponent <Building>() != null) 
				return true;
		}
		return false;
	}
	
	
	// Check if the ground was hit by raycast.
	bool RaycastHitGround (RaycastHit hit){
		// Check raycast collision with a ground component.
		if (hit.collider != null) {
			if (hit.collider.transform.GetComponent <Ground>() != null) 
				return true;
		}
		return false;
	}
}

