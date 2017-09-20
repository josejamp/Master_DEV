using UnityEngine;
using System.Collections;

public class Enemy_0 : Enemy {

	public float waveFrequency = 2;
	public float waveWidth = 4;

	private float x0 = -10;
	private float birthTime;

    // Use this for initialization
    void Start () {
		x0 = transform.position.x;
		birthTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 tr_aux = transform.position;
		float age = Time.time - birthTime;
		float theta = Mathf.PI * 2 * age / waveFrequency;
		float sin = Mathf.Sin(theta);
		tr_aux.x = x0 + waveWidth * sin;
		transform.position = tr_aux;
        
		this.transform.rotation = Quaternion.Euler(0, sin * 45, 0);

		base.Update();
    }
}
