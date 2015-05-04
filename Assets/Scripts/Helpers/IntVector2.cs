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

	public static IntVector2 operator +(IntVector2 v1, IntVector2 v2){
		return new IntVector2 (v1.x + v2.x, v1.z + v2.z);
	}

	public override bool Equals (object obj)
	{
		// If parameter is null return false.
		if (obj == null)
		{
			return false;
		}
		
		// If parameter cannot be cast return false.
		IntVector2 p = (IntVector2)obj;
		if ((object)p == null)
		{
			return false;
		}
		
		// Return true if the fields match:
		return (x == p.x) && (z == p.z);
	}
}

