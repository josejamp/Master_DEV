using UnityEngine;
using System.Collections;

public class Tirachinas : MonoBehaviour {

    private static string LAUNCH_POINT_NAME = "LaunchPoint";

    private GameObject launchPoint;
    private GameObject proyectil;
    private bool proyExiste;
    private bool lanzado;

    public GameObject proyectilPrefab;


	// Use this for initialization
	void Start () {
        this.launchPoint = this.transform.FindChild(Tirachinas.LAUNCH_POINT_NAME).gameObject;

        Component h = this.launchPoint.GetComponent("Halo");
        h.GetType().GetProperty("enabled").SetValue(h, false, null);

        proyExiste = false;
        FollowCam.S.setLanzado(false);
        Linea.S.setLanzado(false);
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButton(0)){
            if (proyExiste) {
                Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));


                float radioMax = this.GetComponent<SphereCollider>().radius;
                Vector3 center = this.transform.TransformPoint(this.GetComponent<SphereCollider>().center);
                Vector3 diff = mousePos3D - center;
                float dist = diff.magnitude;
                if (dist > radioMax) {
                    mousePos3D = center + (diff / dist) * radioMax;
                }


                Vector3 pos = this.transform.position;
                pos.x = mousePos3D.x;
                pos.y = mousePos3D.y;
                this.proyectil.transform.position = pos;
            }
        }


        if (Input.GetMouseButtonUp(0)){
            if (proyExiste) {
                this.proyectil.GetComponent<Rigidbody>().isKinematic = false;
                Vector3 v = (this.proyectil.transform.position - this.transform.TransformPoint(this.GetComponent<SphereCollider>().center)) * 13;
                this.proyectil.GetComponent<Rigidbody>().velocity = -v;
                FollowCam.S.setLanzado(true);
                Linea.S.setLanzado(true);
                Linea.S.setNuevo(true);

                FollowCam.S.poi = this.proyectil;
                Linea.S.poi = this.proyectil;
                proyExiste = false;

                General.S.shots++;
            }
        }

	}

    void OnMouseOver(){
        Component h = this.launchPoint.GetComponent("Halo");
        h.GetType().GetProperty("enabled").SetValue(h, true, null);
    }

    void OnMouseExit(){
        Component h = this.launchPoint.GetComponent("Halo");
        h.GetType().GetProperty("enabled").SetValue(h, false, null);
    }

    void OnMouseDown() {
        if (!proyExiste) {
            this.proyectil = Instantiate(this.proyectilPrefab) as GameObject;
            this.proyectil.transform.parent = this.transform;
            this.proyExiste = true;
            this.proyectil.GetComponent<Rigidbody>().isKinematic = true;

            FollowCam.S.setLanzado(false);
            Linea.S.setLanzado(false);
        }

    }
}
