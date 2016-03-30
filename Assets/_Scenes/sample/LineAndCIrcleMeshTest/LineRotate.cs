using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
//[RequireComponent (typeof (MeshFilter))]
[RequireComponent (typeof (MeshRenderer))]
public class LineRotate : MonoBehaviour {
	private LinePolygon line;
	
	public int num = 12;
	public float r = 10;
	
	public float angleA = -90.0f;
	public float angleB = 0.0f;
	
	public float thicknessA = 2;
	public float thicknessB = 2;

	private Mesh mesh;
	
	// Use this for initialization
	void Start () {
		MeshFilter mf = this.gameObject.GetComponent<MeshFilter>();
		if(mf == null){
			mf = this.gameObject.AddComponent<MeshFilter>();
		}
		if(mesh == null){
			mesh = new Mesh();
		}
		mf.mesh = mesh;
		//line = new LinePolygon(m, this.transform);
		line = new LinePolygon(mesh);
	}

	public Transform[] points;
	public float[] anglesZ;
	
	// Update is called once per frame
	void Update () {
		
		num = (num <= 1) ? 2 : num;

		line.Start();

//		float d = Mathf.PI * 2 / num;
//		line.angleZ = angleA;
//		line.thickness = thicknessA;
//		for(int i = 0; i < num+1; i++){
//			float x = r * Mathf.Sin(d * i);
//			float z = r * Mathf.Cos(d * i);
//			if(i == 0){
//				line.MoveTo(new Vector3(x,0,z));
//			}else{
//				line.LineTo(new Vector3(x,0,z));
//			}
//		}
		
		line.angleZ = angleB;
		line.thickness = 1;
		for(int i = 0; i < points.Length; i++){
			float thickness = thicknessB;
			float angleZ = anglesZ[i];
			Transform trans = points[i];
			if(i == 0){
				line.MoveTo(trans.position, thickness, angleZ);
			}else{
				line.LineTo(trans.position, thickness, angleZ);
			}
		}
		
		line.End();
	}


}
