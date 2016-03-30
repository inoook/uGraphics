using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Models {
	
	private List<Vector3> vertices;
	private List<int> triangles;
	//private List<Vector3> normals;
	private List<Vector2> uvs;
	
	public Mesh mesh;
	
	public void Begin(Mesh _mesh)
	{
		mesh = _mesh;
		mesh.Clear();
		
		vertices = new List<Vector3>();
		triangles = new List<int>();
		//normals = new List<Vector3>();
		uvs = new List<Vector2>();
		
	}
	
	public void End()
	{
		mesh.vertices = vertices.ToArray();
		mesh.triangles = triangles.ToArray();
		mesh.uv = uvs.ToArray();
		
		mesh.RecalculateNormals();
	}
	
	private Vector3 pt0 = new Vector3(0.0f, -0.3f, -0.8f);
	private Vector3 pt1 = new Vector3(-0.7f, -0.3f, 0.4f);
	private Vector3 pt2 = new Vector3(0.7f, -0.3f, 0.4f);
	private Vector3 pt3 = new Vector3(0.0f, 0.9f, 0.0f);

	//
	public void CreateTriMesh(Vector3 pos, Vector3 scale)
	{
		Vector3 p0 = new Vector3(pos.x + pt0.x * scale.x, pos.y + pt0.y * scale.y, pos.z + pt0.z * scale.z);
		Vector3 p1 = new Vector3(pos.x + pt1.x * scale.x, pos.y + pt1.y * scale.y, pos.z + pt1.z * scale.z);
		Vector3 p2 = new Vector3(pos.x + pt2.x * scale.x, pos.y + pt2.y * scale.y, pos.z + pt2.z * scale.z);
		Vector3 p3 = new Vector3(pos.x + pt3.x * scale.x, pos.y + pt3.y * scale.y, pos.z + pt3.z * scale.z);
		
		Vector3[] vertices = new Vector3[4]{ p0, p1, p2, p3 };
		SetTriangleMesh(vertices);
	}
	public void CreateTriMesh(Matrix4x4 mtx)
	{
		/*
		Vector3 p0 = new Vector3(pos.x + pt0.x * scale.x, pos.y + pt0.y * scale.y, pos.z + pt0.z * scale.z);
		Vector3 p1 = new Vector3(pos.x + pt1.x * scale.x, pos.y + pt1.y * scale.y, pos.z + pt1.z * scale.z);
		Vector3 p2 = new Vector3(pos.x + pt2.x * scale.x, pos.y + pt2.y * scale.y, pos.z + pt2.z * scale.z);
		Vector3 p3 = new Vector3(pos.x + pt3.x * scale.x, pos.y + pt3.y * scale.y, pos.z + pt3.z * scale.z);
		*/
		Vector3[] vertices = new Vector3[4]{ pt0, pt1, pt2, pt3 };
		for(int i = 0; i < vertices.Length; i++){
			Vector3 pt = mtx.MultiplyPoint3x4(vertices[i]);
	        vertices[i] = pt;
		}
		SetTriangleMesh(vertices);
	}
	
	void SetTriangleMesh(Vector3[] _vertices)
	{
		vertices.AddRange( new List<Vector3>(_vertices) );
		
		Vector2 uv0 = new Vector2(0.5f, 1);
		Vector2 uv1 = new Vector2(1, 0);
		Vector2 uv2 = new Vector2(0, 0);
		Vector2 uv3 = new Vector2(0.5f,1.0f);
		uvs.AddRange(new List<Vector2>{ uv0, uv1, uv2, uv3 });
		
		int index = vertices.Count - 1;
		// bottom
		triangles.AddRange(new List<int>{ index-1, index-2, index-3 });
		// back
		triangles.AddRange(new List<int>{ index-2, index-1, index});
		
		// -- backface --
		triangles.AddRange(new List<int>{ index-3, index-2, index-1});
		triangles.AddRange(new List<int>{ index, index-1, index-2});
		
		//
		//vertices.AddRange(new List<Vector3> { p0, p1, p2, p3 });
		vertices.AddRange(new List<Vector3>( _vertices ) );
		
		uv3 = new Vector2(0, 0);
		uv0 = new Vector2(1, 0);
		uv1 = new Vector2(0.5f, 1.0f);
		uv2 = new Vector2(0.5f, 1.0f);
		uvs.AddRange(new List<Vector2>{ uv0, uv1, uv2, uv3} );
		
		index = vertices.Count - 1;
		
		// right
		triangles.AddRange(new List<int> { index-3, index-2, index});
		// left
		triangles.AddRange(new List<int> { index-1, index-3, index});
		
		// -- backface --
		triangles.AddRange(new List<int> { index, index-2, index-3});
		triangles.AddRange(new List<int> { index, index-3, index-1});
	}
	
	
	public void CreateCubeMesh(Vector3 pos, Vector3 scale)
	{
		MakeCube( vertices.Count, pos, scale);
	}
	
	public void CreateCubeMesh( Matrix4x4 mtx )
	{
		MakeCube( vertices.Count, mtx);
	}
	
	void MakeCube( int vIndex, Matrix4x4 mtx )
	{
		Vector3[] vertices = new Vector3[8];
		
		int vi = 0;
		Vector3 scale = Vector3.one;
		//basepos += Vector3.Scale(new Vector3(-0.5f,-0.5f,-0.5f), 1.0f);
		Vector3 basepos = -Vector3.one * 0.5f;
		vertices[vi+0] = basepos + (Vector3.Scale(new Vector3( 0,0,0 ), scale)); // Z=0
		vertices[vi+1] = basepos + (Vector3.Scale(new Vector3( 0,1,0 ), scale)); //
		vertices[vi+2] = basepos + (Vector3.Scale(new Vector3( 1,0,0 ), scale)); //
		vertices[vi+3] = basepos + (Vector3.Scale(new Vector3( 1,1,0 ), scale)); //
		
		vertices[vi+4] = basepos + (Vector3.Scale(new Vector3( 0,0,1 ), scale)); // Z=1
		vertices[vi+5] = basepos + (Vector3.Scale(new Vector3( 0,1,1 ), scale)); //
		vertices[vi+6] = basepos + (Vector3.Scale(new Vector3( 1,0,1 ), scale)); //
		vertices[vi+7] = basepos + (Vector3.Scale(new Vector3( 1,1,1 ), scale)); //
		
		for(int i = 0; i < vertices.Length; i++){
			Vector3 pt = mtx.MultiplyPoint3x4(vertices[i]);
	        vertices[i] = pt;
		}
		
		SetCubeMesh(vertices, vIndex);
		
	}
	
	void MakeCube( int vIndex, Vector3 basepos, Vector3 scale )
	{
		Vector3[] vertices = new Vector3[8];
		
		int vi = 0;
		basepos += Vector3.Scale(new Vector3(-0.5f,-0.5f,-0.5f), scale);
		vertices[vi+0] = basepos + (Vector3.Scale(new Vector3( 0,0,0 ), scale)); // Z=0
		vertices[vi+1] = basepos + (Vector3.Scale(new Vector3( 0,1,0 ), scale)); //
		vertices[vi+2] = basepos + (Vector3.Scale(new Vector3( 1,0,0 ), scale)); //
		vertices[vi+3] = basepos + (Vector3.Scale(new Vector3( 1,1,0 ), scale)); //
		
		vertices[vi+4] = basepos + (Vector3.Scale(new Vector3( 0,0,1 ), scale)); // Z=1
		vertices[vi+5] = basepos + (Vector3.Scale(new Vector3( 0,1,1 ), scale)); //
		vertices[vi+6] = basepos + (Vector3.Scale(new Vector3( 1,0,1 ), scale)); //
		vertices[vi+7] = basepos + (Vector3.Scale(new Vector3( 1,1,1 ), scale)); //
		
		SetCubeMesh(vertices, vIndex);
	}
	
	void SetCubeMesh(Vector3[] _vertices, int vIndex)
	{
		int vi = 0;
		int ti = 0;
		
		//Vector3[] _vertices = new Vector3[8];
		Vector2[] _uv = new Vector2[8];
		int[] _triangles = new int[36];
		
		_uv[vi+0] = new Vector2(0,0); // Z=0
		_uv[vi+1] = new Vector2(0,1);
		_uv[vi+2] = new Vector2(1,0);
		_uv[vi+3] = new Vector2(1,1);
		
		_uv[vi+4] = new Vector2(1,1); // Z=1
		_uv[vi+5] = new Vector2(1,0);
		_uv[vi+6] = new Vector2(0,0);
		_uv[vi+7] = new Vector2(0,1);
		
		_triangles[ti+0] = vIndex+0; // Z=0平面の三角0
		_triangles[ti+1] = vIndex+1; //
		_triangles[ti+2] = vIndex+2; //
		_triangles[ti+3] = vIndex+1; // Z=0平面の三角1
		_triangles[ti+4] = vIndex+3; //
		_triangles[ti+5] = vIndex+2; //
		
		_triangles[ti+6] = vIndex+4; // Z=1平面の三角0
		_triangles[ti+7] = vIndex+6;
		_triangles[ti+8] = vIndex+5;
		_triangles[ti+9] = vIndex+5; // Z=1平面の三角1
		_triangles[ti+10]= vIndex+6;
		_triangles[ti+11]= vIndex+7;
		
		_triangles[ti+12] = vIndex+5; // X=0平面の三角0
		_triangles[ti+13] = vIndex+1;
		_triangles[ti+14] = vIndex+0;
		_triangles[ti+15] = vIndex+0; // X=0平面の三角1
		_triangles[ti+16] = vIndex+4;
		_triangles[ti+17] = vIndex+5;
		
		_triangles[ti+18] = vIndex+2; // X=1平面の三角0
		_triangles[ti+19] = vIndex+3;
		_triangles[ti+20] = vIndex+6;
		_triangles[ti+21] = vIndex+3; // X=1平面の三角1
		_triangles[ti+22] = vIndex+7;
		_triangles[ti+23] = vIndex+6;
		
		_triangles[ti+24] = vIndex+0; // Y=0平面の三角0
		_triangles[ti+25] = vIndex+2;
		_triangles[ti+26] = vIndex+6;
		_triangles[ti+27] = vIndex+0; // Y=1平面の三角1
		_triangles[ti+28] = vIndex+6;
		_triangles[ti+29] = vIndex+4;
		
		_triangles[ti+30] = vIndex+1; // Y=1平面の三角0
		_triangles[ti+31] = vIndex+5;
		_triangles[ti+32] = vIndex+3;
		_triangles[ti+33] = vIndex+5; // Y=1平面の三角1
		_triangles[ti+34] = vIndex+7;
		_triangles[ti+35] = vIndex+3;
		
		
		vertices.AddRange(new List<Vector3>(_vertices));
		uvs.AddRange(new List<Vector2>(_uv));
		triangles.AddRange(new List<int>(_triangles));
	}
	
}
