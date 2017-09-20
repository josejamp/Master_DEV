using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class General : MonoBehaviour {

    public string showing = "Slingshot";

    static public General S;

    public GameObject tirachinas;

    public GameObject[] castles;
    private int level;

    private GameObject castle;
    public Vector3 posCastle;

    public Text nivel;
    public Text punt;

    public int shots;

    // Use this for initialization
    void Start () {
        level = 0;
        S = this;
        shots = 0;

        castle = Instantiate(castles[level]) as GameObject;
        castle.transform.position = posCastle;
    }
	
	// Update is called once per frame
	void Update () {
        if (Objetivo.fin) {
            Linea.S.clear();
            Linea.S.reinicia();

            GameObject[] gos = GameObject.FindGameObjectsWithTag("Proyectil");
            foreach (GameObject pTemp in gos) {
                Destroy(pTemp);
            }
            Destroy(castle);

            SwitchView("Both");
            Invoke("Siguiente", 2f);
            Objetivo.fin = false;

            shots = 0;
        }

        punt.text = "Shots Taken: " + this.shots.ToString();
        nivel.text = "Level: " + (this.level+1).ToString();
    }

    void Siguiente() {
        level = (level+1)%castles.Length;
        castle = Instantiate(castles[level]) as GameObject;
        castle.transform.position = posCastle;
    }

    void OnGUI() {
        // Draw the GUI button for view switching at the top of the screen
        Rect buttonRect = new Rect( (Screen.width/2)-50, 10, 100, 24 );
         switch(showing) {
             case "Slingshot":
             if ( GUI.Button( buttonRect, "Show Castle" ) ) {
                SwitchView("Castle");
             }
             break;
             case "Castle":
             if ( GUI.Button( buttonRect, "Show Both" ) ) {
                SwitchView("Both");
             }
             break;
             case "Both":
             if ( GUI.Button( buttonRect, "Show Slingshot" ) ) {
                SwitchView( "Slingshot" );
             }
             break;
        }
    }

    static public void SwitchView(string eView) {
        S.showing = eView;
        switch (S.showing) {
            case "Slingshot":
                FollowCam.S.poi = S.tirachinas;
                break;
            case "Castle":
                FollowCam.S.poi = S.castle;
                break;
            case "Both":
                FollowCam.S.poi = GameObject.Find("ViewBoth");
                break;
        }
    }
}
