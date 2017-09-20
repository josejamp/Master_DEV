using UnityEngine;
using System.Collections;

public class Arbol : MonoBehaviour {
    
    public GameObject applePrefab;

    public float vel = 5.0f;
    public float probCambio = 0.03f;
    public float tInicialManzanas= 1.0f;
    public float frecManzanas = 2.2f;
    public float extremos = 7.0f;

    private bool cambioSentido;

    // Use this for initialization
    void Start () {
        InvokeRepeating("tiraManzana", this.tInicialManzanas, this.frecManzanas);

        this.cambioSentido = false;
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = transform.position;
        pos.x += vel * Time.deltaTime;
        transform.position = pos;
        
        if (pos.x < -this.extremos || pos.x > this.extremos) {
            vel *= (-1);
            this.cambioSentido = true;
        }
        else
        {
            this.cambioSentido = false;
        }
    }

    void FixedUpdate(){
        if(!this.cambioSentido) vel *= (Random.value < probCambio) ? (-1) : 1;
    }

    private void tiraManzana(){
        GameObject instance = Instantiate(this.applePrefab) as GameObject;
        instance.transform.position = this.transform.position;
    }
}
