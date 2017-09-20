using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

/*
 * Clase Singleton que se encarga de controlar los turnos de los enemigos,
 * mantener los datos entre los niveles y acabar la partida cuando el 
 * jugador muere.
 * Basada en la clase con el mismo nombre del tutorial de Unity Roguelike 2D
 */
public class GameManager : MonoBehaviour {

    /*
     * Es un objeto Singleton
     */
    public static GameManager Instance = null;

    /*
     * Posicion de la camara
     */
    public static Vector3 cameraPos;

    /*
     * Delay al emepzar un nivel y delay al
     * empezar el turno
     */
    public float levelStartDelay = 2f;
    public float turnDelay = 0.1f;

    /*
     * Referencia al creador de escenario
     */
    public BoardManager boardScript;

    /*
     * Informacion del jugador a mantener entre niveles:
     * su cantidad de comida (empieza a 100), sus atributos de combate,
     * su inventario y la magia que conoce
     */
    public int playerFoodPoints = 100;
    public CharacterStats playerStats;
    public Bag playerBag;
    public MagicLearned playerMagics;
    public bool beggining = false;

    /*
     * Atributos booleanos qpara controlar turnos:
     * si es el turno del jugador, si un lanzamiento de objeto o magia
     * esta ocurriendo, si los enemigos se estan moviendo o si se estan
     * haciendo otro tioo de operaciones que bloqueen turnos
     */
    [HideInInspector]
    public bool PlayersTurn = false;
    public bool itemAnimation = false;
    private bool enemiesMoving;
    private bool doingSetup;

    /*
     * Texto e imagen cuando hay transicion entre niveles o acaba la
     * partida
     */
    private Text levelText;
    private GameObject levelImage;

    /*
     * Nivel inicial y lista de enemigos presentes en el actual escenario
     */
    private int level = 0;
    private List<Enemy> enemies;
    

    /*
     * Al despertarse se crea la instancia Singleton si no esta creada, se
     * consiguen los componentes del jugador, se indica que este GameObject no debe
     * ser destruido al cargar la escena, se inicializa la lista de enemigos y
     * se consigue el creador de escenarios
     */
    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else if (Instance != this) {
            Destroy(gameObject);
        }

        this.beggining = true;
        this.playerStats = this.gameObject.AddComponent<CharacterStats>();
        this.playerBag = this.gameObject.AddComponent<Bag>();
        this.playerMagics = this.gameObject.AddComponent<MagicLearned>();
        DontDestroyOnLoad(this.gameObject);
        enemies = new List<Enemy>();
        boardScript = GetComponent<BoardManager>();
        //InitGame();
    }

    /*
    void OnLevelWasLoaded(int index)
    {
        //Add one to our level number.
        level++;
        //Call InitGame to initialize our level.
        InitGame();
    }
    */

    /*
     * Cuando un nivel acaba de cargarse se aumenta el contador y se inicializa
     * el juego
     */
    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
        level++;
        InitGame();
    }
    

    void OnEnable() {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }
    
    /*
     * Metodo para copiar los atributos del jugador
     */
    public void AddPlayerInfo(CharacterStats stats, Bag bag, MagicLearned magics) {
        this.playerStats.Copy(stats);
        this.playerBag.Copy(bag, this.gameObject);
        this.playerMagics.Copy(magics, this.gameObject);
    }

    /*
    // Use this for initialization
    void Start() {

    }
    */

    /*
     * Si no es el turno del jugador, los enemigos no se estan moviendo, no hay una animacion
     * de un objeto en curso y no se estan haciendo operaciones de carga, se da el turno a los
     * enemigos
     */
    void Update() {

        if (PlayersTurn || enemiesMoving || doingSetup || itemAnimation) {
            return;
        }

        StartCoroutine(MoveEnemies());
    }
    
    /*
     * Metodo para añadir un enemigo a la lista de enemigos
     */
    public void AddEnemyToList(Enemy script) {
        script.setId(enemies.Count);
        enemies.Add(script);
    }

    /*
     * Metodo que elimina un enemigo de la lista de enemigos.
     * Hace una copia del array y guarda a los enemigos que no
     * son el que se queria eliminar
     */
    public void RemoveEnemyFromList(Enemy script) {
        int i = 0;
        Enemy[] enems = enemies.ToArray();
        enemies.Clear();
        while(i<enems.Length) {
            if(script.getId() != enems[i].getId()) {
                enemies.Add(enems[i]);
            }
            i++;
        }
    }
    
    /*
     * Cuando sea GameOver se muestra el mensage, se cambia la pantalla
     * y se sale del juego
     */
    public void GameOver() {
        levelText.text = "After " + level + " days, you starved.";
        levelImage.SetActive(true);
        enabled = false;

        Invoke("Quit", 5);
    }

    public void Quit() {
        Application.Quit();
    }
    
    /*
     * Al iniciar el juego se muestra un fondo en negro con el nivel, y se
     * crea el escenario
     */
    void InitGame() {
        
        doingSetup = true;
        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Day " + level;
        levelImage.SetActive(true);
        Invoke("HideLevelImage", levelStartDelay);
        
        enemies.Clear();
        boardScript.SetupScene(level);
    }

    /*
     * Metodo que oculta la imagen de transicion entre niveles
     */
    private void HideLevelImage() {
        levelImage.SetActive(false);
        doingSetup = false;
    }
    
    /*
     * Corutina para mover enemigos. Se ordena a un enemigo moverse y
     * despues de un tiempo al siguiente, asi parece que el movimiento se
     * desarrolla por turnos sin tener que esperar demasiado
     */
    IEnumerator MoveEnemies() {
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);
        if (enemies.Count == 0) {
            yield return new WaitForSeconds(turnDelay);
        }

        for (int i = 0; i < enemies.Count; i++) {
            enemies[i].MoveEnemy();
            yield return new WaitForSeconds(enemies[i].moveTime);
        }

        PlayersTurn = true;
        enemiesMoving = false;
    }

    public Enemy[] getEnemies() {
        return this.enemies.ToArray();
    }
    
}
