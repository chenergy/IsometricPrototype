using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;


public class TouchListener : MonoBehaviour {
	private static TouchListener instance = null;

	public delegate void TouchEvent (PointerEventData e);
	public event TouchEvent OnPointerDownEvent;
	public event TouchEvent OnPointerUpEvent;
	public event TouchEvent OnDragEvent;


	public static TouchListener Instance {
		get { return instance; }
	}
	
	
	void Awake (){
		if (instance != null) {
			GameObject.Destroy (this.gameObject);
		} else {
			instance = this;
		}
	}


	public void OnPointerDown (BaseEventData b){
		PointerEventData p = (PointerEventData)b;

		// Check if main camera raycast hit something.
		Ray ray = Camera.main.ScreenPointToRay (p.position);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 100)) {
			RaycastReceiver tr = hit.collider.GetComponent <RaycastReceiver> ();
			if (tr != null)
				tr.OnRaycastUpTouch (p, this.gameObject);
		}

		if (this.OnPointerDownEvent != null)
			this.OnPointerDownEvent (p);
	}


	public void OnPointerUp (BaseEventData b){
		PointerEventData p = (PointerEventData)b;

		if (this.OnPointerUpEvent != null)
			this.OnPointerUpEvent (p);
	}


	public void OnDrag (BaseEventData b){
		PointerEventData p = (PointerEventData)b;

		if (this.OnDragEvent != null)
			this.OnDragEvent (p);
	}
}

