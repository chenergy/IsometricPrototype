using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class CameraController : MonoBehaviour
{
	public Camera mainCamera;
	//public TouchDrag_Camera touchDrag;

	[Range (0.0f, 1.0f)]
	public float moveScaleX = 0.5f;

	[Range (0.0f, 1.0f)]
	public float moveScaleY = 0.5f;
	

	public void Move (PointerEventData p){
		float aspectRatio = Screen.width * 1.0f / Screen.height;
		Vector2 disp = p.delta;
		disp *= this.mainCamera.orthographicSize * 0.01f;

		this.mainCamera.transform.localPosition += -disp.x * new Vector3 (1.0f, 0.0f, 0.0f) * this.moveScaleX * (1024.0f / Screen.width) * (aspectRatio / (16.0f / 9.0f));
		this.transform.position += -disp.y * new Vector3(this.transform.forward.x, 0.0f, this.transform.forward.z).normalized * this.moveScaleY * (575.0f / Screen.height);
	}


	public void SetZoom (float zoom) {
		this.mainCamera.orthographicSize = Mathf.Lerp (10f, 1f, zoom);
	}


	public void SetRotation (float r) {
		this.transform.rotation = Quaternion.Euler (new Vector3 (30.0f, Mathf.Lerp (45.0f, 404.0f, r), 0.0f));
	}
}

