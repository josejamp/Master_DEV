using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

/*
 * Clase para el jugador principal. Contiene su inventario,
 * magias, caracteristicas, controlador de animacion, audio
 * y referencias a la GUI presente en la parte superior de
 * la pantalla.
 * Basada en la clase con el mismo nombre del tutorial de 
 * Unity Roguelike 2D
 */
public class Player : MovingObject {
    
    /*
     * Delay al comenzar un nivel
     */
    public float restartLevelDelay = 1f;

    /*
     * Referencias a la GUI de la parte superior del juego
     */
    public Text foodText;
    public Text levelText;
    public Text healthText;
    public Text manaText;
    public Slider hpBar;
    public Slider expBar;
    public Slider manaBar;
    private bool showingMenu;

    /*
     * Controlador de la animcacion, fuente de audio
     * y pista de audio al atacar
     */
    private Animator animator;
    public AudioSource audioSource;
    public AudioClip audioAttack;

    /*
     * Nivel de comida del jugador, caracteristicas
     * de combate, inventario y magia
     */
    private int food;
    public CharacterStats stats;
    public Bag bag;
    public MagicLearned magics;

    /*
     * Bolleano que se pone a true cuando el jugador
     * lanza un objeto o magia, y esta a true
     * mientras dure la animacion
     */
    private bool waitUse;


    /*
     * Al iniciar el juego:
     * * Se consiguen los componentes y se inicializan
     * * Inicializar la GUI superior con los valores
     * * Inicializar la mascara para el movimiento
     * * Inicializr los arrays de botones del menu central
     * * Llamar al metodo Start de MovingObject
     */
    protected override void Start() {

        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
 
        this.stats = GetComponent<CharacterStats>();
        this.bag = GetComponent<Bag>();
        this.bag.Init();
        this.magics = GetComponent<MagicLearned>();

        // Si no es el principio del juego sino que se carga un nuevo nivel,
        // se copian losa datos guardados en el GameManager
        if (!GameManager.Instance.beggining) {
            this.stats.Copy(GameManager.Instance.playerStats);
            this.bag.Copy(GameManager.Instance.playerBag, this.gameObject);
            this.magics.Copy(GameManager.Instance.playerMagics, this.gameObject);
        }

        //print(this.magics.GetCurrentNumMagics());

        this.stats.actualHp = this.stats.hp;
        this.stats.actualMana = this.stats.mana;

        /* BLOQUE de inicializacion de GUI */
        food = GameManager.Instance.playerFoodPoints;
        foodText.text = "Food: " + food;
        levelText.text = "Lvl: " + stats.nivel;
        healthText.text = this.stats.actualHp + "/" + this.stats.hp;
        manaText.text = this.stats.actualMana + "/" + this.stats.mana;

        this.hpBar.maxValue = this.stats.hp;
        this.hpBar.minValue = 0;
        this.hpBar.value = this.stats.actualHp;
        this.hpBar.interactable = false;

        this.manaBar.maxValue = this.stats.mana;
        this.manaBar.minValue = 0;
        this.manaBar.value = this.stats.actualMana;
        this.manaBar.interactable = false;

        this.expBar.maxValue = this.stats.getExpToNextLevel();
        this.expBar.minValue = 0;
        this.expBar.value = this.stats.getExp();
        this.expBar.interactable = false;
        /* FIN BLOQUE */

        // EL jugador no puede atravesar paredes ni enemigos
        int mask1 = 1 << LayerMask.NameToLayer("Wall");
        int mask2 = 1 << LayerMask.NameToLayer("Enemy");
        this.blockingLayer = mask1 | mask2;

        // El menu principal no se muestra
        this.showingMenu = false;
        
        // No hay ninguna animacion de lanzamiento inicial
        this.waitUse = false;

        // Inicializacion de los arrays de botones
        Menu.M.initInventoryMenu(this);
        Menu.M.initMagicMenu(this);

        base.Start();
        
    }

    /*
     * Metodo que se ejecuta cuando el GameObject se desactiva.
     * Guarda su comida, inventario, magia y estadisticas en
     * el GameManager
     */
    private void OnDisable() {
        GameManager.Instance.AddPlayerInfo(this.stats, this.bag, this.magics);
        GameManager.Instance.playerFoodPoints = food;
        GameManager.Instance.beggining = false;
    }

    /*
     * El metodo Update se encarga de detectar si se han pulsado las
     * flechas para moverse, la barra espaciadora para atacar, o Intro
     * para abrir el menu; y acutar en consecuencia
     */
    void Update() {

        // SI no es el turno del jugador o hay una animacion en curso
        // no se hace nada
        if (!GameManager.Instance.PlayersTurn || this.waitUse) return;

        // Si se ha pulsado Intro o Tabulador se actúa en consecuencia
        // ero no se consume turno
        if (this.enterPressed() || this.tabPressed()) return;

        // Si no, se comprueba si se han pulsado las flechas
        int horizontal = 0;
        int vertical = 0;
        int[] axis = this.GetDirectonalKeys();
        horizontal = axis[0];
        vertical = axis[1];

        // SI se ha pulsado alguna flecha se cambia la posicion del
        // personaje y se le rota
        if (horizontal != 0 || vertical != 0) {
            this.ChangeDir(vertical, horizontal);
            AttemptMove<Player>(horizontal, vertical);
        } // Si se ha pulsado la barra espaciadora se ataca al enemigo
        else if (Input.GetAxis("Jump") == 1) {
            this.animator.SetTrigger("playerAttack");
            this.audioSource.PlayOneShot(this.audioAttack);
            this.Attack<Enemy>(this.DetectObstacle<Enemy>());
            // Se actualiza la GUI
            this.updateHpBar();
            healthText.text = this.stats.actualHp + "/" + this.stats.hp;
            this.updateExpBar();
            levelText.text = "Lvl: " + stats.nivel;
        }

    }

    /*
     * Metodo sobreescrito para hacer que al tratar de mover al jugador
     * se pierda comida, y si se consigue mover, ejecutar la animacion,
     * y finalmente actualizar la GUI y comprobar si no le queda comida
     * o salud
     */
    protected override void AttemptMove<T>(int xDir, int yDir) {

        if (food > 0) {
            food--;
            foodText.text = "Food: " + food;
        }

        base.AttemptMove<T>(xDir, yDir);

        RaycastHit hit;
        if (Move(xDir, yDir, out hit)) {
            animator.SetTrigger("playerWalk");
        }

        CheckIfNoFood();
        CheckIfGameOver();
        GameManager.Instance.PlayersTurn = false;

    }

    /**
     * Si el personaje colisiona con la salida, se avanza el nivel,
     * Si colisiona con un objeto, si el jugador tiene espacio
     * libre en su mochila, el objeto se elimina del suelo y se
     * añade a la mochila
     */
    private void OnTriggerEnter(Collider other) {
        /* DEBUG
        print("En el trigger: " + other);
        print("Player: " + this.transform.position);
        print("Player bounds: " + this.GetComponent<Collider>().bounds);
        print("Other pos: " + other.transform.position);
        print("Other bounds: " + other.bounds);
        */
        if (other.tag == "Exit") {
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        }
        else if (other.tag == "Item") {
            if (!this.bag.Full()) {
                Item nuevo = Instantiate(other.GetComponent<Item>());
                nuevo.gameObject.SetActive(false);
                nuevo.gameObject.GetComponent<MeshRenderer>().enabled = false;
                nuevo.GetComponent<BoxCollider>().isTrigger = false;
                this.bag.Add(nuevo);
                Destroy(other.gameObject);
            }
        }
    }

    /*
     * El jugador no tiene que hacer nada es especial cuando no
     * puede moverse 
     */
    protected override void OnCantMove<T>(T component) {

    }

    private void Restart() {
        SceneManager.LoadScene(0);
    }

    /*
     * Cuando el jugador es atacado hayq ue actualizar su salud,
     * la GUI, y comprobar si no es GameOver
     */
    public void beAttacked(CharacterStats enemy) {
        Menu.M.ShowDamage(this.stats.doDamage(enemy), this, false);
        this.updateHpBar();
        healthText.text = this.stats.actualHp + "/" + this.stats.hp;
        CheckIfGameOver();
    }

    /*
     * Cuando el jugador pierde comida hay que actualizar la GUI
     * y comprobar si no se pierde la partida
     */
    public void LoseFood(int loss) {

        if(food > 0) food -= loss;
        foodText.text = "Food: " + food;
        CheckIfGameOver();
    }

    /*
     * Si el jugador gana comida hay que actualizar la GUI, y
     * comprobar que la comida no sea superior a 100
     */
    public void eatFood(int gain)
    {
        food += gain;
        if (food > 100) food = 100;
        foodText.text = "Food: " + food;
    }

    /*
     * Al atacar a un enemigo, el enemigo debe encargarse del
     * calculo del daño y de dar la experiencia.
     * El jugador consume su turno
     */
    private void Attack<T>(T component){
        Enemy enemy = component as Enemy;
        if (enemy != null) {
            enemy.beAttacked(this.stats);
        }
        GameManager.Instance.PlayersTurn = false;
    }

    /*
     * Si la salud del personaje es menor o igual que 0 se pierde
     * la partida
     */
    private void CheckIfGameOver() {
        if (this.stats.actualHp <= 0) {
            animator.SetTrigger("playerDeath");
            GameManager.Instance.GameOver();
        }

    }

    /*
     * Si el personaje tiene el estomago vacio se pierde salud,
     * y se actualiza la GUI
     */
    private void CheckIfNoFood() {
        if (this.food <= 0) {
            this.stats.actualHp -= 4;
            Menu.M.ShowDamage(4, this, false);
            this.hpBar.value = this.stats.actualHp;
            healthText.text = this.stats.actualHp + "/" + this.stats.hp;
        }

    }

    /*
     * Metodo que consigue si se han pulsado las flechas del teclado
     */
    private int[] GetDirectonalKeys() {
        int horizontal = 0;
        int vertical = 0;


        horizontal = (int)(Input.GetAxisRaw("Horizontal"));
        vertical = (int)(Input.GetAxisRaw("Vertical"));

        if (horizontal != 0)
            vertical = 0;

        return  new int[]{horizontal, vertical};
    }


    /*
     * Actuañoza la barra de vida de la GUI
     */
    private void updateHpBar() {
        this.hpBar.maxValue = this.stats.hp;
        this.hpBar.value = this.stats.actualHp;
    }

    /*
     * Actuañoza la barra de mana de la GUI
     */
    private void updateManaBar() {
        this.manaBar.maxValue = this.stats.mana;
        this.manaBar.value = this.stats.actualMana;
    }

    /*
     * Actuañoza la barra de experiencia de la GUI
     */
    private void updateExpBar() {
        this.expBar.maxValue = this.stats.getExpToNextLevel();
        this.expBar.value = this.stats.getExp();
    }

    /*
     * Si se ha pulsado Intro, muestra el menu si no se
     * mostraba ya, si ya estaba mostrandose, lo cierra
     */
    private bool enterPressed() {
        if (Input.GetKeyUp("return")) {
            this.showingMenu = !this.showingMenu;
            if (this.showingMenu) Menu.M.Activate();
            else this.Resume();
            return true;
        }
        else if (this.showingMenu) return true;
        else return false;
    }

    /*
     * Si se ha pulsado Tab, se comprueba la flecha que
     * se pulsa para girar al personaje en ese sentido
     */
    private bool tabPressed() {
        if (Input.GetKey("tab")) {
            int horizontal = 0;
            int vertical = 0;
            int[] axis = this.GetDirectonalKeys();
            horizontal = axis[0];
            vertical = axis[1];


            if (horizontal != 0 || vertical != 0) {
                this.ChangeDir(vertical, horizontal);
            }
            return true;
        }
        else return false;
    }

    /*
     * Metodo para desactivar el menu
     */
    public void Resume() {
        Menu.M.Deactivate();
        this.showingMenu = false;
    }

    /*
     * Metodo para mostrar la info del jugador en el menu
     */
    public void PlayerInfo() {
        Menu.M.ShowPlayerInfo(this.stats);
    }

    /*
     * Metodo para mostrar el inventario del jugador en el menu
     */
    public void InventoryInfo() {
        Menu.M.ShowPlayerInventory(this.bag);
    }

    /*
     * Metodo para mostrar la magia del jugador en el menu
     */
    public void MagicInfo() {
        Menu.M.ShowPlayerMagics(this.magics);
    }

    /*
     * Metodo para usar el objeto que ocupa la posicion pos
     * del inventario, consume turno
     */
    public void UseItem(int pos) {
        if (pos < this.bag.GetCurrentNumItems()) {
            this.bag.Use<Player>(pos, this);
            this.Resume();
        }
        GameManager.Instance.PlayersTurn = false;
    }

    /*
     * Metodo para usar la magia que ocupa la posicion pos
     * de las magias conocidas, consume turno
     */
    public void UseMagic(int pos) {
        if (pos < this.magics.GetCurrentNumMagics()) {
            this.magics.Use<Player,Enemy>(pos, this, GameManager.Instance.getEnemies());
            this.Resume();
        }
        GameManager.Instance.PlayersTurn = false;
    }

    /*
     * Devuelve el numero de objetos del inventario
     */
    public int getBagSize() {
        return this.bag.inventorySize;
    }

    /*
     * Devuelve el numero de magias conocidas
     */
    public int getNumMagicsLearned() {
        return this.magics.magicCapacity;
    }

    /*
     * Devuelve la direccion a la que mira el personaje
     */
    public Direction getDirection() {
        return this.dir;
    }

    /*
     * Metodo que aumenta la experiencia del jugador en cierta
     * cantidad y actualiza la GUI
     */
    public void gainExp(int val) {
        this.stats.addExperience(val);
        this.updateExpBar();
        levelText.text = "Lvl: " + stats.nivel;
        this.updateHpBar();
        healthText.text = this.stats.actualHp + "/" + this.stats.hp;
    }

    /*
     * Metodo que aumenta la salud del jugador en cierta
     * cantidad y actualiza la GUI
     */
    public void gainHealth(int hp) {
        this.stats.actualHp += hp;
        if (this.stats.actualHp > this.stats.hp) {
            this.stats.actualHp = this.stats.hp;
        }
        this.updateHpBar();
        healthText.text = this.stats.actualHp + "/" + this.stats.hp;
    }

    /*
     * Metodo que aumenta el mana del jugador en cierta
     * cantidad y actualiza la GUI
     */
    public void spendMana(int mana) {
        this.stats.actualMana -= mana;
        if (this.stats.actualMana <= 0) {
            this.stats.actualMana = 0;
        }
        this.updateManaBar();
        manaText.text = this.stats.actualMana + "/" + this.stats.mana;
    }

    public void waitForUse() {
        this.waitUse = true;
    }

    public void waitUseFinished(){
        this.waitUse = false;
    }
}
