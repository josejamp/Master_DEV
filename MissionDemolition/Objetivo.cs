using UnityEngine;
using System.Collections;

public class Objetivo : MonoBehaviour {

    public static bool fin = false;


    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag.Equals("Proyectil")) {
            Objetivo.fin = true;

            Color c = GetComponent<Renderer>().material.color;
            c.a = 1;
            GetComponent<Renderer>().material.color = c;
        }
    }
}
