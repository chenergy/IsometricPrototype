using UnityEngine;
using System.Collections;
using TouchControl;

public class RaycastReceiver : MonoBehaviour
{
	public delegate void RaycastEvent ();
	public event RaycastEvent OnRaycastHit;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void OnHit (TouchInfo t){
		if (this.OnRaycastHit != null)
			this.OnRaycastHit ();
	}
}

