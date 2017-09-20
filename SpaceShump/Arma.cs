using UnityEngine;
using System.Collections;

public enum WeaponType {
    none, 
    blaster, 
    spread,
    shield
}

[System.Serializable]
public class Arma : MonoBehaviour {

    private WeaponType _type;
    private WeaponDefinition wd;

	private GameObject collar;

    private float timeUltDisp;

    public WeaponType type {
        get { return (_type); }
        set { SetType(value); }
    }

	void Awake(){
		this.SetType(WeaponType.none);
	}

    // Use this for initialization
    void Start () {
		collar =  this.transform.Find("Collar").gameObject;

		GameObject parentGO = transform.parent.gameObject;
		if (parentGO.tag == "Hero") {
			Hero.S.fireDelegate += Fire;
		}
		wd = Main.getWeaponDefinition(_type);
		collar.GetComponent<Renderer>().material.color = wd.color;
		timeUltDisp = 0;
	}
	
	// Update is called once per frame
	void Update () {

    }


    public void SetType(WeaponType wt) {
        _type = wt;
        if (type == WeaponType.none) {
            this.gameObject.SetActive(false);
            return;
        }
        else {
            this.gameObject.SetActive(true);
        }
    }

    public void Fire() {
        if (gameObject.activeInHierarchy && (Time.time - timeUltDisp > wd.delayBetweenShots) ){
			MakeProjectile().GetComponent<Rigidbody>().velocity = Vector3.up * wd.velocity;
			if(type.Equals(WeaponType.spread)){
				MakeProjectile().GetComponent<Rigidbody>().velocity = new Vector3 (-0.5f, 0.9f, 0) * wd.velocity;
				MakeProjectile().GetComponent<Rigidbody>().velocity = new Vector3 (0.5f, 0.9f, 0) * wd.velocity;
            }
        }

    }

    public GameObject MakeProjectile() {
        GameObject proj = Instantiate(wd.projectilePrefab) as GameObject;
		proj.transform.position = collar.transform.position;
        //proj.transform.parent = collar.transform;
        proj.GetComponent<Disparo>().setType(this._type);
        timeUltDisp = Time.time;
        return proj;
    }
}
