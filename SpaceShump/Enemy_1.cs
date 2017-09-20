using UnityEngine;
using System.Collections;

public class Enemy_1 : Enemy{

    public Vector3[] points;
    private float birthTime;
    public float lifeTime = 10;

    public float sinEccentricity = 0.4f;

    // Use this for initialization
    void Start () {

        birthTime = Time.time;
        
        points = new Vector3[2];
        Vector3 cbMin = Utils.camBounds.min;
        Vector3 cbMax = Utils.camBounds.max;
        

        points[0] = new Vector3(cbMin.x - Main.S.tabSpawnEnem, Random.Range(cbMin.y, cbMax.y), 0);
        points[1] = new Vector3(cbMax.x + Main.S.tabSpawnEnem, Random.Range(cbMin.y, cbMax.y), 0);

    }
	

    public override void Move() {
        float u = (Time.time - birthTime) / lifeTime;
        if (u > 1) {
            Destroy(this.gameObject);
            return;
        }
        u = u + sinEccentricity * (Mathf.Sin(u * Mathf.PI * 2));
        transform.position = (1 - u) * points[0] + u * points[1];
    }
}
