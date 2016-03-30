using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LinePolygon2 {
	
	private enum LineCMD
	{
		MOVETO, LINETO
	}

	private class ProtPoint{
		public LineCMD cmd;
		public Vector3 point;
		public Quaternion? rotation = null;
		public float thickness;
	}

	private Vector3 preVector3;
	//private Vector3 preCrossVector;
	private Vector3 myVector;
	private Vector3 myPreVector;

	public float thickness = 2;

	private Mesh mesh;
	private Transform trans;
	//private ArrayList myVertices;
	private List<Vector3> myVertices;

	private List<float> angleList;
	private List<ProtPoint> protPoints;

	
	public float angleZ = 0.0f;

	public LinePolygon2(Mesh mesh_, Transform trans_){
		mesh = mesh_;
		trans = trans_;
		myVertices = new List<Vector3>();
	}
	public LinePolygon2(Mesh mesh_){
		mesh = mesh_;
		myVertices = new List<Vector3>();
	}
	
	public void Start(){
		Clear();
	}
	public void End()
	{
		UpdateLines();
	}

	void Init(Vector3 v)
	{
		preVector3 = v;
	}
	
	public void Clear()
	{
		if(mesh != null){
			mesh.Clear();
		}
		
		if(angleList != null){
			angleList.Clear();
		}

		if(myVertices != null){
			myVertices.Clear();
		}
		//myVertices = new ArrayList();
		myVertices = new List<Vector3>();
		
		angleList = new List<float>();

		if(protPoints != null){
			protPoints.Clear();
		}
		protPoints = new List<ProtPoint>();
	}
	
	public void MoveTo(Vector3 toVector3)
	{
		MoveTo(toVector3, thickness, angleZ);
	}
	public void MoveTo(Vector3 toVector3, float thickness_)
	{
		MoveTo(toVector3, thickness_, angleZ);
	}
	public void MoveTo(Vector3 toVector3, float thickness_, float rotateZ)
	{
		Init(toVector3);
		
		angleList.Add(rotateZ);
		
		ProtPoint prot = new ProtPoint();
		prot.cmd = LineCMD.MOVETO;
		prot.point = toVector3;
		prot.thickness = thickness_;

		protPoints.Add(prot);
	}
	public void MoveTo(Vector3 toVector3, float thickness_, Quaternion quat)
	{
		Init(toVector3);

		ProtPoint prot = new ProtPoint();
		prot.cmd = LineCMD.MOVETO;
		prot.point = toVector3;
		prot.thickness = thickness_;
		prot.rotation = quat;
		
		protPoints.Add(prot);
	}
	
	public void LineTo(Vector3 toVector3)
	{
		LineTo(toVector3, thickness, angleZ);
	}
	public void LineTo(Vector3 toVector3, float thickness_)
	{
		LineTo(toVector3, thickness_, angleZ);
	}
	public void LineTo(Vector3 toVector3, float thickness_, float rotateZ)
	{
		LineTo(toVector3, thickness_, rotateZ, false);
	}
	public void LineTo(Vector3 toVector3, float thickness_, bool isBillboard)
	{
		LineTo(toVector3, thickness_, 0, isBillboard);
	}
	public void LineTo(Vector3 toVector3, float thickness_, float rotateZ, bool isBillboard)
	{
		angleList.Add(rotateZ);
		
		ProtPoint prot = new ProtPoint();
		prot.cmd = LineCMD.LINETO;
		prot.point = toVector3;
		prot.thickness = thickness_;
		
		protPoints.Add(prot);
	}
	public void LineTo(Vector3 toVector3, float thickness_, Quaternion quat)
	{
		ProtPoint prot = new ProtPoint();
		prot.cmd = LineCMD.LINETO;
		prot.point = toVector3;
		prot.thickness = thickness_;
		prot.rotation = quat;
		
		protPoints.Add(prot);
	}

	public void LineToBillBoard(Vector3 toVector3, float thickness_)
	{
		LineTo(toVector3, thickness_, 0, true);
	}
	private Vector3 CrossUnitVector2(Vector3 preVec, Vector3 vec, float rotateZ)
	{
		Vector3 v = ((vec.normalized + preVec.normalized) / 2.0f).normalized;
		return v;
	}
	private Vector3 CrossUnitVector(Vector3 vec, float rotateZ)
	{
		Quaternion q = Quaternion.LookRotation(vec, Vector3.up);
		q *= Quaternion.AngleAxis(rotateZ, Vector3.forward);
		Vector3 v = q * Vector3.right;
		
		v.Normalize();
		return v;
	}

	private Quaternion GetRotation(Vector3 vec, float rotateZ)
	{
		Quaternion q = Quaternion.LookRotation(vec, Vector3.up);
		q *= Quaternion.AngleAxis(rotateZ, Vector3.forward);
		return q;
	}
	
	private Vector3 CrossUnitVectorBillboard(Vector3 vec)
	{
		Vector3 forward = Camera.main.transform.forward;
		Vector3 v = Vector3.Cross(vec, forward);
		
		/*
		Quaternion q = Quaternion.LookRotation(-vec, Vector3.forward);
		Vector3 v = q * Vector3.right;
		*/
		
		v.Normalize();
		
		return v;
	}
	private Vector3 CrossUnitVectorBillboard2(Vector3 dir, Vector3 pos)
	{
		//Vector3 forward = Camera.mainCamera.transform.forward;
		//Vector3 v = Vector3.Cross(vec, forward);
		
		/*
		Quaternion q = Quaternion.LookRotation(-vec, Vector3.forward);
		Vector3 v = q * Vector3.right;
		*/
		Vector3 forward = Camera.main.transform.position - pos;
		
		Quaternion q = Quaternion.LookRotation(forward, -dir);
		Vector3 v = q * Vector3.right;
		
		v.Normalize();
		
		return v;
	}
	
	// update Mesh
	private void UpdateLines(){

		myVertices = new List<Vector3>();

		Vector3 myVector; 
		Vector3 preVector3 = protPoints[0].point;
		float thicknessStart = protPoints[0].thickness;

		for(int i = 0; i < protPoints.Count; i++){
			ProtPoint prot = protPoints[i];
			LineCMD c = prot.cmd;

			if(c == LineCMD.MOVETO){
				preVector3 = prot.point;
			}

			Vector3 toVector3 = prot.point;
			float _thickness = prot.thickness;

			if(i < protPoints.Count-1){
				if(protPoints[i+1].cmd == LineCMD.MOVETO){
					myVector = myPreVector;
				}else{
					Vector3 nextVector3 = protPoints[i+1].point;
					myVector = nextVector3 - toVector3;
				}
			}else{
				myVector = myPreVector;
				myPreVector = protPoints[i-1].point - protPoints[i-2].point;
			}
			
			Vector3 crossUnit;
			
			if(c == LineCMD.MOVETO){
				thicknessStart = protPoints[i].thickness;
				Vector2 t_myVector = protPoints[i+1].point - protPoints[i].point;
				//crossUnit = CrossUnitVector(t_myVector, angleZ);

				Quaternion q;
				if(protPoints[i].rotation == null){
					float angleZ = angleList[i];
					q = GetRotation(t_myVector, angleZ);
				}else{
					q = (Quaternion)(protPoints[i].rotation);
				}
				
				crossUnit = q * Vector3.right;
				crossUnit.Normalize();

				crossUnit *= thicknessStart/2;

				Vector3 p_vecA = new Vector3( preVector3.x + crossUnit.x, preVector3.y + crossUnit.y, preVector3.z + crossUnit.z);
				Vector3 p_vecB = new Vector3( preVector3.x - crossUnit.x, preVector3.y - crossUnit.y, preVector3.z - crossUnit.z);
				myVertices.Add( p_vecA );
				myVertices.Add( p_vecB );
			}
			if(c == LineCMD.LINETO){
				Quaternion q;
				if(protPoints[i].rotation == null){
					// auto
					float angleZ = angleList[i];
					q = GetRotation(myVector, angleZ);
				}else{
					q = (Quaternion)(protPoints[i].rotation);
//					Vector3 unit = CrossUnitVector2(-myPreVector, myVector, angleZ);
//					q = Quaternion.FromToRotation(Vector3.right, unit);
//					Debug.DrawLine(toVector3, unit * 1.0f + toVector3, Color.yellow);
				}
				crossUnit = q * Vector3.right;
				crossUnit.Normalize();
				
				crossUnit *= _thickness/2;

				Vector3 vecA = new Vector3(crossUnit.x + toVector3.x, crossUnit.y + toVector3.y, crossUnit.z + toVector3.z);
				Vector3 vecB = new Vector3(-crossUnit.x + toVector3.x, -crossUnit.y + toVector3.y, -crossUnit.z + toVector3.z);
				
				myVertices.Add(vecA);
				myVertices.Add(vecB);
			}
			
			preVector3 = toVector3;
			myPreVector = myVector;
		}

		// uvs
		int sections_all = protPoints.Count;
		Vector2[] uv = new Vector2[sections_all * 2];

//		int u = 0;
//		for(int n = 0; n < sections_all; n++){
//			u = n % 2;
//			
//			uv[n * 2 + 0] = new Vector2(u, 0);
//			uv[n * 2 + 1] = new Vector2(u, 1);
//		}

//		for(int n = 0; n < sections_all; n++){
//			float p = ((float)n/(float)sections_all);
//			uv[n * 2 + 0] = new Vector2(1.0f*p, 1.0f);
//			uv[n * 2 + 1] = new Vector2(1.0f*p, 0.0f);
//		}

		float length = 0;
		for(int n = 1; n < sections_all; n++){
			float dist = Vector3.Distance( protPoints[n].point, protPoints[n-1].point );
			length += dist;
		}
		float repeatCount = length / thickness;

		float allDist = 0;
		uv[0] = new Vector2(0, 1.0f);
		uv[1] = new Vector2(0, 0.0f);
		for(int n = 1; n < sections_all; n++){
			float dist = Vector3.Distance( protPoints[n].point, protPoints[n-1].point );
			allDist += dist;
			float p = allDist/length * repeatCount;
			uv[n * 2 + 0] = new Vector2(p, 1.0f);
			uv[n * 2 + 1] = new Vector2(p, 0.0f);
		}
		
		int[] triangles = new int[(myVertices.Count - 2) * 3];

		for (int i=0; i < triangles.Length / 6; i++)
		{
			if(protPoints[i+1].cmd != LineCMD.MOVETO){
				triangles[i * 6 + 0] = i * 2;
				triangles[i * 6 + 1] = i * 2 + 1;
				triangles[i * 6 + 2] = i * 2 + 2;
				
				triangles[i * 6 + 3] = i * 2 + 2;
				triangles[i * 6 + 4] = i * 2 + 1;
				triangles[i * 6 + 5] = i * 2 + 3;
			}
		}

		if(mesh != null){
			if(trans != null){
				Vector3[] t_vertices = new Vector3[myVertices.Count];
				for(int i = 0; i< myVertices.Count; i++){
					t_vertices[i] = trans.InverseTransformPoint(myVertices[i]);
				}
				mesh.vertices = t_vertices;
			}else{
				mesh.vertices = myVertices.ToArray();
			}
			mesh.vertices = myVertices.ToArray();
			
			mesh.uv = uv;
			mesh.triangles = triangles;
			
			mesh.RecalculateNormals();
		}
	}
}
