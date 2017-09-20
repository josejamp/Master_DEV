using UnityEngine;
using System.Collections;

public class Fondo : MonoBehaviour {
    
    public GameObject fondo1;
    public GameObject fondo2;

    public float vel = -30f;


    void Start() {
        fondo1.transform.position = new Vector3(0, 0, 5);
        fondo2.transform.position = new Vector3(0, 160, 5);
    }

    // Update is called once per frame
    void Update() {
        float desp;
        desp = Time.time * vel % 160;

        fondo1.transform.position = new Vector3(0, desp, 5);
        fondo2.transform.position  = (desp >= 0)? new Vector3(0, desp - 160, 5) : new Vector3(0, desp + 160, 5);
        
    }
}
