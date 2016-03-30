using UnityEngine;
using System.Collections;

public class DebugDraw {
	
	public static void DrawCross(Vector3 point, float length, Color color){
		float x = point.x;
		float y = point.y;
		Debug.DrawLine(	new Vector3(x - length, y, 0), 
						new Vector3(x + length, y, 0),
						color);
		Debug.DrawLine(	new Vector3(x, y - length, 0), 
						new Vector3(x, y + length, 0),
						color);
	}
}
