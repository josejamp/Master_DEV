using UnityEngine;
using System.Collections;

public class Disparo : MonoBehaviour {

	public Bounds bounds;

    private WeaponType _type;

    // Use this for initialization
    void Start () {
		bounds = Utils.CombineBoundsOfChildren(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
	
		this.bounds.center = transform.position;
		Vector3 dentro = Utils.ScreenBoundsCheck(this.bounds, BoundsTest.onScreen);
		if ( dentro != Vector3.zero ) {
			Destroy (this.gameObject);
		}

	}

    public WeaponType getType() {
        return _type;
    }

    public void setType(WeaponType type) {
        _type = type;
    }
}
