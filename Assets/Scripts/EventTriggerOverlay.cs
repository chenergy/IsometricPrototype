using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class EventTriggerOverlay : MonoBehaviour
{
	public TouchDrag_Select touch;
	public CameraController cameraCtrl;
	public ConstructionController constructionCtrl;

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


	void OnPointerDownEvent (PointerEventData p, A_TouchDrag source){
		this.hasBeenDragged = false;
		this.foundBuilding = false;
		
		if (this.RaycastHitBuilding (p) != null) { 
			this.constructionCtrl.EventTrigger_OnPointerDownEvent (p);
			this.foundBuilding = true;
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


	// Cast a ray to the world at the current pointer location.
	Building RaycastHitBuilding (PointerEventData p){
		// Convert pointer screen point to ray.
		Ray ray = Camera.main.ScreenPointToRay (p.position);
		RaycastHit hit;
		
		// Check raycast collision with a tile component.
		if (Physics.Raycast (ray, out hit, 100)) {
			if (hit.collider != null) {
				Building building = hit.collider.transform.GetComponent <Building>();
				
				if (building != null) 
					return building;
			}
		}
		return null;
	}
}

