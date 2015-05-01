using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class TouchDrag_Button : A_TouchDrag
{
	// Prefab object to create.
	public GameObject prefabToCreate;
	
	// Prefab object to show when dragged.
	public GameObject prefabToDrag;
	
	// Reference to the instantiated drag object.
	private GameObject draggedPrefab;


	// Show that the player is interacting by creating an image.
	public override void OnPointerDown (BaseEventData b) {
		base.OnPointerDown (b);

		PointerEventData p = (PointerEventData)b;
		
		// Create prefab image (if available)
		if (this.prefabToDrag != null) {
			// Create the object image that shows when dragging.
			this.draggedPrefab = GameObject.Instantiate (this.prefabToDrag, p.position, Quaternion.identity) as GameObject;
			
			// Parent it to this object (displays on the canvas).
			this.draggedPrefab.transform.SetParent (this.transform);
		}
	}
	
	
	// On release, what does the player want to interact with?
	public override void OnPointerUp (BaseEventData b) {
		base.OnPointerUp (b);
		
		// Destroy the dragged prefab.
		if (this.draggedPrefab != null)
			GameObject.Destroy (this.draggedPrefab);
	}
	
	
	// Drag image with the pointer position.
	public override void OnDrag (BaseEventData b) {
		base.OnDrag (b);

		PointerEventData p = (PointerEventData)b;
		
		// Reposition the dragged image to the position of the pointer.
		if (this.draggedPrefab != null)
			this.draggedPrefab.transform.position = p.position;
	}
}

