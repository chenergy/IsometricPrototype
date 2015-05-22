using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;


public enum ViewMode {
	DECK_VIEW, MAP_VIEW
}

public class CameraController : MonoBehaviour
{
	// Reference to main camera, child of this transform.
	public Camera mainCamera;
	
	// Buttons controlling different views of the ship.
	public Button btnMapView;
	public Button btnDeckView;
	
	[Range (0.0f, 1.0f)]
	public float moveScaleX = 0.5f;

	[Range (0.0f, 1.0f)]
	public float moveScaleY = 0.5f;
	
	// Reference to last zoom value to return to.
	private float savedZoomValue = 10.0f;
	
	// Check if currently zooming in or out.
	private bool isZooming = false;
	
	// Check which view mode we are currently seeing.
	private ViewMode viewMode = ViewMode.DECK_VIEW;
	public ViewMode Mode {
		get { return this.viewMode; }
	}
	
	// Ship reference so it can be targeted.
	private Ship ship;
	
	
	void Start (){
		this.btnMapView.gameObject.SetActive (true);
		this.btnDeckView.gameObject.SetActive (false);
	}
	
	
	public void Init (Ship ship){
		this.ship = ship;
	}
	

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
	
	
	public void SwitchViewMode (){
		if (!this.isZooming) {			
			switch (this.viewMode){
			case ViewMode.DECK_VIEW:
				this.viewMode = ViewMode.MAP_VIEW;		
				this.btnDeckView.gameObject.SetActive (true);
				this.btnMapView.gameObject.SetActive (false);
				this.savedZoomValue = this.mainCamera.orthographicSize;
				StartCoroutine ("ZoomToSize", 50.0f);
				break;
			case ViewMode.MAP_VIEW:
				this.viewMode = ViewMode.DECK_VIEW;
				this.btnDeckView.gameObject.SetActive (false);
				this.btnMapView.gameObject.SetActive (true);
				//this.savedZoomValue = this.mainCamera.orthographicSize;
				StartCoroutine ("ZoomToSize", this.savedZoomValue);
				break;
			default:
				break;
			}
		}
	}
	
	
	IEnumerator ZoomToSize (float zoom){
		float timer = 0.0f;
		float zoomTime = 1.0f;
		float zoomValue = this.mainCamera.orthographicSize;
		
		this.isZooming = true;
		
		while (timer < zoomTime){
			yield return new WaitForEndOfFrame ();
			this.mainCamera.orthographicSize = Mathf.Lerp (zoomValue, zoom, timer / zoomTime);
			this.mainCamera.transform.localPosition = Vector3.Lerp (this.mainCamera.transform.localPosition, new Vector3 (0.0f, 0.0f, -zoom), timer / zoomTime);
			this.transform.position = Vector3.Lerp (this.transform.position, this.ship.transform.position, timer / zoomTime);
			timer += Time.deltaTime;
		}
		
		this.mainCamera.orthographicSize = zoom;
		this.mainCamera.transform.localPosition = new Vector3 (0.0f, 0.0f, -zoom);
		this.transform.position = this.ship.transform.position;
		
		this.isZooming = false;
	}
}

