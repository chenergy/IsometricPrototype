using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody))]
public class Ship : MonoBehaviour
{
	public TileController tc;
	public float speed = 1.0f;

	private Rigidbody rigidbody;
	private Vector3 moveDir = Vector3.zero;

	void Start (){
		this.rigidbody = this.GetComponent <Rigidbody> ();
	}

	void Update (){
		if (this.moveDir != Vector3.zero) {
			//this.rigidbody.AddRelativeForce
		}
	}

	void OnDrawGizmos (){
		Gizmos.DrawLine (this.transform.position, this.transform.position + this.transform.forward * 10);
	}


	public void MoveInDirection (Vector3 dir){
		
	}


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