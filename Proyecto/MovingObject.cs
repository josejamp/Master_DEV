using UnityEngine;
using System.Collections;

/*
 * Enumerado que representa los puntos cardinales
 * para indicar la direccion a la que se dirige el
 * MovingObject.
 * Basada en la clase con el mismo nombre del tutorial de Unity
 * Roguelike 2D
 */
public enum Direction {
    none,
    north,
    south,
    east,
    west
}

/*
 * Clase que encapsula la funcionalidad de los elementos
 * del juego que necesitan moverse por el escenario
 */
public abstract class MovingObject : MonoBehaviour {
    
    /*
     * Tiempo para moverse, usado para calcular
     * su inversa y usarlo en Vector3.MoveTowards
     */
    public float moveTime = 0.1f;
    private float inversemoveTime;

    /*
     * Mascara para indicar que Layers bloquean el movimiento
     */
    public LayerMask blockingLayer;

    /*
     * BoxCollider y RgidBody del presente MovingObject
     */
    private BoxCollider boxCollider;
    private Rigidbody rb;

    /*
     * Direccion a la que mira el MovingObject
     */
    protected Direction dir;

    /*
     * Al comenzar se consiguen los componentes y se
     * inicializa el tiempo de movimiento y la direccion
     */
    protected virtual void Start() {

        boxCollider = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        inversemoveTime = 1.0f / moveTime;

        this.dir = Direction.north;

    }


    // Update is called once per frame
    void Update() {

    }

    /*
     * Metodo para moverse a una coordenada (x,0,z), devuelve un
     * booleano inciando si pude moverse y un RayCast con el objecto
     * con el que colisiona el movimiento, si es que esto sucede.
     * Basado en el metodo de Unity Roguelike 2D, adaptado a 3D
     */
    protected bool Move(int xDir, int zDir, out RaycastHit hit) {
        Vector3 start = transform.position;
        Vector3 end = start + new Vector3(xDir, 0.0f, zDir);

        boxCollider.enabled = false;
        // Se usa un Linecast para ver si al moverse colisiona
        Physics.Linecast(start, end, out hit, blockingLayer);
        boxCollider.enabled = true;

        // Si no choca, se comienza una corutina para moverse
        if (hit.transform == null) {
            StartCoroutine(SmoothMovement(end));
            return true;
        }
        return false;
    }

    /*
     * Corutina que mueve al MovingObject hacia la posicion end.
     * Sacado del tutorial de UNity Roguelike 2D
     */
    protected IEnumerator SmoothMovement(Vector3 end) {
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        // Se compara con Epsilon para evitar comparar con 0
        while (sqrRemainingDistance > float.Epsilon) {
            Vector3 newPosition = Vector3.MoveTowards(rb.position, end, inversemoveTime * Time.deltaTime);
            rb.MovePosition(newPosition);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;
        }
    }

    /*
     * Metodo para tratar de moverse hacia un punto. Hace uso
     * del metodo Move anterior. Si choca devuelve el objeto con
     * el que ha chocado
     * Sacado del tutorial de Unity Roguelike 2D
     */
    protected virtual void AttemptMove<T>(int xDir, int zDir)
        where T : Component {
        RaycastHit hit;
        bool canMove = Move(xDir, zDir, out hit);

        if (hit.transform == null) return;

        T hitComponent = hit.transform.GetComponent<T>();

        if (!canMove && hitComponent != null) {
            OnCantMove(hitComponent);
        }
    }

    /*
     * Metodo que detecta un obstaculo situado enfrente del MovingObject
     * y devuelve el obstaculo encontrado
     */
    protected T DetectObstacle<T>()
        where T : Component {
        RaycastHit hit;
        Vector3 start = transform.position;
        Vector3 end = start;
        switch (this.dir) {
            case Direction.north: end += new Vector3( 0, 0, 1); break;
            case Direction.east: end += new Vector3(1, 0, 0); break;
            case Direction.south: end += new Vector3(0, 0, -1); break;
            case Direction.west: end += new Vector3(-1, 0, 0); break;
            default: break;
        }
        boxCollider.enabled = false;
        Physics.Linecast(start, end, out hit, blockingLayer);
        boxCollider.enabled = true;

        if (hit.transform == null) return null;

        T hitComponent = hit.transform.GetComponent<T>();
        return hitComponent;
    }

    /*
     * Metodo abstracto que se debe implementar en las clases que hereden
     * de MovingObject de tal manera que indique lo que hacer
     * cuando el movimiento en una direccion es imposible
     */
    protected abstract void OnCantMove<T>(T component)
            where T : Component;

    /*
     * Metodo que rota al personaje cuando cambia su direccion
     */
    protected void changeFacing(Direction newDir) {

        switch (newDir) {
            case Direction.north: {
                    this.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            break;
            case Direction.east: {
                    this.transform.rotation = Quaternion.Euler(0, 90, 0);
                }
                break;
            case Direction.south: {
                    this.transform.rotation = Quaternion.Euler(0, 180, 0); 
                 }
                break;
            case Direction.west: {
                    this.transform.rotation = Quaternion.Euler(0, -90, 0);
                }
                break;
            default: break;
        }
    }

    /*
     * Metodo que cambia la direccion de un personaje y le hace
     * rotar en funcion del movimiento que se le ordena hacer
     */
    protected void ChangeDir(int vertical, int horizontal) {
        if (horizontal > 0) {
            this.changeFacing(Direction.east);
            this.dir = Direction.east;
        }
        else if (horizontal < 0) {
            this.changeFacing(Direction.west);
            this.dir = Direction.west;
        }
        if (vertical > 0) {
            this.changeFacing(Direction.north);
            this.dir = Direction.north;
        }
        else if (vertical < 0) {
            this.changeFacing(Direction.south);
            this.dir = Direction.south;
        }
    }

}

