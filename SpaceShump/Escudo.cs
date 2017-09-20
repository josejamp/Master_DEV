using UnityEngine;
using System.Collections;

public class Escudo : MonoBehaviour {

    public static Escudo S;

    public int nivel;

	// Use this for initialization
	void Start () {
        Escudo.S = this;
		this.nivel = 1;
    }
	
	// Update is called once per frame
	void Update () {
        Material mat = this.GetComponent<Renderer>().material;
        mat.mainTextureOffset = new Vector2(0.2f * this.nivel, 0);
        float rZ = ( 0.1f * Time.time*360) % 360f;
		transform.rotation = Quaternion.Euler( 0, 0, rZ );
	}
}
