using UnityEngine;
using System.Collections;

[RequireComponent (typeof (LineRenderer))]
[ExecuteInEditMode]
public class ConnectionLine : MonoBehaviour {
	
	public Transform start;
	public Transform end;
	public float w = 0.1f;
	public float baseScreenZ = 20.0f;
	
	public LineRenderer lineRenderer;
	
	// Use this for initialization
	void Start () {
		lineRenderer = this.gameObject.GetComponent<LineRenderer>();
		lineRenderer.useWorldSpace = true;
	}
	
	public float perStart = 0.0f;
	public float perEnd = 1.0f;
	
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
		lineRenderer.SetVertexCount(2);
		lineRenderer.SetPosition(0, startPos);
		lineRenderer.SetPosition(1, endPos);
		
		lineRenderer.SetWidth(w*perS, w*perE);
	}
}
