using UnityEngine;
using System;
using System.Collections;
using TouchControl;


public enum CameraType {
	MAIN, INPUT
}

public class TouchListener : MonoBehaviour {
	public Camera inputCamera;
	public bool viewGizmos = false;

	private TouchInfo[] touches;
	private static TouchListener instance = null;

	public delegate void TouchEvent (TouchInfo touchInfo);
	public event TouchEvent OnTouchBegan;
	public event TouchEvent OnTouchMoved;
	public event TouchEvent OnTouchStationary;
	public event TouchEvent OnTouchEnd;


	public TouchInfo[] Touches {
		get { return this.touches; }
	}

	public static TouchListener Instance {
		get { return instance; }
	}
	
	
	void Awake (){
		if (instance != null) {
			GameObject.Destroy (this.gameObject);
		} else {
			instance = this;
		}
	}
	
	void Start (){
		this.touches = new TouchInfo[] { new TouchInfo (0), new TouchInfo (1) };
	}
	
	void Update (){
		#if UNITY_EDITOR
		TouchInfo mouseTouch = this.touches[0];

		if (Input.GetMouseButton (0)){
			Vector2 mousePos = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);

			// Call for an input update using mouse position
			if (Input.GetMouseButtonDown (0)) {
				// Check if main camera raycast hit something.
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast(ray, out hit, 100)) {
					RaycastReceiver tr = hit.collider.GetComponent <RaycastReceiver> ();
					if (tr != null)
						tr.OnHit (this.touches[0]);
				}

				mouseTouch.OnTouchBegan (mousePos);
				if (this.OnTouchBegan != null) 
					this.OnTouchBegan (this.touches[0]);
			} else {
				if ((mousePos - mouseTouch.LastScreenPoint).sqrMagnitude > 0) {
					mouseTouch.OnTouchMoved (mousePos);
					if (this.OnTouchMoved != null) 
						this.OnTouchMoved (this.touches[0]);
				} else {
					mouseTouch.OnTouchStationary (mousePos);
					if (this.OnTouchStationary != null) 
						this.OnTouchStationary (this.touches[0]);
				}
			}
		} else {
			if (!mouseTouch.IsEnded) {
				mouseTouch.OnTouchEnd ();
				if (this.OnTouchEnd != null) 
					this.OnTouchEnd (this.touches[0]);
			} else {
				if (mouseTouch.IsUp)
					mouseTouch.OnTouchFinished ();
			}
		}

		#elif UNITY_IOS
		if (Input.touchCount > 0) {
			for (int i = 0; i < Input.touchCount; i++) {
				Touch inputTouch = Input.touches[i];
				Vector2 touchPos = inputTouch.position;

				switch (inputTouch.phase){
				case TouchPhase.Began:
					this.touches[i].OnTouchBegan (touchPos);
					break;
				case TouchPhase.Moved:
					this.touches[i].OnTouchMoved (touchPos);
					break;
				case TouchPhase.Stationary:
					this.touches[i].OnTouchStationary (touchPos);
					break;
				case TouchPhase.Ended:
					if (!this.touches[i].IsEnded)
						this.touches[i].OnTouchEnd ();
					else 
						if (this.touches[i].IsUp)
							this.touches[i].OnTouchFinished ();
					break;
				default:
					break;
				}
			}
		} else {
			foreach (TouchInfo t in this.touches){
				if (!t.IsEnded)
					t.OnTouchEnd ();
				else 
					if (t.IsUp)
						t.OnTouchFinished ();
			}
		}
		#endif

		/*for (int i = 0; i < this.touches.Length; i++) {
			TouchInfo ti = this.touches [i];
			Debug.Log ("[" + i.ToString() + "] CurScreenPoint: " + ti.CurScreenPoint);
		}*/
	}


	void OnDrawGizmos (){
		if (this.viewGizmos) {
			if (Application.isEditor) {
				if (Camera.current == this.inputCamera/* || Camera.current == SceneView.lastActiveSceneView.camera*/) {
					if (this.touches != null) {
						foreach (TouchInfo t in this.touches) {
							Vector3 firstWorldPoint = this.ScreenToWorldPoint (t.FirstScreenPoint, CameraType.INPUT);
							Vector3 curWorldPoint = this.ScreenToWorldPoint (t.CurScreenPoint, CameraType.INPUT);

							Gizmos.DrawLine (firstWorldPoint, curWorldPoint);
							Gizmos.DrawSphere (curWorldPoint, 0.25f);
						}
					}
				}
			}
		}
	}


	public Vector3 ScreenToWorldPoint (Vector2 screenPoint, CameraType camera){
		if (camera == CameraType.INPUT) {
			Vector3 curInputWorldPoint = TouchListener.Instance.inputCamera.ScreenToWorldPoint (screenPoint);
			curInputWorldPoint = new Vector3 (curInputWorldPoint.x, curInputWorldPoint.y, 0.0f);
			return curInputWorldPoint; 
		} else if (camera == CameraType.MAIN) {
			Vector3 curInputWorldPoint = Camera.main.ScreenToWorldPoint (screenPoint);
			curInputWorldPoint = new Vector3 (curInputWorldPoint.x, curInputWorldPoint.y, 0.0f);
			return curInputWorldPoint; 
		}

		return Vector3.zero;
	}
}

