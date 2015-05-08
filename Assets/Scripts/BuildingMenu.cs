using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuildingMenu : MonoBehaviour
{
	public RectTransform parentMenu;
	public Text buildingName;
	/*public RectTransform placeButton;
	public RectTransform cancelButton;
	public RectTransform moveButton;
	public RectTransform deleteButton;*/

	private Building target;

	// Use this for initialization
	/*void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}*/

	public void Open (Building b) {
		this.target = b;
		this.buildingName.text = b.buildingName;
		this.parentMenu.gameObject.SetActive (true);
	}


	public void Close (){
		this.target = null;
		this.parentMenu.gameObject.SetActive (false);
	}
}

