using UnityEngine;
using System.Collections;

/*
 * Clase para representar los objetos que se pueden lanzar.
 * Estos objetos se lanzan en linea recta y se rompan
 * al chocar; si chocan contra un enemigo le hacen perder salud.
 * Hereda de Item
 */
public class Throwable : Item {

    /*
     * Salud que hace perder al enemigo con el que choca
     */
    public int damage;

    /*
     * Modelo del objeto que aparece cuando se lanza el objeto
     */
    public GameObject projectile;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /*
     * Al usar un objeto lanzable ha de fijarse su posicion inicial
     * usando la posicion del jugador, ha de fijarse la direccion que
     * seguira al lanzarse usando la direccion en la que mira el jugador,
     * asignarle una velocidad e instanciarlo como un GameObject Projectile
     */
    public override void Use<T>(T component) {
        Player thrower = component as Player;
        GameObject instanceProj = Instantiate(this.projectile) as GameObject;
        instanceProj.transform.position = thrower.transform.position;
        instanceProj.GetComponent<Projectile>().setThrower(thrower);
        instanceProj.GetComponent<Projectile>().damage = this.damage;
        Vector3 dir;
        switch (thrower.getDirection()) {
            case Direction.north: {
                    dir = new Vector3(0, 0, 1);
                } break;
            case Direction.east: {
                    dir = new Vector3(1, 0, 0);
                }
                break;
            case Direction.south: {
                    dir = new Vector3(0, 0, -1);
                }
                break;
            case Direction.west: {
                    dir = new Vector3(-1, 0, 0);
                }
                break;
            default: dir = new Vector3(0, 0, 1);  break;
        }
        instanceProj.GetComponent<Rigidbody>().velocity = dir * projectile.GetComponent<Projectile>().speed;
        GameManager.Instance.itemAnimation = true;
        thrower.waitForUse();
    }

    /*
     * Una vez se lanza el objeto este deja de formar parte
     * del inventario
     */
    public override bool UnlimitedUses() {
        return false;
    }

    public void Copy(Throwable item) {
        base.Copy(item);
        this.damage = item.damage;
        this.projectile = item.projectile;
    }

}
