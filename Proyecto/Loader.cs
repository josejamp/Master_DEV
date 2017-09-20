using UnityEngine;
using System.Collections;

/*
 * Clase que se encarga de instanciar al GameManager y de fijar
 * la posicion de la camara al inicio del juego.
 * Basada en la clase con el mismo nombre del tutorial de Unity
 * Roguelike 2D
 */
public class Loader : MonoBehaviour {

    /*
     * Referencia al objeto Singketon GameManager
     */
    public GameObject gameManager;

    private Transform cameraPos;

    /*
     * Al inicio del juego se crea el singleton si no
     * esta ya creado
     */
    void Awake() {
        cameraPos = this.GetComponent<Transform>();
        if (GameManager.Instance == null) {
            Instantiate(gameManager);
        }

    }

    /*
     * Al comenzar el juego se coloca la camara sobtre la posicion dictada
     * por GameManager
     */
    void Start() {
        cameraPos.position = new Vector3(GameManager.cameraPos.x, cameraPos.position.y, GameManager.cameraPos.x);
    }

    // Update is called once per frame
    void Update() {
        
    }
}