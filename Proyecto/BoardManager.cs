using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

/*
 * Clase encargada de crear la habitacion donde se
 * desarrolla el juego. Crea un numero aleatorio de
 * elementos y los coloca en posiciones aleatorias
 * Basada en la clase con el mismo nombre del tutorial
 * de Unity Roguelike 2D
 */
public class BoardManager : MonoBehaviour {

    /*
     * Par para representar el numero minimo
     * y maximo de objetos que pueden aparecer
     */
    [Serializable]
    public class Count {
        public int minimum;
        public int maximum;

        public Count(int min, int max) {
            minimum = min;
            maximum = max;
        }
    }

    /*
     * Numero de columnas y filas del escenario
     */
    public int columns = 8;
    public int rows = 8;

    /*
     * Maximos y minimos para la comida, los obstaculos
     * y los objetos que aparecen en el escenario
     */
    public Count wallCount = new Count(5, 9);
    public Count foodCount = new Count(1, 5);
    public Count itemCount = new Count(1, 2);

    /*
     * Arrays con los prefabs de los GameObjects que
     * aparecen en el escenario
     */
    public GameObject exit;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] itemTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallTiles;

    /*
     * Posiciones para colocar los objetos
     */
    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();

    /*
     * Inicializa las posiciones del grid donde colocar
     * los objetos en funcion de las dimensiones del escenario
     */
    void InitialiseList() {
        gridPositions.Clear();

        for (int x = 1; x < columns - 1; x++) {
            for (int y = 1; y < rows - 1; y++) {
                gridPositions.Add(new Vector3(x, 1.0f, y));
            }
        }
    }

    /*
     * Metodo que crea el suelo y las paredes
     */
    void BoardSetup() {
        boardHolder = new GameObject("Board").transform;

        for (int x = -1; x < columns + 1; x++) {
            for (int y = -1; y < rows + 1; y++) {
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                GameObject instance;
                if (x == -1 || x == columns || y == -1 || y == rows) {
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                    instance = Instantiate(toInstantiate, new Vector3(x, 1.0f, y), Quaternion.identity) as GameObject;
                }

                instance = Instantiate(toInstantiate, new Vector3(x, 0.0f, y), Quaternion.identity) as GameObject;

                instance.transform.SetParent(boardHolder);
            }
        }
    }

    /*
     * Metodo que devuelve una posicion aleatoria del escenario
     * que no este ocupada
     */
    Vector3 RandomPosition() {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

    /*
     * Metodo que coloca elementos aleatorios de un array de prefabs de objetos 
     * en posiciones aleatorias del escenario teniendo en cuenta el numero minimo
     * y maximo de objetos que se pueden colocar
     */
    void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum) {
        int objectCount = Random.Range(minimum, maximum + 1);
        for (int i = 0; i < objectCount; i++) {
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            tileChoice.transform.position = randomPosition;
            Instantiate(tileChoice);
        }
    }

    /*
     * Metodo principal para crear la escena. Crea el suelo y las paredes,
     * inicializa la lista, coloca los enemigos, obstaculos, comida y suelo; y
     * finalmente coloca la salida 
     */
    public void SetupScene(int level) {
        BoardSetup();
        InitialiseList();
        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);
        LayoutObjectAtRandom(itemTiles, itemCount.minimum, itemCount.maximum);
        int enemyCount = (int)Mathf.Log(level, 2.0f);
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
        exit.transform.position = new Vector3(columns - 1, 1.0f, rows - 1);
        Instantiate(exit);
       // GameManager.cameraPos = new Vector3(columns/2, 0.0f, rows/2);
    }
}
