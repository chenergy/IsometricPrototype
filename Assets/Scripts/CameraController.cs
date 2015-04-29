using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class CameraController : MonoBehaviour
{
	public Camera mainCamera;
	public TouchListener touchListener;

	[Range (0.0f, 1.0f)]
	public float moveScaleX = 0.5f;

	[Range (0.0f, 1.0f)]
	public float moveScaleY = 0.5f;

	void OnEnable (){
		if (touchListener != null)
			touchListener.OnDragEvent += this.Move;
	}
	
	
	void OnDisable (){
		if (touchListener != null)
			touchListener.OnDragEvent -= this.Move;
	}

	// Use this for initialization
	/*void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}*/

	public void Move (PointerEventData touchInfo){
		float aspectRatio = Screen.width * 1.0f / Screen.height;
		//Vector2 disp = touchInfo.CurScreenPoint - touchInfo.LastScreenPoint;
		Vector2 disp = touchInfo.delta;
		disp *= this.mainCamera.orthographicSize * 0.01f;

		this.mainCamera.transform.localPosition += -disp.x * new Vector3 (1.0f, 0.0f, 0.0f) * this.moveScaleX * (1024.0f / Screen.width) * (aspectRatio / (16.0f / 9.0f));
		this.mainCamera.transform.position += -disp.y * new Vector3 (0.7071f, 0.0f, 0.7071f) * this.moveScaleY * (575.0f / Screen.height);

		//Debug.Log (disp);
	}


	public void SetZoom (float zoom) {
		this.mainCamera.orthographicSize = zoom * 10;
	}
}

