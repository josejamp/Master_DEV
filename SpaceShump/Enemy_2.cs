using UnityEngine;
using System.Collections;

public class Enemy_2 : Enemy {

    public Vector3[] points;
    private float birthTime;
    public float lifeTime = 10;
    

    // Use this for initialization
    void Start () {
        points = new Vector3[3]; 

        float xMin = Utils.camBounds.min.x + Main.S.tabSpawnEnem;
        float xMax = Utils.camBounds.max.x - Main.S.tabSpawnEnem;

        points[0] = transform.position;
        points[1] = new Vector3(Random.Range(xMin, xMax), Random.Range(Utils.camBounds.min.y, 0), 0);
        points[2] = new Vector3(Random.Range(xMin, xMax), transform.position.y, 0);

        birthTime = Time.time;
    }
	

    public override void Move() {
        float u = (Time.time - birthTime) / lifeTime;
        if (u > 1) {
            Destroy(this.gameObject);
            return;
        }
        Vector3 p01, p12;
        p01 = (1 - u) * points[0] + u * points[1];
        p12 = (1 - u) * points[1] + u * points[2];
        transform.position = (1 - u) * p01 + u * p12;
    }
}
