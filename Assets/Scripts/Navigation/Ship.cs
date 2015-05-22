using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour
{
	public TileController tc;
	public float speed = 1.0f;
	
	
	
	public void MoveTo (Vector3 pos){
		StopCoroutine ("MoveToRoutine");
		StopCoroutine ("RotateToRoutine");
		
		Vector3 from = new Vector3 (this.transform.position.x, 0.0f, this.transform.position.z);
		Vector3 to = new Vector3 (pos.x, 0.0f, pos.z);
		Quaternion rot = Quaternion.LookRotation (to - from, Vector3.up);
		
		StartCoroutine ("MoveToRoutine", pos);
		StartCoroutine ("RotateToRoutine", rot);
	}
	
	
	IEnumerator MoveToRoutine (Vector3 pos){
		float p = 0.0f;
		float q = 0.0f;
		Vector3 initPos = this.transform.position;
		
		while (p < Mathf.PI) {
			yield return new WaitForEndOfFrame ();

			q = (1 - TrigLookup.Cos (p)) / 2.0f;  // Also, (cos (x + PI) + 1) / 2
			this.transform.position = Vector3.Lerp (initPos, pos, q);
				
			p += Time.deltaTime * this.speed;
		}
		
		this.transform.position = pos;
	}
	
	
	IEnumerator RotateToRoutine (Quaternion rot){
		float p = 0.0f;
		float q = 0.0f;
		Quaternion initRot = this.transform.rotation;

		while (p < Mathf.PI) {
			yield return new WaitForEndOfFrame ();

			q = (1 - TrigLookup.Cos (p)) / 2.0f;  // Also, (cos (x + PI) + 1) / 2
			this.transform.rotation = Quaternion.Lerp (initRot, rot, q);

			p += Time.deltaTime * this.speed * 2;
		}
		
		this.transform.rotation = rot;
	}
}