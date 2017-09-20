using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class WeaponDefinition {
    public WeaponType type = WeaponType.none;
    public string letter; 
    public Color color = Color.white;
    public GameObject projectilePrefab; 
    public Color projectileColor = Color.white;
    public float damageOnHit = 0;
    public float continuousDamage = 0; 
    public float delayBetweenShots = 0;
    public float velocity = 20;
}



public class Main : MonoBehaviour {

    static public Main S;

    static public Dictionary<WeaponType, WeaponDefinition> W_DEFS;
    public WeaponDefinition[] weaponDefinitions;

    public WeaponType[] powerUpFrequency = new WeaponType[] {
                                             WeaponType.blaster, WeaponType.blaster,
                                             WeaponType.spread,
                                             WeaponType.shield };

    public GameObject[] tipoEnem;

    public float tabSpawnEnem = 1.5f;
    public float frecSpawnEnem = 1.0f/0.5f;

    void Awake() {
        S = this;

        Utils.SetCameraBounds(this.GetComponent<Camera>());

        Invoke("SpawnEnemy", frecSpawnEnem);


        W_DEFS = new Dictionary<WeaponType, WeaponDefinition>();
        foreach (WeaponDefinition def in weaponDefinitions) {
            W_DEFS[def.type] = def;
        }
    }

    // Use this for initialization
    void Start () {

    }

    public void SpawnEnemy() {
        GameObject enem = Instantiate(tipoEnem[Random.Range(0, tipoEnem.Length)]) as GameObject;

        Vector3 pos;
        pos.x = Random.Range(Utils.camBounds.min.x + tabSpawnEnem, Utils.camBounds.max.x - tabSpawnEnem);
        pos.y = Utils.camBounds.max.y + tabSpawnEnem;
        pos.z = 0.0f;
        enem.transform.position = pos;

        Invoke("SpawnEnemy", frecSpawnEnem);
    }

    // Update is called once per frame
    void Update () {
	
	}

    public void Reinicia() {
        SceneManager.LoadScene("spaceShump");
    }

	static public WeaponDefinition getWeaponDefinition( WeaponType wt ) {
		return( W_DEFS.ContainsKey(wt)? W_DEFS[wt] : (new WeaponDefinition()) );
	}


}
