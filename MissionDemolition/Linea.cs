using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Linea : MonoBehaviour {

    static public Linea S;

    public GameObject poi;
    public float camZ;

    public float speed = 1.0F;
    private float startTime;
    private bool lanzado;
    private bool nuevo;

    public LineRenderer line;
    public List<Vector3> points;

    // Use this for initialization
    void Start () {

        S = this;

        line = GetComponent<LineRenderer>();
        points = new List<Vector3>();

        startTime = Time.time;

        line.SetWidth(0, 0.5F);
    }

    // Update is called once per frame
    void FixedUpdate() {

        if (this.nuevo) {
            this.clear();
        }

        if (poi != null) {

            if (poi.GetComponent<Rigidbody>().IsSleeping() && this.lanzado) {
                poi = null;
                return;
            }

            this.mueve();
        }


    }

    public void setLanzado(bool lanzado) {
        this.lanzado = lanzado;
    }

    public void setNuevo(bool nuevo) {
        this.nuevo = nuevo;
    }

    public void clear() {
        transform.position = new Vector3(-12, -9, 0);
        line.SetVertexCount(2);
        line.SetPosition(0, Vector3.zero);
        line.SetPosition(1, Vector3.zero);
        this.points.Clear();
        this.nuevo = false;

        this.mueve();
    }

    public void reinicia() {
        poi = null;
    }

    private void mueve() {
        Vector3 dest;

        float distCovered = (Time.time - startTime) * speed;
        dest = Vector3.Lerp(this.transform.position, poi.transform.position, 0.1F);
        dest.z = camZ;

        transform.position = dest;

        this.points.Add(dest);
        this.line.SetVertexCount(points.Count);
        this.line.SetPosition(points.Count - 1, dest);
    }
}
