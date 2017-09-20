using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * Clase que representa la mochila del jugador, se usa para
 * guardar los objetos que recoge y su dinero (en esta version no) 
 */
public class Bag : MonoBehaviour {

    public int inventorySize;

    public Item[] items;
    private int num;

    /*
     * El atributo dinero no se usa en la version actual
     */ 
    public int money;

    // Use this for initialization
    void Start () {

	}

    /*
     * El inventario se inicializa vacio
     * */
    public void Init() {
        this.items = new Item[this.inventorySize];
        num = 0;
        this.money = 0;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    /*
     * Se incluye un objeto en la primera posicion vacia
     */
    public void Add(Item it) {
        this.items[num] = it;
        this.num++;
    }

    /*
     * Al usar un obejto se llama al metodo Use del dicho objeto,
     * despues se reproduce su sonido, y finalmente se elimina si no tiene
     * usos ilimitados
     */ 
    public void Use<T>(int pos, T component) where T : Component{
        this.items[pos].Use<T>(component);
        this.Sound<T>(pos,component);
        if (!this.items[pos].UnlimitedUses()){
            Destroy(items[pos]);
            this.Remove(pos);
        }
    }

    /*
     * Al reproducir un sonido se usa al jugador como fuente
     */ 
    public void Sound<T>(int pos, T component) {
        Player p = component as Player;
        p.audioSource.PlayOneShot(this.items[pos].sound);
    }

    /*
     * Eliminar un elemento del inventario requiere recorrer
     * las posiciones ocuapdas hasta que se encuentra el objeto a eliminar,
     * a partir de entonces se copian los objetos siguiente de la posicion i en la i-1
     */ 
    public void Remove(int pos){
        Item[] aux = new Item[this.inventorySize];
        for(int i = 0; i < pos; i++){
            aux[i] = Instantiate(this.items[i]);
        }
        int ultima = pos;
        for (int i = pos + 1; i < num; i++){
            aux[ultima] = Instantiate(this.items[i]);
            ultima++;
        }
        this.items = aux;
        this.num--;
    }

    /*
     * Devuelve el numero de objetos en la mochila
     */
    public int GetCurrentNumItems() {
        return this.num;
    }

    /*
     * Comprueba si la mochila esta llena
     */ 
    public bool Full() {
        return this.num >= this.inventorySize;
    }
    
    /*
     * Metodo que dada una mochila y un GameObject, copia
     * los valores de esta mochila en la dada, y transforma
     * a los objetos de la mochila en hijos del GameObject
     */
    public void Copy(Bag bag, GameObject parent) {
        this.inventorySize = bag.inventorySize;
        this.items = new Item[this.inventorySize];
        this.num = 0;
        this.money = 0;
        for (int i = 0; i < bag.num; i++) {
            print(bag.items[i]);
            this.items[i] = Instantiate(bag.items[i]);
            this.items[i].transform.parent = parent.transform;
            this.items[i].Copy(bag.items[i]);
            //this.items[i].enabled = false;
            this.num++;
        }
    }

}
