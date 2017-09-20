using UnityEngine;
using System.Collections;

/*
 * La clase Food representa los objetos que se
 * puden usar para recuperar el indicador de
 * comida.
 * Hereda de Item
 */
public class Food : Item {

    /*
     * Cantidad de comida que recupera el objeto
     */
    public int regain;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /*
     * Al usar este tipo de objeto el jugador recupera
     * una cierta cantidad de comida
     */
    public override void Use<T>(T component){
        Player consumer = component as Player;
        consumer.eatFood(this.regain);
    }

    /*
     * La comida se consume al usarla
     */
    public override bool UnlimitedUses(){
        return false;
    }

    public void Copy(Food item) {
        base.Copy(item);
        this.regain = item.regain;
    }

}
