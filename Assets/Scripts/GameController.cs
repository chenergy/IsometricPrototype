using UnityEngine;
using System.Collections;


public class GameController : MonoBehaviour {
	private static GameController instance = null;
	
	void Awake (){
		if (instance == null)
			instance = this;
		else
			GameObject.Destroy (this.gameObject);
	}
}