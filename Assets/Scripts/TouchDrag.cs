using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;


public class TouchDrag : MonoBehaviour
{ 
	// Prefab object to create
	public GameObject prefabToCreate;

	// Prefab object to show when dragged
	public GameObject prefabToDrag;

	// Reference to the instantiated drag object
	private GameObject draggedPrefab;
	
	public delegate void TouchEvent ();
	public event TouchEvent OnSelectNoneEvent;


	// Show that the player is interacting by creating an image.
	public void OnPointerDown (BaseEventData b) {
		PointerEventData p = (PointerEventData)b;

		// Create the object image that shows when dragging.
		this.draggedPrefab = GameObject.Instantiate (this.prefabToDrag, p.position, Quaternion.identity) as GameObject;

		// Parent it to this object (displays on the canvas).
		this.draggedPrefab.transform.SetParent (this.transform);
	}


	// On release, what does the player want to interact with?
	public void OnPointerUp (BaseEventData b) {
		PointerEventData p = (PointerEventData)b;

		// Destroy the dragged prefab.
		GameObject.Destroy (this.draggedPrefab);

		// Convert pointer screen point to ray.
		Ray ray = Camera.main.ScreenPointToRay (p.position);
		RaycastHit hit;

		// Check to see if a collider was hit.
		bool colliderHit = false;

		// Cast a ray to the world at the current pointer location.
		if (Physics.Raycast (ray, out hit)) {
			if (hit.collider != null) {
				RaycastReceiver tr = hit.collider.GetComponent <RaycastReceiver> ();
				if (tr != null)
					tr.OnRaycastUpTouch (p, this.gameObject);

				// A collider has been hit.
				colliderHit = true;
			}
		}

		if (!colliderHit) {
			Debug.Log ("Cannot find c3d");

			if (this.OnSelectNoneEvent != null)
				this.OnSelectNoneEvent ();
		}
	}


	// Drag image with the pointer position.
	public void OnDrag (BaseEventData b) {
		PointerEventData p = (PointerEventData)b;

		// Check if main camera raycast hit something.
		Ray ray = Camera.main.ScreenPointToRay (p.position);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit, 100)) {
			if (hit.collider != null) {
				RaycastReceiver tr = hit.collider.GetComponent <RaycastReceiver> ();
				if (tr != null)
					tr.OnRaycastDragTouch (p, this.gameObject);
			}
		}

		// Reposition the dragged image to the position of the pointer.
		if (this.draggedPrefab != null)
			this.draggedPrefab.transform.position = p.position;
	}


	// Callback when a target has been hit by raycast.
	/*protected virtual void OnHitCallback (GameObject target){
		// Get the part, if it exists. on the target.
		A_AttachablePart targetPart = target.GetComponent<A_AttachablePart> ();

		// Process if it exists.
		if (targetPart != null) {
			// Find out if the prefab has an attachable component.
			A_AttachablePart partToCreate = this.prefabToCreate.GetComponent <A_AttachablePart> ();

			// The new part must also have a part script.
			if (partToCreate != null) {
				// The targeted part must be able to accept the new part. 
				// The new part must be attachable to the target part.
				if (targetPart.attachableToSelf.Contains (partToCreate.type) 
					&& partToCreate.attachableToTarget.Contains (targetPart.type)) {
					// Create a new part based on this button's saved prefab.
					A_AttachablePart newPart = (GameObject.Instantiate (this.prefabToCreate) as GameObject)
						.GetComponent <A_AttachablePart> ();

					Debug.Log (newPart.partName.ToString ());

					// Attach the new part to the parent part.
					//targetPart.AttachPart (newPart);
					newPart.AttachTo (targetPart);
				}
			}
		}
	}*/
}

