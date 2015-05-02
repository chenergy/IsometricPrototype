using UnityEngine;
using System.Collections;

[System.Serializable]
public struct IntVector2
{
	public int x;
	public int z;

	public IntVector2 (int x, int z) {
		this.x = x; 
		this.z = z;
	}

	public float sqrMagnitude {
		get { return x * x + z * z; }
	}

	public Vector3 ToVector3 (){
		return new Vector3 (x, 0, z);
	}

	public override string ToString ()
	{
		return string.Format ("({0}, {1})", x, z);
	}
}

