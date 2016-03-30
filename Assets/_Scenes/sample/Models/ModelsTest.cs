using UnityEngine;
using System.Collections;

public class ModelsTest : MonoBehaviour {
	
	public Models models;
	public Mesh _mesh;

	// Use this for initialization
	void Start () {
		
	}
	
	public float rotAngle = 0.0f;
	public Vector3 pos = Vector3.zero;
	public Vector3 scale = Vector3.one;

	// Update is called once per frame
	void Update () {
		_mesh = (Mesh)(this.gameObject.GetComponent<MeshFilter>().mesh);
		
		models = new Models();
		models.Begin(_mesh);

		Quaternion rot = Quaternion.Euler(rotAngle, 0, 0);
		Matrix4x4 mtx = Matrix4x4.TRS(pos, rot, scale);
		models.CreateCubeMesh(mtx);
		
		models.CreateCubeMesh(Vector3.zero, Vector3.one);
		models.CreateTriMesh(new Vector3(1,1,1), Vector3.one);
		models.CreateTriMesh(new Vector3(1,2,1), Vector3.one*1.5f);
		
		models.End();
	}
}
