using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour {

    public WeaponType _type;
    public Bounds bounds;

    private GameObject cubo;
    private TextMesh letra;

    float min = 10.0f, max = 100.0f;
    private Vector3 rotPerSecond;

    void Awake() {
        letra = GetComponent<TextMesh>();
        cubo = this.transform.Find("Cube").gameObject;
    }
    
    // Use this for initialization
    void Start () {

        rotPerSecond = new Vector3(Random.Range(min, max),
                           Random.Range(min, max),
                           Random.Range(min, max));


        Vector3 vel = new Vector3(0.0f, -15.0f, 0.0f); 
        GetComponent<Rigidbody>().velocity = vel;

    }
	
	// Update is called once per frame
	void Update () {
        cubo.transform.rotation = Quaternion.Euler(rotPerSecond * Time.time);

        this.bounds.center = transform.position;
        Vector3 dentro = Utils.ScreenBoundsCheck(this.bounds, BoundsTest.onScreen);
        if (dentro != Vector3.zero) {
            Destroy(this.gameObject);
        }
    }

    public void setType(WeaponType wt) {
        WeaponDefinition def = Main.getWeaponDefinition(wt);
        _type = wt;
        cubo.GetComponent<MeshRenderer>().materials[0].color = def.color;
        letra.text = def.letter; 
    }
}
