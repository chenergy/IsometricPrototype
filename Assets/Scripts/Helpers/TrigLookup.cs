using UnityEngine;
using System.Collections;

public static class TrigLookup
{
	// For trig lookup tables
	public const float PI2 = Mathf.PI * 2.0f; 
	private const int TABLE_SIZE = 1024 * 128;
	private const float TABLE_SIZE_D = (float)TABLE_SIZE;
	private const float FACTOR = TABLE_SIZE_D / PI2;

	private static float[] _CosineFloatTable;
	private static float[] _SineFloatTable;

	//private static void InitializeTrigonometricTables(){
	static TrigLookup() {
		_CosineFloatTable = new float[TABLE_SIZE];
		_SineFloatTable = new float[TABLE_SIZE];

		for (int i = 0; i < TABLE_SIZE; i++) {
			float Angle = (1.0f * i / TABLE_SIZE_D) * PI2;
			_SineFloatTable [i] = Mathf.Sin (Angle);
			_CosineFloatTable [i] = Mathf.Cos (Angle);
		}
	}

	public static float Sin (float value){
		value %= PI2;  // In case that the angle is larger than 2pi
		if (value < 0) value += PI2; // in case that the angle is negative
		int index = (int)(value * FACTOR); //from radians to index and casted in to an int
		return _SineFloatTable[index]; // get the value from the table
	}


	public static float Cos (float value){
		value %= PI2;  // In case that the angle is larger than 2pi
		if (value < 0) value += PI2; // in case that the angle is negative
		int index = (int)(value * FACTOR); //from radians to index and casted in to an int
		return _CosineFloatTable[index]; // get the value from the table
	}
}

