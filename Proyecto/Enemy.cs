using UnityEngine;
using System.Collections;

/*
 * Clase que representa a los zombis enemigos en el juego,
 * Heredan de movingObject ya que necesitan moverse.
 * Basada en la clase con el mismo nombre del tutorial de Unity
 * Roguelike 2D
 */
public class Enemy : MovingObject {
    
    /*
     * Controlador de la animacion
     */
    private Animator animator;

    /*
     * Origen del audio y sonidos que hace
     * el zombi al morir
     */
    public AudioSource audioSource;
    public AudioClip audio1;
    public AudioClip audio2;

    /*
     * Objetivo al que moverse y booleano
     * que indica si el zombi se salta
     * el turno
     */
    private Transform target;
    private bool skipMove;

    /*
     * Identificador
     */
    private int id;

    /*
     * Caracteristicas de combate del enemigo
     */
    public CharacterStats stats;

    /*
     * Al comenzar se añade el enemigo a la lista de enemigos del GameManager,
     * se consiguen los componentes necesarios y se inicializa el
     * objetivo al que moverse al jugador para que le persiga.
     * Se inicializa tambien su salud, la mascara de layers bloqueantes y
     * el MovingObject del que hereda
     */
    protected override void Start() {

        GameManager.Instance.AddEnemyToList(this);
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        this.stats = GetComponent<CharacterStats>();
        target = GameObject.FindGameObjectWithTag("Player").transform;

        this.stats.actualHp = this.stats.hp;

        int mask1 = 1 << LayerMask.NameToLayer("Wall");
        int mask2 = 1 << LayerMask.NameToLayer("Hero");
        int mask3 = 1 << LayerMask.NameToLayer("Enemy");
        this.blockingLayer = mask1 | mask2 | mask3;

        base.Start();

    }

    /*
     * Se reimplementa el metodo AttemptMove para hacer uso de la animacion
     * y no hacer nada si skipMove es true
     */
    protected override void AttemptMove<T>(int xDir, int yDir) {
        if (skipMove) {
            skipMove = false;
            return;
        }

        animator.SetTrigger("enemyWalk");
        base.AttemptMove<T>(xDir, yDir);
        skipMove = true;
    }

    /*
     * Metodo para mover al enemigo en direccion al jugador.
     * Sacado del tutorial de Unity Roguelike 2D
     */
    public void MoveEnemy() {
        int xDir = 0;
        int yDir = 0;

        if (Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon) {
            yDir = target.position.z > transform.position.z ? 1 : -1;
        }
        else {
            xDir = target.position.x > transform.position.x ? 1 : -1;
        }

        this.ChangeDir(yDir, xDir);

        AttemptMove<Player>(xDir, yDir);
    }

    /*
     * Metodo ejecutado cuando el enemigo sufre daño.
     * Muestra el daño como numeros flotantes, cambia la salud,
     * y si la salud es menor que cero, muestra las animaciones y sonidos
     * correspondientes y da experiencia al jugador
     */
    public void beAttacked(CharacterStats enemy) {
        Menu.M.ShowDamage(this.stats.doDamage(enemy), this, false);

        if (this.stats.actualHp <= 0) {
            this.animator.SetTrigger("enemyDeath");
            enemy.addExperience(this.stats.giveExperience());
            GameManager.Instance.RemoveEnemyFromList(this);
            if (Random.value >= 0.5) this.audioSource.PlayOneShot(this.audio1);
            else this.audioSource.PlayOneShot(this.audio2);
            Invoke("die",3);
        }
    }

    /*
     * Equivalente al método anterior, pero el daño es puro y no se usan
     * las características del personaje para calcularlo
     */
    public void beAttacked(CharacterStats origin, int pureDamage) {
        this.stats.actualHp -= pureDamage;
        Menu.M.ShowDamage(pureDamage, this, false);

        if (this.stats.actualHp <= 0) {
            this.animator.SetTrigger("enemyDeath");
            origin.addExperience(this.stats.giveExperience());
            GameManager.Instance.RemoveEnemyFromList(this);
            if (Random.value >= 0.5) this.audioSource.PlayOneShot(this.audio1);
            else this.audioSource.PlayOneShot(this.audio2);
            Invoke("die", 3);
        }
    }

    /*
     * Cuando el enemigo no puede moverse debido a que el jugador
     * le bloquea el movimiento, le ataca
     */
    protected override void OnCantMove<T>(T component) {
        Player hitPlayer = component as Player;

        print("Enemigo atacando: " );
        animator.SetTrigger("enemyAttack");

        hitPlayer.beAttacked(this.stats);
    }

    private void die() {
        Destroy(this.gameObject);
    }

    public void setId(int id) {
        this.id = id;
    }

    public int getId() {
        return this.id;
    }

}
