using UnityEngine;
using System.Collections;

public class NavigationController : MonoBehaviour
{
	private Ship ship;
	
	
	public void Init (Ship ship){
		this.ship = ship;
	}
	
	
	public void MoveShip (Vector3 pos){
		this.ship.MoveTo (pos);
	}
}