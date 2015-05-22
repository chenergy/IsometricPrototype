using UnityEngine;
using System.Collections;


public class GameController : MonoBehaviour {
	public ConstructionController conCtrl;
	//public EventTriggerOverlay eventTrig;
	public NavigationController navCtrl;
	public CameraController camCtrl;
	public GameObject shipPrefab;
	
	private Ship mainShip;
	private static GameController instance = null;
	
	
	void Awake (){
		if (instance == null)
			instance = this;
		else
			GameObject.Destroy (this.gameObject);
	}
	
	void Start (){
		this.mainShip = (GameObject.Instantiate (this.shipPrefab, Vector3.zero, Quaternion.identity) as GameObject).GetComponent <Ship>();
		
		//this.eventTrig.Init (this.mainShip);
		this.conCtrl.Init (this.mainShip.tc);
		this.camCtrl.Init (this.mainShip);
		this.navCtrl.Init (this.mainShip);
	}
}