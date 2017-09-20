using UnityEngine;
using System.Collections;

public class Manzana : MonoBehaviour {

    public static float limite = -10f;
    void Update()
    {
        if (transform.position.y < limite)
        {
            Destroy(this.gameObject);
            GameObject.Find("Cesta").GetComponent<Cesta>().destruyeCapa();

        }
    }
}
