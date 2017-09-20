using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

/*
 * Clase que encapsula la funcionalidad relativa
 * al menu central de la pantalla que aparece al pulsar Intro y
 * a los numeros flotantes cuando se realiza daño en combate
 */
public class Menu : MonoBehaviour {
    
    /*
     * Solo hay un Menu por lo que se usara un Singleton
     */
    public static Menu M = null;

    /*
     * Padre de los menus
     */
    public GameObject menu;

    /*
     * Menus correspondiente al menu principal, y las
     * pestañas con el inventario, las magias y la
     * informacion del jugador
     */
    private GameObject mainMenu;
    private GameObject playerMenu;
    private GameObject magicMenu;
    private GameObject inventoryMenu;

    /*
     * Cuadros de texto para mostrar la salud, el ataque y la
     * defensa del jugador
     */
    private Text playerHP;
    private Text playerAtt;
    private Text playerDef;

    /*
     * Arrays de botones para usar los objetos del inventario
     * y las magias
     */
    private Button[] playerInventory;
    private Button[] playerMagic;

    /*
     * Prefab para mostrar el daño realizado o sufrido durante
     * el combate
     */
    public GameObject damagePrefab;

    /*
     * Al despertar se crea la instancia Singleton si no esta ya creada,
     * y se inicializan los componentes con sus referencias
     */
    void Awake() {
        if (M == null) {
            M = this;
        }
        else if (M != this) {
            Destroy(gameObject);
        }

        this.mainMenu = this.menu.transform.Find("PanelMenuMain").gameObject;
        this.playerMenu = this.menu.transform.Find("PanelMenuEst").gameObject;
        this.magicMenu = this.menu.transform.Find("PanelMenuMagic").gameObject;
        this.inventoryMenu = this.menu.transform.Find("PanelMenuBag").gameObject;

        this.playerHP = this.playerMenu.transform.Find("HPInfo").GetComponent<Text>();
        this.playerAtt = this.playerMenu.transform.Find("AttInfo").GetComponent<Text>();
        this.playerDef = this.playerMenu.transform.Find("DefInfo").GetComponent<Text>();
    }

    // Use this for initialization
    void Start () {


    }

    /*
     * Metodo para inicializar el array de botones del inventario. Se
     * incluyen Listeners en cada boton para que al hacer clic
     * se use el objeto correspondiente del inventario del jugador
     */
    public void initInventoryMenu(Player player) {
        this.playerInventory = new Button[player.getBagSize()];
        for (int i = 0; i < 5; i++) {
            this.playerInventory[i] = this.inventoryMenu.transform.Find("Inv" + (i + 1)).GetComponent<Button>();
            int button = i;
            this.playerInventory[i].onClick.AddListener(() => player.UseItem(button));
            this.playerInventory[i].transform.Find("Text").GetComponent<Text>().text = "";
        }
    }

    /*
     * Metodo para inicializar el array de botones de la magia del jugador. Se
     * incluyen Listeners en cada boton para que al hacer clic
     * se use la magia correspondiente del jugador
     */
    public void initMagicMenu(Player player) {
        this.playerMagic = new Button[player.getNumMagicsLearned()];
        for (int i = 0; i < 2; i++) {
            this.playerMagic[i] = this.magicMenu.transform.Find("Mag" + (i + 1)).GetComponent<Button>();
            int button = i;
            this.playerMagic[i].onClick.AddListener(() => player.UseMagic(button));
            this.playerMagic[i].transform.Find("Text").GetComponent<Text>().text = "";
        }
    }

    // Update is called once per frame
    void Update () {
	
	}

    /*
     * Metodo que muestra la info del jugador. Activa los menus
     * correspondientes y desactiva los otros
     */
    public void ShowPlayerInfo(CharacterStats info) {
        this.mainMenu.SetActive(false);
        this.playerMenu.SetActive(true);

        this.playerHP.text = "Max HP: " + info.hp;
        this.playerAtt.text = "Attack: " + info.attack;
        this.playerDef.text = "Defense: " + info.defense;
    }

    /*
     * Muestra el inventario del jugador actualizando el texto de los botones
     */
    public void ShowPlayerInventory(Bag inventory) {
        this.mainMenu.SetActive(false);
        this.inventoryMenu.SetActive(true);

        for (int i = 0; i < inventory.GetCurrentNumItems(); i++) {
            this.playerInventory[i].transform.Find("Text").GetComponent<Text>().text = inventory.items[i].nombre;
        }
        for (int i = inventory.GetCurrentNumItems(); i < inventory.inventorySize; i++) {
            this.playerInventory[i].transform.Find("Text").GetComponent<Text>().text = "";
        }
    }

    /*
     * Muestra la magia del jugador actualizando el texto de los botones
     */
    public void ShowPlayerMagics(MagicLearned magics) {
        this.mainMenu.SetActive(false);
        this.magicMenu.SetActive(true);

        for (int i = 0; i < magics.GetCurrentNumMagics(); i++) {
            this.playerMagic[i].transform.Find("Text").GetComponent<Text>().text = magics.magics[i].nombre;
        }
        for (int i = magics.GetCurrentNumMagics(); i < magics.magicCapacity; i++) {
            this.playerMagic[i].transform.Find("Text").GetComponent<Text>().text = "";
        }
    }

    /*
     * Metodo para salir del juego
     */
    public void Quit() {
        Application.Quit();
    }

    /*
     * Metodo para activar la pantalla principal del menu
     */
    public void Activate() {
        this.menu.SetActive(true);
        this.mainMenu.SetActive(true);
    }

    /*
     * Desactiva todas las pantallas del menu
     */
    public void Deactivate() {
        this.menu.SetActive(false);
        this.mainMenu.SetActive(false);
        this.playerMenu.SetActive(false);
        this.magicMenu.SetActive(false);
        this.inventoryMenu.SetActive(false);
    }

    /*
     * Metodo para volver atras desde una pestaña
     * del menu
     */
    public void Back() {
        this.menu.SetActive(true);
        this.mainMenu.SetActive(true);
        this.playerMenu.SetActive(false);
        this.magicMenu.SetActive(false);
        this.inventoryMenu.SetActive(false);
    }

    /*
     * Metodo que recibe el daño que sufre un jugador y lo muestra en pantalla
     * con letras rojas flotando encima de él. Para esto se realiza una traduccion
     * entre las coordenadas del jugador y las coordenadas de la GUI
     */
    public void ShowDamage(int damage, MovingObject mo, bool heal) {
        Vector3 pos = Camera.main.WorldToViewportPoint(mo.transform.position);
        GameObject damageInstance = Instantiate(this.damagePrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        damageInstance.transform.SetParent(this.menu.transform.parent, false);
        damageInstance.GetComponent<RectTransform>().anchorMin = new Vector3(pos.x +0.1f, pos.y, pos.z);
        damageInstance.GetComponent<RectTransform>().anchorMax = new Vector3(pos.x + 0.1f, pos.y, pos.z);
        if (!heal) {
            damageInstance.GetComponent<Text>().text =  "-" + damage;
            damageInstance.GetComponent<Text>().color = Color.red;
        }
        else {
            damageInstance.GetComponent<Text>().text = "+" + damage;
            damageInstance.GetComponent<Text>().color = Color.green;
        }
        StartCoroutine(EndDamage(damageInstance));
    }

    /*
     * Corutina que espera un segundo para destruir el texto flotante del daño
     */
    public IEnumerator EndDamage(GameObject text) {
        yield return new WaitForSeconds(1.0f);
        Destroy(text);
    }
}
