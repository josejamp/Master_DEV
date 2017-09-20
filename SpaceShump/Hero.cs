using UnityEngine;
using System.Collections;

public class Hero : MonoBehaviour {

	public static Hero S;

	public float vel = 30;
	public Bounds bounds;
	public int numMaxArmas = 5;

    public Arma[] armas;
    private int libre;

    private GameObject ultimoColl;
    
    public delegate void WeaponFireDelegate();
    public WeaponFireDelegate fireDelegate;


	void Awake(){
		Hero.S = this;
	}

    // Use this for initialization
    void Start () {
		bounds = Utils.CombineBoundsOfChildren(this.gameObject);

		armas =  new Arma[numMaxArmas];
		for (int i = 0; i < numMaxArmas; i++) {
			armas[i] = this.transform.Find("Weapon_"+i).gameObject.GetComponent<Arma>();
		}

        foreach (Arma arma in armas) arma.SetType(WeaponType.none);
        libre = 0;
		armas[libre].SetType(WeaponType.blaster);
        libre++;
    }
	
	// Update is called once per frame
	void Update () {
		float xAxis = Input.GetAxis("Horizontal");
		float yAxis = Input.GetAxis("Vertical");

		Vector3 pos = transform.position;
		pos.x += xAxis * this.vel * Time.deltaTime;
		pos.y += yAxis * this.vel * Time.deltaTime;
		transform.position = pos;

		this.bounds.center = transform.position;
		Vector3 dentro = Utils.ScreenBoundsCheck(this.bounds, BoundsTest.onScreen);
		if ( dentro != Vector3.zero ) {
			pos -= dentro;
			transform.position = pos;
		}

		transform.rotation = Quaternion.Euler(yAxis*30,xAxis*(-45),0);
        
        if (Input.GetAxis("Jump") == 1 && fireDelegate != null) {
            fireDelegate();
        }
    }

    void OnTriggerEnter(Collider other) {
        GameObject coll = Utils.FindTaggedParent(other.gameObject);
        if (coll != null && coll != ultimoColl) {
            ultimoColl = coll;
            if (coll.tag == "Enemy") {
                Escudo.S.nivel--;
                Destroy(coll);
                if (Escudo.S.nivel < 0) {
                    Destroy(this.gameObject);
                    Main.S.Reinicia();
                }
            }
            else if (coll.tag == "PowerUp") {
                PowerUp pu = coll.GetComponent<PowerUp>();
                if (pu._type.Equals(WeaponType.shield)) {
                    if (Escudo.S.nivel < 4)
                        Escudo.S.nivel++;
                }
                else {
                    if (pu._type == armas[0].type) {
                        if (libre < numMaxArmas) {
                            Arma a = armas[libre];
                            libre++;
                            if (a != null) {
                                a.SetType(pu._type);
                            }
                        }
                    }
                    else {
                        foreach (Arma arma in armas) arma.SetType(WeaponType.none);
                        libre = 0;
                        armas[libre].SetType(pu._type);
                        libre++;
                    }
                }
                Destroy(coll);
            }
        }
    }
}
