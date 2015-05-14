using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuildingMenu : MonoBehaviour
{
	public RectTransform parentMenu;
	public Text buildingName;
	
	public GameObject selectingPanel;
	public GameObject placingPanel;
	public GameObject editingPanel;

	private Building target;
	
	
	public void Show (Building b, ConstructionState state) {
		this.target = b;
		this.buildingName.text = b.buildingName;
		
		switch (state) {
			case ConstructionState.NO_SELECTION:
				this.selectingPanel.SetActive (false);
				this.placingPanel.SetActive (false);
				this.editingPanel.SetActive (true);
				break;
			
			case ConstructionState.PLACING:
				this.selectingPanel.SetActive (false);
				this.placingPanel.SetActive (true);
				this.editingPanel.SetActive (false);
				break;
			
			case ConstructionState.SELECTING:
				this.selectingPanel.SetActive (true);
				this.placingPanel.SetActive (false);
				this.editingPanel.SetActive (false);
				break;
			
			default:
				break;
		}
		
		this.parentMenu.gameObject.SetActive (true);
	}


	public void Close (){
		this.target = null;
		this.parentMenu.gameObject.SetActive (false);
	}
}

