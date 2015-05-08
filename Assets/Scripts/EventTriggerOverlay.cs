using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class EventTriggerOverlay : MonoBehaviour
{
	public TouchDrag_Select touch;
	public CameraController cameraCtrl;
	public ConstructionController constructionCtrl;

	private bool hasBeenDragged = false;
	//private Building targetBuilding;


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
			Building b = this.RaycastHitBuilding (p);
			if (b != null) {
				//this.constructionCtrl.SelectBuilding (this.targetBuilding);
				this.constructionCtrl.OnBuildingSelectEvent (b);
			} else {
				//this.constructionCtrl.DeselectBuilding ();
				this.constructionCtrl.OnBuildingDeselectEvent ();
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
	Building RaycastHitBuilding (PointerEventData p){
		// Convert pointer screen point to ray.
		Ray ray = Camera.main.ScreenPointToRay (p.position);
		RaycastHit hit;
		
		// Check raycast collision with a tile component.
		if (Physics.Raycast (ray, out hit, 100)) {
			if (hit.collider != null) {
				Building building = hit.collider.transform.GetComponent <Building>();
				
				if (building != null) {
					//this.targetBuilding = building;
					//return true;
					return building;
				}
			}
		}
		
		//this.targetBuilding = null;
		
		//return false;

		return null;
	}
}

