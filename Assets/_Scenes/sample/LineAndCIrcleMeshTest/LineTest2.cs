using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(MeshFilter))]
[RequireComponent (typeof(MeshRenderer))]
public class LineTest2 : MonoBehaviour
{
	private LinePolygon3 line;
	public int num = 12;
	public float r = 10;
	public float angleA = -90.0f;
	public float angleB = 0.0f;
	public float thicknessA = 2;
	public float thicknessB = 2;

	public List<Transform> transList;

	Vector3[] point;
	
	// Use this for initialization
	void Start ()
	{
		num = (num <= 1) ? 2 : num;
		float d = Mathf.PI * 2 / num;

		point = new Vector3[num+1+1];
		for (int i = 0; i < (num+1+1); i++) {
			float x = r * Mathf.Sin (d * i);
			float z = r * Mathf.Cos (d * i);
			Vector3 pt = new Vector3 (x, 0, z);
			point[i] = pt;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		Mesh m = (this.gameObject.GetComponent<MeshFilter> () as MeshFilter).mesh;
		line = new LinePolygon3 (m);
		
		line.Start ();
	
		line.angleZ = angleA;
		line.thickness = thicknessA;

		for (int i = 0; i < (num+1); i++) {
			Vector3 pt = point[i];
			if (i == 0) {
				line.MoveTo (pt, thicknessA);
			} else {
				line.LineTo (pt, thicknessA);
			}
		}
		
		line.End ();
	}
}
