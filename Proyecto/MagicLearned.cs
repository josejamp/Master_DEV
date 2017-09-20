using UnityEngine;
using System.Collections;

/*
 * Clase que actua como un contenedor de las magias
 * que conoce el jugador
 */
public class MagicLearned : MonoBehaviour {

    public int magicCapacity;

    public Magic[] magics;
    private int num;

    /*
     * El jugador conoce dos mahias, que se asignan
     * en el editor de Unity
     */ 
    void Start() {
        //this.magics = new Magic[this.magicCapacity];
        num = 2;
    }

    // Update is called once per frame
    void Update() {

    }

    /*
     * Incluye una magia en la lista de magias conocidas
     */
    public void Add(Magic it) {
        this.magics[num] = it;
        num++;
    }

    /*
     * Un componente usa una determinada magia con varios objetivos
     */
    public void Use<T,O>(int pos, T component, O[] objectives) where T : Component {
        this.magics[pos].Use<T,O>(component, objectives);
    }

    /*
     * Devuelve el numero de magias conocidas
     */
    public int GetCurrentNumMagics() {
        return this.num;
    }

    /*
     * Copia el objeto en el que llega como parametro y transforma
     * el padre de las magias en el GameObject parent
     */
    public void Copy(MagicLearned magics, GameObject parent) {
        this.magicCapacity = magics.magicCapacity;
        this.magics = new Magic[this.magicCapacity];
        this.num = 0;
        for (int i = 0; i < magics.num; i++) {
            this.magics[num] = Instantiate(magics.magics[num]);
            this.magics[num].transform.parent = parent.transform;
            this.magics[num].Copy(magics.magics[num]);
            this.num++;
        }
    }
}
