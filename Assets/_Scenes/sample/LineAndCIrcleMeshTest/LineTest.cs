using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof(MeshFilter))]
[RequireComponent (typeof(MeshRenderer))]
public class LineTest : MonoBehaviour
{
	private LinePolygon2 line;
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
		//line = new LinePolygon(m, this.transform);
		line = new LinePolygon2 (m);
		
		line.Start ();
	
		line.angleZ = angleA;
		line.thickness = thicknessA;

		for (int i = 0; i < (num+1); i++) {
			Vector3 pt = point[i];

			Vector3 pt1 = point[i+1];
			Vector3 pt0 = point[i];
			Vector3 fwd = pt1 - pt0;

			Vector3 up = new Vector3(0, 1, 0);
			Quaternion t_quat = Quaternion.LookRotation(fwd, up);
			Quaternion quat = t_quat;
			
			if (i == 0) {
				line.MoveTo (pt, thicknessA, quat);
			} else {
				line.LineTo (pt, thicknessA, quat);
			}
		}
		
		line.End ();
	}
}
