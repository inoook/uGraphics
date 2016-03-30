using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LinePolygon {
	
	private enum LineCMD
	{
		MOVETO, LINETO
	}

	private class ProtPoint{
		public LinePolygon.LineCMD cmd;
		public Vector3 point;
		public Quaternion rotation;
		public float thickness;
	}

	private Vector3 myVector;
	private Vector3 myPreVector;
	
	public float thicknessStart = 2;
	public float thickness = 2;

	private Mesh mesh;
	private Transform trans;
	//private ArrayList myVertices;
	private List<Vector3> myVertices;
	
	private List<LineCMD> cmd;
	private List<Vector3> points;
	private List<float> angleList;
	private List<float> thicknessList;
	private List<bool> isBillboardList;

	int sections = 0;
	int sections_all = 0;
	
	public float angleZ = 0.0f;

	public LinePolygon(Mesh mesh_, Transform trans_){
		mesh = mesh_;
		trans = trans_;
		myVertices = new List<Vector3>();
	}
	public LinePolygon(Mesh mesh_){
		mesh = mesh_;
		myVertices = new List<Vector3>();
	}
	
	public void Start(){
		Clear();

		sections_all = 0;
	}
	public void End()
	{
		UpdateLines();
	}

	void Init(Vector3 v)
	{
		sections = 0;
		sections ++;
		
		sections_all ++;
	}
	
	public void Clear()
	{
		if(mesh != null){
			mesh.Clear();
		}
		if(cmd != null){
			cmd.Clear();
		}
		if(points != null){
			points.Clear();
		}
		if(thicknessList != null){
			thicknessList.Clear();
		}
		if(angleList != null){
			angleList.Clear();
		}
		if(isBillboardList != null){
			isBillboardList.Clear();
		}
		
		if(myVertices != null){
			myVertices.Clear();
		}
		//myVertices = new ArrayList();
		myVertices = new List<Vector3>();
		cmd = new List<LineCMD>();
		points = new List<Vector3>();
		thicknessList = new List<float>();
		angleList = new List<float>();

		isBillboardList = new List<bool>();
	}
	
	public void MoveTo(Vector3 toVector3)
	{
		MoveTo(toVector3, thickness, angleZ);
	}
	public void MoveTo(Vector3 toVector3, float thickness_)
	{
		MoveTo(toVector3, thickness_, angleZ);
	}
	public void MoveTo(Vector3 toVector3, float thickness_, bool isBillboard)
	{
		MoveTo(toVector3, thickness_, angleZ, isBillboard);
	}
	public void MoveTo(Vector3 toVector3, float thickness_, float rotateZ, bool isBillboard = false)
	{
		Init(toVector3);
		
		cmd.Add(LineCMD.MOVETO);
		points.Add(toVector3);
		thicknessList.Add(thickness_);

		isBillboardList.Add(isBillboard);

		thickness = thickness_;
		thicknessStart = thickness_;
		angleZ = rotateZ;
		
		angleList.Add(angleZ);

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
		LineTo(toVector3, thickness_, angleZ, isBillboard);
	}
	public void LineTo(Vector3 toVector3, float thickness_, float rotateZ, bool isBillboard)
	{
		sections ++;
		sections_all ++;
		
		thickness = thickness_;
		angleZ = rotateZ;
		
		cmd.Add(LineCMD.LINETO);
		points.Add(toVector3);
		thicknessList.Add(thickness_);

		isBillboardList.Add(isBillboard);


		angleList.Add(angleZ);
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
	
	private Vector3 CrossUnitVectorBillboard(Vector3 vec)
	{
		Vector3 forward = Camera.main.transform.forward;
		Vector3 v = Vector3.Cross(vec, forward);
		
		v.Normalize();
		
		return v;
	}
	private Vector3 CrossUnitVectorBillboard2(Vector3 dir, Vector3 pos)
	{
		Vector3 forward = Camera.main.transform.position - pos;
		Quaternion q = Quaternion.LookRotation(forward, -dir);
		Vector3 v = q * Vector3.right;
		v.Normalize();

		return v;
	}

	//public bool isBillboard = false;

	// update Mesh
	private void UpdateLines(){
		myVertices = new List<Vector3>();

		Vector3 myVector; 
		Vector3 preVector3 = points[0];

		for(int i = 0; i < points.Count; i++){
			LineCMD c = cmd[i];

			if(c == LineCMD.MOVETO){
				preVector3 = points[i];
			}

			Vector3 toVector3 = points[i];
			float thickness = thicknessList[i];
			float angleZ = angleList[i];

			myVector = toVector3 - preVector3;

			if(i < cmd.Count-1){
				if(cmd[i+1] == LineCMD.MOVETO){
					myVector = myPreVector;
				}else{
					Vector3 nextVector3 = points[i+1];
					myVector = nextVector3 - toVector3;
				}
			}else{
				if(points.Count > 2){
					myVector = myPreVector;
					myPreVector = points[i-1] - points[i-2];
				}else{
					myVector = points[1] - points[0];
				}
			}
			
			Vector3 crossUnit;
			bool isBillboard = isBillboardList[i];
			
			if(c == LineCMD.MOVETO){
				Vector2 t_myVector = points[i+1] - points[i];
				if(isBillboard){
					//crossUnit = CrossUnitVectorBillboard2(t_myVector, points[i+1]);
					//crossUnit = Vector3.right;
					crossUnit = CrossUnitVectorBillboard2(myVector, points[i]);
				}else{
					crossUnit = CrossUnitVector(t_myVector, angleZ);
				}

				crossUnit *= thickness/2;

				Vector3 p_vecA = new Vector3( preVector3.x + crossUnit.x, preVector3.y + crossUnit.y, preVector3.z + crossUnit.z);
				Vector3 p_vecB = new Vector3( preVector3.x - crossUnit.x, preVector3.y - crossUnit.y, preVector3.z - crossUnit.z);
				myVertices.Add( p_vecA );
				myVertices.Add( p_vecB );
			}
			if(c == LineCMD.LINETO){
				if(isBillboard){
					crossUnit = CrossUnitVectorBillboard2(myVector, points[i]);
				}else{
					crossUnit = CrossUnitVector(myVector, angleZ);
				}
				
				crossUnit *= thickness/2;

				Vector3 vecA = new Vector3( toVector3.x + crossUnit.x, toVector3.y + crossUnit.y, toVector3.z + crossUnit.z);
				Vector3 vecB = new Vector3( toVector3.x - crossUnit.x, toVector3.y - crossUnit.y, toVector3.z - crossUnit.z);
				
				myVertices.Add(vecA);
				myVertices.Add(vecB);
			}
			
			preVector3 = toVector3;
			myPreVector = myVector;
		}

		Vector2[] uv = new Vector2[sections_all * 2];
		for(int n = 0; n < sections_all; n++){
			float p = ((float)n/(float)sections_all);
			uv[n * 2 + 0] = new Vector2(1.0f*p, 1.0f);
			uv[n * 2 + 1] = new Vector2(1.0f*p, 0.0f);
		}
		
		int[] triangles = new int[(sections_all - 1) * 2 * 3];
		for (int i=0; i < triangles.Length / 6; i++)
		{
			if(cmd[i+1] != LineCMD.MOVETO){
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
			
			mesh.uv = uv;
			mesh.triangles = triangles;
			
			mesh.RecalculateNormals();
		}
	}
}
