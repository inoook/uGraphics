using UnityEngine;
using System.Collections;

[RequireComponent (typeof (MeshRenderer))]
[RequireComponent (typeof (MeshFilter))]
[ExecuteInEditMode]
public class ConnectionLineMesh : MonoBehaviour {
	
	public Transform start;
	public Transform end;
	public float w = 0.1f;
	public float baseScreenZ = 20.0f;
	
	public float perStart = 0.0f;
	public float perEnd = 1.0f;
	
	public Mesh mesh;
	public LinePolygon linePolygon;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(start == null || end == null){ return; }
		
		Vector3 direction = (end.position - start.position);
		Vector3 startPos = start.position + direction * perStart;
		Vector3 endPos = start.position + direction * perEnd;
		
		Camera camera = Camera.main;
		Vector3 start_screenPos = camera.WorldToScreenPoint(startPos);
		Vector3 end_screenPos = camera.WorldToScreenPoint(endPos);
		
		float perS = start_screenPos.z / baseScreenZ;
		float perE = end_screenPos.z / baseScreenZ;
		
		if(mesh == null){
			mesh = new Mesh();
			mesh.name = "lineMesh";
			this.GetComponent<MeshFilter>().sharedMesh = mesh;
		}
		if(linePolygon == null){
			//mesh = this.GetComponent<MeshFilter>().mesh;
			linePolygon = new LinePolygon(mesh, this.transform);
		}
		linePolygon.Start();
		linePolygon.MoveTo(startPos, w * perS, true);
		linePolygon.LineTo(endPos, w * perE, true);
		//linePolygon.LineTo(endPos*2, w * perE, true);
		linePolygon.End();
	}
}
