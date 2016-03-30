using UnityEngine;
using System.Collections;

public class NodesGUI : MonoBehaviour {
	
	public Nodes nodes;
	public GUISkin skin;
	
	public float rotSpeed = 0.0f;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		nodes.transform.Rotate(new Vector3(0,-rotSpeed*Time.deltaTime,0));
	}
	
	Rect windowRect = new Rect(10,10,200,105);
	void OnGUI()
	{
		GUI.skin = skin;
		windowRect = GUI.Window(0, windowRect, DoMyWindow, "param");
	}
	void DoMyWindow(int windowID) {
		GUILayoutExtra.labelWidth = 100;
        nodes.minDist = GUILayoutExtra.slider(nodes.minDist, "Distance", 0.1f, 14.0f);
		nodes.lineMaxW = GUILayoutExtra.slider(nodes.lineMaxW, "Thickness", 0.1f, 10.0f);
		nodes.amp = GUILayoutExtra.slider(nodes.amp, "Speed", 0.0f, 20.0f);
        
		rotSpeed = GUILayoutExtra.slider(rotSpeed, "Rotation v", -40.0f, 40.0f);
		
		GUI.DragWindow();
    }
}
