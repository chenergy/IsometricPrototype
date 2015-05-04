using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class EventTriggerOverlay : MonoBehaviour
{
	public TouchDrag_Select touch;
	public CameraController cameraCtrl;
	public ConstructionController constructionCtrl;

	private bool hasBeenDragged = false;
	private Building targetBuilding;


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


	void OnPointerUpEvent (PointerEventData p, A_TouchDrag source){
		if (!this.hasBeenDragged) {
			if (this.RaycastHasHitBuilding (p)) {
				this.constructionCtrl.SelectBuilding (this.targetBuilding);
			} else {
				this.constructionCtrl.DeselectBuilding ();
			}
		}

		this.hasBeenDragged = false;
	}


	void OnPointerDownEvent (PointerEventData p, A_TouchDrag source){
		this.hasBeenDragged = false;
	}


	void OnDragEvent (PointerEventData p, A_TouchDrag source){
		this.hasBeenDragged = true;

		this.cameraCtrl.Move (p);
	}


	// Cast a ray to the world at the current pointer location.
	bool RaycastHasHitBuilding (PointerEventData p){
		// Convert pointer screen point to ray.
		Ray ray = Camera.main.ScreenPointToRay (p.position);
		RaycastHit hit;
		
		// Check raycast collision with a tile component.
		if (Physics.Raycast (ray, out hit, 100)) {
			if (hit.collider != null) {
				Building building = hit.collider.transform.GetComponent <Building>();
				
				if (building != null) {
					this.targetBuilding = building;
					return true;
				}
			}
		}
		
		this.targetBuilding = null;
		
		return false;
	}
}

