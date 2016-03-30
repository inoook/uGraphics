using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LinePolygon3 {

	private enum LineCMD
	{
		MOVETO, LINETO
	}
	
	private class ProtPoint{
		public LineCMD cmd;
		public Vector3 point;
		public Quaternion rotation;
		public float thickness;
	}
	
	private Vector3 myVector;
	private Vector3 preLineDir;
	
	public float thicknessStart = 2;
	public float thickness = 2;
	
	private Mesh mesh;
	//private ArrayList myVertices;
	private List<Vector3> myVertices;
	
	private List<LineCMD> cmd;
	private List<Vector3> points;
	private List<float> angleList;
	private List<float> thicknessList;

	int sections_all = 0;
	
	public float angleZ = 0.0f;

	public Vector3 upDir = Vector3.up;
	
	public LinePolygon3(Mesh mesh_){
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
		
		if(myVertices != null){
			myVertices.Clear();
		}


		myVertices = new List<Vector3>();
		cmd = new List<LineCMD>();
		points = new List<Vector3>();
		thicknessList = new List<float>();
		angleList = new List<float>();
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
		
		cmd.Add(LineCMD.MOVETO);
		points.Add(toVector3);
		thicknessList.Add(thickness_);
		
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
		sections_all ++;
		
		thickness = thickness_;
		angleZ = rotateZ;
		
		cmd.Add(LineCMD.LINETO);
		points.Add(toVector3);
		thicknessList.Add(thickness_);

		angleList.Add(angleZ);
	}

	private Vector3 CrossUnitVector(Vector3 vec, float rotateZ)
	{
		Quaternion q = Quaternion.LookRotation(vec, Vector3.up);
		q *= Quaternion.AngleAxis(rotateZ, Vector3.forward);
		Vector3 v = q * Vector3.right;
		
		v.Normalize();
		return v;
	}

	private Vector3 CrossUnitVectorToDir(Vector3 forwardDir, Vector3 upDir)
	{
		Vector3 v = Vector3.Cross(upDir, forwardDir.normalized);
		v.Normalize();
		return v;
	}
	
//	private Vector3 CrossUnitVectorBillboard(Vector3 vec)
//	{
//		Vector3 forward = Camera.main.transform.forward;
//		Vector3 v = Vector3.Cross(vec, forward);
//		
//		v.Normalize();
//		
//		return v;
//	}

	// update Mesh
	private void UpdateLines(){

		myVertices = new List<Vector3>();
		
		Vector3 lineDir; 
		Vector3 fromPos = points[0];
		
		for(int i = 0; i < points.Count; i++){
			LineCMD c = cmd[i];
			
			if(c == LineCMD.MOVETO){
				fromPos = points[i];
			}
			
			Vector3 toPos = points[i];
			float thickness = thicknessList[i];
			float angleZ = angleList[i];
			
			lineDir = toPos - fromPos;
			
			if(i < cmd.Count-1){
				if(cmd[i+1] == LineCMD.MOVETO){
					lineDir = preLineDir;
				}else{
					Vector3 nextVector3 = points[i+1];
					lineDir = nextVector3 - toPos;
				}
			}else{
				// end
				if(points.Count > 2){
					lineDir = preLineDir;
					//preLineDir = points[i-1] - points[i-2];
				}else{
					lineDir = points[1] - points[0];
				}
			}
			
			Vector3 crossUnit;
			
			if(c == LineCMD.MOVETO){
				lineDir = points[i+1] - points[i];
				crossUnit = CrossUnitVectorToDir(lineDir, upDir);
				
				crossUnit *= thickness/2;
				
				Vector3 p_vecA = new Vector3( fromPos.x + crossUnit.x, fromPos.y + crossUnit.y, fromPos.z + crossUnit.z);
				Vector3 p_vecB = new Vector3( fromPos.x - crossUnit.x, fromPos.y - crossUnit.y, fromPos.z - crossUnit.z);
				myVertices.Add( p_vecA );
				myVertices.Add( p_vecB );
			}
			if(c == LineCMD.LINETO){
				Vector3 crossUnit0 = CrossUnitVectorToDir(lineDir, upDir);
				Vector3 crossUnit1 = CrossUnitVectorToDir(preLineDir, upDir);

				crossUnit = (crossUnit0 + crossUnit1) * 0.5f;// preLineDir and lineDir avrg

				crossUnit *= thickness/2;
				
				Vector3 vecA = new Vector3( toPos.x + crossUnit.x, toPos.y + crossUnit.y, toPos.z + crossUnit.z);
				Vector3 vecB = new Vector3( toPos.x - crossUnit.x, toPos.y - crossUnit.y, toPos.z - crossUnit.z);
				
				myVertices.Add(vecA);
				myVertices.Add(vecB);
			}
			
			fromPos = toPos;
			preLineDir = lineDir;
		}

		// UV
		Vector2[] uv = new Vector2[sections_all * 2];
		for(int n = 0; n < sections_all; n++){
			float p = ((float)n/(float)sections_all);
			uv[n * 2 + 0] = new Vector2(1.0f*p, 1.0f);
			uv[n * 2 + 1] = new Vector2(1.0f*p, 0.0f);
		}

		// trinangle
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
			mesh.vertices = myVertices.ToArray();
			
			mesh.uv = uv;
			mesh.triangles = triangles;
			
			mesh.RecalculateNormals();
		}
	}
}
