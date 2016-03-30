using UnityEngine;
using System.Collections;

public class GraphicLine : MonoBehaviour {
	
	public Mesh mesh;
	
	public LinePolygon linePolygon;
	
	// Use this for initialization
	void Start () {
		if(linePolygon == null){
			mesh = this.GetComponent<MeshFilter>().mesh;
			//linePolygon = new LinePolygon(mesh, this.transform);
			linePolygon = new LinePolygon(mesh);
		}
	}
	
	public void Begin()
	{
		if(linePolygon == null){
			mesh = this.GetComponent<MeshFilter>().mesh;
			linePolygon = new LinePolygon(mesh, this.transform);
		}
		linePolygon.Start();
	}
	
	public void DrawLine(Vector3 fromPos, Vector3 toPos, float w){
		linePolygon.MoveTo(fromPos, w, true);
		linePolygon.LineTo(toPos, w, true);
	}
	
	public void End()
	{
		linePolygon.End();
	}
}
