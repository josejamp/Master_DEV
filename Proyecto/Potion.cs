using UnityEngine;
using System.Collections;

/*
 * Enumerado con los ditintos tipos de pocion:
 * para recuperar salud o ganar puntos de experiencia
 */
public enum PotionType {
    none,
    health,
    exp
}

/*
 * Clase para representar las pociones. Objetos que
 * se usan para recuperar salud o ganar experiencia.
 * Hereda de Item
 */
public class Potion : Item {

    /*
     * La pocion debe ser de un tipo en concreto. Y debe
     * recuperar una cierta cantidad de salud o experiencia
     */
    public PotionType type;
    public int boost;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /*
     * Al usar la pocion, el jugador gana una cierta
     * cantidad de salud o de experiencia, dependiendo
     * del tipo de la pocion
     */
    public override void Use<T>(T component) {
        Player consumer = component as Player;
        switch (this.type) {
            case PotionType.health: {
                    consumer.gainHealth(this.boost);
                } break;
            case PotionType.exp: {
                    consumer.gainExp(this.boost);
                } break;
            default: break;
        }
    }

    /*
     * La pocion se consume al ser usada
     */
    public override bool UnlimitedUses() {
        return false;
    }

    public void Copy(Potion item) {
        base.Copy(item);
        this.type = item.type;
        this.boost = item.boost;
    }

}
