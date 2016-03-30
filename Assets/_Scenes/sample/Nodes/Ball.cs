using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {
	public float _radius;
    //public Color _color = Color.blue;
    public float _alpha = 0.3f;

    public float mass = 25;
    
    public float vx = 0;
    public float vy = 0;
	public float vz = 0;
	
	public float x = 0;
	public float y = 0;
	public float z = 0;
    
    public Ball(float radius) {
		//Color color = Color.blue;
		float alpha = 0.3f;
        this._radius = radius;
        //this._color = color;
        this._alpha = alpha;
        this.mass = radius * 5.0f;
            
        init();
    }
        
    private void init() {
        
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.localPosition = new Vector3(x,y,z);
		this.transform.localScale = Vector3.one * mass*0.15f;
	}
}
