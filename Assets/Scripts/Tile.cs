using UnityEngine;
using System.Collections;


public class Tile : MonoBehaviour {
	public RaycastReceiver receiver;


	void OnEnable (){
		if (receiver != null)
			receiver.OnRaycastHit += this.Selected;
	}


	void OnDisable (){
		if (receiver != null)
			receiver.OnRaycastHit -= this.Selected;
	}


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void Selected (){
		Debug.Log ("Selected " + this.transform.position.ToString ());
	}
}
