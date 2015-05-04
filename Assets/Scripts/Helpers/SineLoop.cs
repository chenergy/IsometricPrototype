using UnityEngine;
using System.Collections;

public class SineLoop : MonoBehaviour
{
	public float speed = 1.0f;
	public float variance = 1.0f;

	//private Vector3 startPos;

	// Use this for initialization
	void Start ()
	{
		//this.startPos = this.transform.position;
	}
	
	// Update is called once per frame
	void Update ()
	{
		this.transform.localPosition = new Vector3 (0, TrigLookup.Sin (Time.time * this.speed) * this.variance, 0);
	}
}

