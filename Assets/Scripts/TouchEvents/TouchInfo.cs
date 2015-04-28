using UnityEngine;
using System.Collections;


namespace TouchControl
{
	public class TouchInfo
	{
		protected 	Vector2 	firstScreenPoint;
		protected 	Vector2 	lastScreenPoint;
		protected 	Vector2 	curScreenPoint;

		private 	int 		id;
		private 	bool 		isStarted 	= false;
		private 	bool 		isEnded 	= true;
		private 	bool 		isDown 		= false;
		private 	bool 		isPressed 	= false;
		private 	bool 		isUp 		= false;
		private 	TouchPhase 	phase 		= TouchPhase.Ended;

		public TouchInfo (int id)
		{
			this.id = id;
			this.firstScreenPoint = this.lastScreenPoint = this.curScreenPoint = Vector3.zero;
		}

		public int Id {
			get { return this.id; }
		}

		public Vector2 FirstScreenPoint {
			get { return this.firstScreenPoint; }
		}

		public Vector2 LastScreenPoint {
			get { return this.lastScreenPoint; }
		}

		public Vector2 CurScreenPoint {
			get { return this.curScreenPoint; }
		}

		public bool IsStarted {
			get { return this.isStarted; }
		}

		public bool IsEnded {
			get { return this.isEnded; }
		}

		public bool IsDown {
			get { return this.isDown; }
		}

		public bool IsUp {
			get { return this.isUp; }
		}

		public bool IsPressed {
			get { return this.isPressed; }
		}

		public TouchPhase Phase {
			get { return this.phase; }
		}

		// Private helper functions for touch events
		public void OnTouchBegan (Vector2 curScreenPoint) {
			this.phase = TouchPhase.Began;
			this.isStarted = true;
			this.isEnded = false;
			this.isDown = true;
			this.isPressed = true;
			this.isUp = false;
			this.curScreenPoint = curScreenPoint;
			this.firstScreenPoint = this.lastScreenPoint = curScreenPoint;
		}

		public void OnTouchStationary (Vector2 curScreenPoint){
			this.phase = TouchPhase.Stationary;
			this.isDown = false;
			this.lastScreenPoint = this.curScreenPoint;
			this.curScreenPoint = curScreenPoint;
		}

		public void OnTouchMoved (Vector2 curScreenPoint) {
			this.phase = TouchPhase.Moved;
			this.isDown = false;
			this.lastScreenPoint = this.curScreenPoint;
			this.curScreenPoint = curScreenPoint;
		}

		public void OnTouchEnd () {
			this.phase = TouchPhase.Ended;
			this.isStarted = false;
			this.isEnded = true;
			this.isDown = false;
			this.isPressed = false;
			this.isUp = true;
			this.firstScreenPoint = this.lastScreenPoint = this.curScreenPoint = Vector3.zero;
		}

		public void OnTouchFinished (){
			this.isUp = false;
		}
	}
}

