using UnityEngine;
using System.Collections;

/*
 * Clase abstracta para representar de manera generica
 * los objetos del juego
 */
public abstract class Item : MonoBehaviour {
    
    /*
     * Los objteos tienen un nombre y opcionalmente un id
     */ 
    public string nombre;
    public int id;

    /*
     * Los objteos emiten un sonido al ser usados 
     */
    public AudioClip sound;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /*
     * Metodo que debe implementarse para indicar lo que
     * hace cada tipo especifico de objeto al usarse
     */
    public abstract void Use<T>(T component)
        where T : Component;

    /*
     * Metodo abstracto que indica si el tipo de objeto
     * se peude usar de manera ilimitada
     */
    public abstract bool UnlimitedUses();

    /*
     * Copia en item el objeto this
     */
    public void Copy(Item item) {
        this.name = item.name;
        this.id = item.id;
    }


}
