using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour {

    static public FollowCam S;

    public bool ______________;

    public GameObject poi;
    public float camZ;

    public float speed = 1.0F;
    private float startTime;
    private bool lanzado;

	// Use this for initialization
	void Start () {
        S = this;
        camZ = this.transform.position.z;

        startTime = Time.time;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        Vector3 dest;

        if (poi == null) {
            dest = new Vector3(0,0,-10);
        }
        else {

            if (poi.tag.Equals("Proyectil") && (poi.GetComponent<Rigidbody>().IsSleeping() || (poi.transform.position.x <= -13 || poi.transform.position.y <= -10)) && this.lanzado) {
                poi = null;
                return;
            }


            float distCovered = (Time.time - startTime) * speed;
            dest = Vector3.Lerp(this.transform.position, poi.transform.position, 0.05F);
            dest.z = camZ;

        }

        transform.position = dest;

        this.GetComponent<Camera>().orthographicSize = dest.y + 10;
	}

    public void setLanzado(bool lanzado) {
        this.lanzado = lanzado;
    }
}
