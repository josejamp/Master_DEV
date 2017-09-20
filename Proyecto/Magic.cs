using UnityEngine;
using System.Collections;

/*
 * Enumerado con los distintos tipos de magia:
 * los proyectiles magicos, que se disparan en linea recta
 * y hacen daño al enemigo con el que chocan; y los conjuros,
 * que dañan a todos los enemigos
 */
public enum MagicType {
    none,
    magicProjectile,
    spell
}

/*
 * Clase que representa la magia que puede lanzar el jugador.
 * La magia tiene un coste de mana y hace un determinado daño al enemigo.
 */
public class Magic : MonoBehaviour {

    /*
     * Las magias tienen un nombre y opcionalmente
     * un identificador
     */
    public string nombre;
    public int id;

    /*
     * Las magias son de un tipo, cuestan un mana
     * y hacen una cierta cantidad de daño al enemigo
     */
    public MagicType type;
    public int manaCost;
    public int damage;

    /*
     * Los proyectiles magicos tienen un modelo cuando
     * se lanzan
     */
    public GameObject spellModel;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /*
     * Al usar una magia esta tiene un lanzador (component)
     * y unos objetivos. Dependiendo del tipo de magia que sea
     * se usaran unos parametros u otros.
     * Antes de usar la magia se comprueba si se tiene mana suficiente
     * para usarla, y al usarla se reduce el mana.
     */
    public void Use<T,O>(T component, O[] objectives) {
        Player consumer = component as Player;
        Enemy[] enemies = objectives as Enemy[];

        if (consumer.stats.actualMana - this.manaCost < 0) return;

        switch (this.type) {
            case MagicType.magicProjectile: {
                    magicProjectile(consumer);
                } break;
            case MagicType.spell: {
                    this.spell(consumer, enemies);
                } break;
            default: break;
        }
        consumer.spendMana(this.manaCost);
    }

    /*
     * Lanzar un proyectil magico es quivalente a lanzr un arma arrojadiza.
     * Se calcula su posicion inicial en funcion de la del jugador, se calcua
     * la direccion y el sentido del lanzamiento y se le aplica velocidad
     */
    private void magicProjectile(Player thrower) {
        GameObject instanceProj = Instantiate(this.spellModel) as GameObject;
        instanceProj.transform.position = thrower.transform.position;
        instanceProj.GetComponent<Projectile>().setThrower(thrower);
        instanceProj.GetComponent<Projectile>().damage = this.damage;
        Vector3 dir;
        switch (thrower.getDirection()) {
            case Direction.north: {
                    dir = new Vector3(0, 0, 1);
                }
                break;
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
            default: dir = new Vector3(0, 0, 1); break;
        }
        instanceProj.GetComponent<Rigidbody>().velocity = dir * this.spellModel.GetComponent<Projectile>().speed;
        GameManager.Instance.itemAnimation = true;
        thrower.waitForUse();
    }

    /*
     * Lanzar un conjuro hace daño a todos los enemigos
     */
    private void spell(Player thrower, Enemy[] enemies) {
        for(int i = 0; i < enemies.Length; i++) {
            enemies[i].beAttacked(thrower.stats, this.damage);
        }
        thrower.gainExp(0); // Cero para solo actualizar el UI
    }


    public void Copy(Magic magic) {
        this.nombre = magic.nombre;
        this.id = magic.id;
        this.manaCost = magic.manaCost;
        this.type = magic.type;
        this.damage = magic.damage;
        this.spellModel = magic.spellModel;
    }
}
