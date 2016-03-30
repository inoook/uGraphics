using UnityEngine;
using System.Collections;


[RequireComponent (typeof (MeshFilter))]
[RequireComponent (typeof (MeshRenderer))]

public class DrawCircleMesh : MonoBehaviour {
	
	private MeshFilter mf;
	private Mesh mesh;
	
	public int count = 16;
	public float per = 1.0f;
	public float r = 2.5f;
	
	public float startAngle = 0.0f;
	
	public bool isInvert = false;
	
	public bool debug = false;
	
	// Use this for initialization
	void Start () {
		mf = GetComponent<MeshFilter>() as MeshFilter;
		mesh = new Mesh();
		
	}
	
	// Update is called once per frame
	void Update () {
		
		per = Mathf.Clamp01(per);
		
		if(count <= 3){
			count = 3;
		}
		// vertices
		float d = Mathf.PI * 2 / (float)count;
		int num = (int)(count * per);
		int t_num = num;
		if(per >= 1.0f){
			num +=1;
		}else{
			num +=2;
		}
		
		Vector3[] vertices = new Vector3[num];
		
		vertices[0] = new Vector3(0,0,0);
		Debug.DrawRay(Vector3.zero, vertices[0]);
		float offsetAngle = Mathf.PI * 2 * (startAngle/360.0f);
		for (int i = 1; i < num ; i++){
			float x = r * Mathf.Cos(d * (i-1) + offsetAngle);
            float y = r * Mathf.Sin(d * (i-1) + offsetAngle);
			x *= isInvert ? -1 : 1;
			vertices[i] = new Vector3(x,y,0);
		}
		
		
		// triangle
		int[] tri = new int[(num)*3];
		int a = isInvert ? 2 : 1;
		int b = isInvert ? 1 : 2;
		for (int i = 0; i < (t_num) ; i++){
			tri[i*3] = 0;
			tri[i*3+a] = i+1;
			
			if(i >= (t_num)-1 && per == 1.0f){
				tri[i*3+b] = 1;
			}else{
				tri[i*3+b] = i+2;
			}
		}
		
		// normal
		Vector3[] normals = new Vector3[vertices.Length];
		for (int i = 0; i < normals.Length ; i++){
			normals[i] = new Vector3(0,0,1);
		}
		
		// uvs
		Vector2[] uvs = new Vector2[vertices.Length];
		for (int i = 0; i < uvs.Length ; i++){
			Vector3 vec = vertices[i];
			vec = vec.normalized;
			uvs[i] = new Vector2(vec.x/2 + 0.5f, vec.y/2 + 0.5f);
		}
		
		//Debug.Log(vertices.Length);
		if(vertices.Length > 0){
			mesh.Clear();
			mesh.vertices = vertices;
			mesh.triangles = tri;
			mesh.normals = normals;
			mesh.uv = uvs;
			
			mesh.RecalculateNormals();
			
			mf.mesh = mesh;
		}
		if(debug){
			for (int i = 0; i < vertices.Length; i++){
				Debug.DrawRay(Vector3.zero, transform.rotation * vertices[i]);
			}
		}
		
	}
	
	/*
	void circle(float originX, float originY, float r) {
	
        for (int i = 0; i < 360 * per; i++){
				
                float x = r * Mathf.Cos(i*Mathf.Deg2Rad) + originX;
                float y = r * Mathf.Sin(i*Mathf.Deg2Rad) + originY;
                GL.Vertex3(x,y,originZ);

                x = r * Mathf.Cos(i*Mathf.Deg2Rad+Mathf.Deg2Rad) + originX;
                y = r * Mathf.Sin(i*Mathf.Deg2Rad+Mathf.Deg2Rad) + originY;
                GL.Vertex3(x,y,originZ);
                
				GL.Vertex3(originX,originY,originZ);
        }
	
	}
	*/
}
