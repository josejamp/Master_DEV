using UnityEngine;
using System.Collections;

/*
 * Clase para representar los proyectiles lanzados
 * en linea recta que causan daño al enemigo. Pueden
 * ser armas arrojadizas o proyectiles magicos.
 */
public class Projectile : MonoBehaviour {

    /*
     * Daño que realiza el proyectil
     */
    public int damage;

    /*
     * Lanzador del proeyctil, se usa
     * para darle experiencia si derrota
     * a un enemigo
     */
    private Player thrower;

    /*
     * Velocidad del proyectil, no tiene efecto
     * en el combate, ya que nos ea ctua hasta que la
     * animacion acabe
     */
    public float speed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /*
     * Si el objeto entra en un trigger con un enemigo, le hace daño.
     * Si entra en un trigger con un objeto sigue su trayectoria (los
     * objetos estan en el suelo). Si colisiona con otra cosa para su
     * trayectoria
     */
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Enemy") {
            Enemy hit = other.GetComponent<Enemy>();
            hit.beAttacked(this.thrower.stats, this.damage);
            this.thrower.gainExp(0); // Cero para solo actualizar el UI
            this.thrower.waitUseFinished();
            GameManager.Instance.itemAnimation = false;
            Destroy(this.gameObject);
        }
        else if (other.tag == "Item") {
            // Nothing
        }
        else {
            this.thrower.waitUseFinished();
            GameManager.Instance.itemAnimation = false;
            Destroy(this.gameObject);
        }
    }

    public void setThrower(Player thrower) {
        this.thrower = thrower;
    }

    public Player getThrower() {
        return this.thrower;
    }
}
