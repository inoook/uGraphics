using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Nodes : MonoBehaviour {
	
	public int numParticles = 50;
	public float minDist = 100;
    private float springAmount =  .001f;
	
	public float stageWidth = 50;
	public float stageHeight = 50;
	
	private List<Ball> particles;
	
	public GameObject prefabBall;
	
	public GraphicLine graphic;
	
	public float lineMaxW = 1.0f;
	
	// Use this for initialization
	void Start () {
		
		particles = new List<Ball>();
		for (int i = 0; i < numParticles; i++) {
            float size = Random.value * 10 + 2;
			GameObject gObj = (GameObject)GameObject.Instantiate(prefabBall);
			gObj.transform.parent = this.transform;
			Ball particle = gObj.GetComponent<Ball>();
			
            particle.x = Random.value * stageWidth - stageWidth * 0.5f;
            particle.y = Random.value * stageHeight - stageHeight * 0.5f;
			particle.z = Random.value * stageHeight - stageHeight * 0.5f;
            particle.vx = Random.value * 2.0f - 1.0f;
            particle.vy = Random.value * 2.0f - 1.0f;
			particle.vz = Random.value * 2.0f - 1.0f;
            particle.mass = size;
            
            particles.Add(particle);
        }
		
	}
	
	public float amp = 1.0f;
	
	// Update is called once per frame
	void Update () {
		
		int lineIndex = 0;
		graphic.Begin();
		
		for (int i = 0; i < numParticles - 1; i++ ) {
            Ball partA = particles[i];
            for (int j = i + 1; j < numParticles; j++ ) {
                Ball partB = particles[j];
                springAndDrawLine(partA, partB, lineIndex);
				lineIndex ++;
            }
        }
		graphic.End();
		
		//Debug.Log(lineIndex);
		
		for (int i = 0; i < numParticles; i++) {
            Ball particle = particles[i];
            particle.x += particle.vx * Time.deltaTime * amp;
            particle.y += particle.vy * Time.deltaTime * amp;
			particle.z += particle.vz * Time.deltaTime * amp;
            border(particle);
        }
		  
	}
	
	void springAndDrawLine(Ball partA, Ball partB, int index) {
        float dx = partB.x - partA.x;
        float dy = partB.y - partA.y;
		float dz = partB.z - partA.z;
        float distSQ = dx * dx + dy * dy + dz * dz;
        float dist = Mathf.Sqrt(distSQ);
		
        if (dist < minDist) {
			
			float w = 1.0f - dist/minDist;
			
			graphic.DrawLine(partA.transform.position, partB.transform.position, w * lineMaxW);
			
            float ax = dx * springAmount * Time.deltaTime;
            float ay = dy * springAmount * Time.deltaTime;
			float az = dz * springAmount * Time.deltaTime;
            partA.vx += ax / partA.mass;
            partA.vy += ay / partA.mass;
			partA.vz += az / partA.mass;
            partB.vx -= ax / partB.mass;
            partB.vy -= ay / partB.mass;
			partB.vz -= az / partB.mass;
		}else{
			//line.ClearLine();
        }
        
    }
	
    void border(Ball particle) {
		float stageW_half = stageWidth * 0.5f;
		float stageH_half = stageHeight * 0.5f;
		/*
        if (particle.x > stageW_half) {
            particle.x = -stageW_half;
        }else if (particle.x < -stageW_half) {
            particle.x = stageW_half;    
        }
        if (particle.y > stageH_half) {
            particle.y = -stageH_half;
        }else if (particle.y < -stageH_half) {
            particle.y = stageH_half;    
        }
		if (particle.z > stageW_half) {
            particle.z = -stageW_half;
        }else if (particle.z < -stageW_half) {
            particle.z = stageW_half;    
        }
        */
		float v = -0.9f;
        if (particle.x > stageW_half) {
            particle.vx *= v;
			particle.x = stageW_half;
        }else if (particle.x < -stageW_half) {
            particle.vx *= v;  
			particle.x = -stageW_half;
        }
        if (particle.y > stageH_half) {
            particle.vy *= v;
			particle.y = stageH_half;
        }else if (particle.y < -stageH_half) {
            particle.vy *= v;
			particle.y = -stageH_half;
        }
		if (particle.z > stageW_half) {
            particle.vz *= v;
			particle.z = stageW_half;
        }else if (particle.z < -stageW_half) {
            particle.vz *= v;
			particle.z = -stageW_half;
        }
    }
}
