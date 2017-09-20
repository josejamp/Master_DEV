using UnityEngine;
using System.Collections;

/*
 * Representa las estadisticas del personaje y enemigos: salud, mana, ataque y
 * defensa. Tambien controla el nivel del personaje y los enemigos y como aumentan
 * sus atributos al subir de nivel
 */
public class CharacterStats : MonoBehaviour {

    /*
     * Atributos principales: salud maxima, mana maximo, ataque, defensa, 
     * salud actual y mana actual
     */
    public int hp;
    public int mana;
    public int attack;
    public int defense;
    public int actualHp;
    public int actualMana;

    /*
     * Atributos relacionados con el nivel: Nivel del personaje,
     * experiencia conseguida hasta ahora y experiencia
     * necesaria para subir al siguiente nivel
     */
    public int nivel;
    private int exp;
    private int expToNextLevel;

    /*
     * Atributos relacionados con la subida de nivel: Experiencia que da
     * el personaje al ser derrotado, incrementos de salud, ataque y defensa
     * al subir de nivel
     */
    public int baseExpGiven;
    public int hpIncrease;
    public int attackIncrease;
    public int defenseIncrease;

    /*
     * Se inicializa con experiencia base y se calcula la experiencia que se
     * necesita para subir al siguiente nivel: EXP = (4/5)*(N^3) (excepto los dos primeros niveles)
     */
    void Start () {
        this.exp = 0;
        if (this.nivel > 1) this.expToNextLevel = ((4 / 5) * (this.nivel * this.nivel * this.nivel)) - ((4 / 5) * (this.nivel - 1) * (this.nivel - 1) * (this.nivel - 1));
        else this.expToNextLevel = 1;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    /*
     * Calculo de daño durante el combate, actualiza la salud
     * y devuelve el daño realizado
     */
    public int doDamage(CharacterStats enemy) {
        int damage = enemy.attack - this.defense;
        if (damage < 0) damage = 1;
        this.actualHp -= damage;
        return damage;
    }

    /*
     * Metodo para añadir experiencia. Tambien se comprueba si
     * se ha de subir de nivel y se encarga de actualizar los
     * atributos como corresponda: incrementando el nivel en 1,
     * poniendo la experiencia actual a 0, y recalculando la
     * experiencia que se necesita para subir al siguiente nivel
     * segun la formula EXP = (4/5)*(N^3)
     */
    public void addExperience(int exp) {
        this.exp += exp;
        if (this.exp >= this.expToNextLevel) {
            newLevel(this.nivel + 1);
            int exp_aux = this.exp - this.expToNextLevel;
            if (this.nivel > 1) this.expToNextLevel = ((4 *(this.nivel * this.nivel * this.nivel))/5) - ((4* (this.nivel - 1) * (this.nivel - 1) * (this.nivel - 1))/5);
            else this.expToNextLevel = 1;
            if (exp_aux >= this.expToNextLevel) {
                this.exp = 0;
                this.addExperience(exp_aux);
            }
        }
    }

    /*
     * Devuelve la experiencia que da este personaje al ser
     * derrotado: la experiencia base si esta por debajo del nivel 2,
     * sino Exp_base*log(N)
     */
    public int giveExperience() {
        if (this.nivel < 2) {
            return this.baseExpGiven;
        }
        else {
            return this.baseExpGiven * Mathf.RoundToInt(Mathf.Log(this.nivel, 2.0f));
        }
    }

    /*
     * Metodo para subir de nivel, aumenta el nivel, los atributos, y
     * cura la vida
     */
    private void newLevel(int newLevel) {
        this.nivel = newLevel;

        this.hp += this.hpIncrease;
        this.attack += this.attackIncrease;
        this.defense += this.defenseIncrease;
        this.actualHp = this.hp;
    }

    public int getExp() {
        return this.exp;
    }

    public int getExpToNextLevel() {
        return this.expToNextLevel;
    }

    public void Copy(CharacterStats stats) {
        this.actualHp = stats.actualHp;
        this.actualMana = stats.actualMana;
        this.hp = stats.hp;
        this.mana = stats.mana;
        this.attack = stats.attack;
        this.defense = stats.defense;
        this.nivel = stats.nivel;
        this.exp = stats.exp;
        this.expToNextLevel = stats.expToNextLevel;
        this.baseExpGiven = stats.baseExpGiven;
        this.hpIncrease = stats.hpIncrease;
        this.attackIncrease = stats.attackIncrease;
        this.defenseIncrease = stats.defenseIncrease;
    }

}
