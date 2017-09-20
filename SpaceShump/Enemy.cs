using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public float vel = 10f;
	public float hp = 10;

    public Bounds bounds;

    public float powerUpProb = 85.0f;
    public GameObject pPrefab;

    public int framesRojo = 10;
	private Color[] originalColors;
	private Material[] materials;
	private bool hurt;
    private bool generandoPU;
	private int framesRestantes;

	void Awake(){
        bounds = Utils.CombineBoundsOfChildren(this.gameObject);

        materials = Utils.GetAllMaterials(this.gameObject);
		framesRestantes = framesRojo;
		hurt = false;
        generandoPU = false;
		originalColors = new Color[materials.Length];
		for (int i=0; i < materials.Length; i++) {
			originalColors[i] = materials[i].color;
		}
	}

	// Use this for initialization
	void Start () {

    }
	
	protected void Update() {
		Move();

		if(hurt){
			framesRestantes--;
			if (framesRestantes == 0) {
				for (int i=0; i < materials.Length; i++) {
					materials[i].color = originalColors[i];
				}
				hurt = false;
				framesRestantes = framesRojo;
			}
		}

        this.bounds.center = transform.position;
        Vector3 dentro = Utils.ScreenBoundsCheck(this.bounds, BoundsTest.onScreen);
        if (dentro != Vector3.zero && dentro.y < 0) {
            Destroy(this.gameObject);
        }
    }

	public virtual void Move() {
		Vector3 tempPos = transform.position;
		tempPos.y -= vel * Time.deltaTime;
		transform.position = tempPos;
	}

	void OnCollisionEnter( Collision coll ) {
		GameObject other = coll.gameObject;
		if(other.tag == "ProjectileHero") {
			Disparo proj = other.GetComponent<Disparo>();

			foreach(Material m in materials) {
				m.color = Color.red;
			}
			hurt = true;
            
			hp -= Main.W_DEFS[proj.getType()].damageOnHit;
			if (hp <= 0) {
				if(!generandoPU) generaPowerUp();
				Destroy(this.gameObject);
			}
			Destroy(other);
		}
	}

    public void generaPowerUp() {

        generandoPU = true;
		if (Random.value <= powerUpProb) {
			WeaponType puType = Main.S.powerUpFrequency[Random.Range(0, Main.S.powerUpFrequency.Length)];
            
            GameObject powerUp= Instantiate( pPrefab) as GameObject;

			PowerUp script = powerUp.GetComponent<PowerUp>();
			script.setType(puType);
			script.transform.position = transform.position;
		}
		
    }
}
