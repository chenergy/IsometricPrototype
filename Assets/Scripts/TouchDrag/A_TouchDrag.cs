using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

[RequireComponent (typeof(EventTrigger))]
public abstract class A_TouchDrag : MonoBehaviour
{ 
	// Touch events that pass this object.
	public delegate void TouchDragEvent (PointerEventData p, A_TouchDrag td);
	public event TouchDragEvent OnPointerDownEvent;
	public event TouchDragEvent OnPointerUpEvent;
	public event TouchDragEvent OnDragEvent;

	public EventTrigger eventTrigger;


	void Awake (){
		this.eventTrigger = this.GetComponent <EventTrigger> ();
	}


	// Show that the player is interacting by creating an image.
	public virtual void OnPointerDown (BaseEventData b) {
		PointerEventData p = (PointerEventData)b;

		if (this.OnPointerDownEvent != null)
			this.OnPointerDownEvent (p, this);
	}


	// On release, what does the player want to interact with?
	public virtual void OnPointerUp (BaseEventData b) {
		PointerEventData p = (PointerEventData)b;

		if (this.OnPointerUpEvent != null)
			this.OnPointerUpEvent (p, this);
	}


	// Drag image with the pointer position.
	public virtual void OnDrag (BaseEventData b) {
		PointerEventData p = (PointerEventData)b;

		if (this.OnDragEvent != null)
			this.OnDragEvent (p, this);
	}
}

