using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class RaycastReceiver : MonoBehaviour
{
	public delegate void RaycastEvent (GameObject source);
	public event RaycastEvent OnRaycastUp;
	public event RaycastEvent OnRaycastDrag;

	// Use this for initialization
	/*void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}*/


	public void OnRaycastUpTouch (PointerEventData p, GameObject gobj){
		if (this.OnRaycastUp != null)
			this.OnRaycastUp (gobj);
	}


	public void OnRaycastDragTouch (PointerEventData p, GameObject gobj){
		if (this.OnRaycastDrag != null)
			this.OnRaycastDrag (gobj);
	}
}

