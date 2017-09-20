using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneController : MonoBehaviour {

    [SerializeField] private GameObject[] prefabs;

    public Text text;
    public Button button;
    private int punt;

    private GameObject[] instances;
    private int numInstances = 8;
	
	private float largoTapete = 1;
	private float anchoCarta = 1f;
	private float largoCarta = 1f;
    private float margenXExt = -4.0f;
	private float margenXInt = 1.25f;
	private float margenYExt = 1.5f;
	private float margenYInt = 1f;
	private float prof = 0;
	

	// Use this for initialization
	void Start () {

        this.punt = 0;
        this.text.text = "Puntuacion: " + this.punt.ToString();

        this.instances = new GameObject[this.numInstances];

        for (int i = 0; i < this.instances.Length; i++){
            GameObject nueva = Instantiate(this.prefabs[i % 4]) as GameObject;
            this.instances[i] = nueva;
        }

		this.baraja();
		
		this.posiciona();
		
	}
	
	// Update is called once per frame
	void Update () {
        StartCoroutine("Wait");
    }

    private void baraja(){
        for (int i = 0; i < this.instances.Length-1; i++){
            int rnd = Random.Range(i + 1, this.instances.Length - 1);
            //GameObject obj1 = Instantiate(this.instances[rnd]);
            // GameObject obj2 = Instantiate(this.instances[i]);
            GameObject obj1 = this.instances[rnd];
            GameObject obj2 = this.instances[i];
            this.instances[i] = obj1;
            this.instances[rnd] = obj2;
        }
    }
	
	private void posiciona(){
		float anchoAcum = 0.0f;
		float largoAcum = 0.0f;
		for (int i = 0; i < this.instances.Length; i++){
			if(i%4==0){
				anchoAcum = 0.0f;
				anchoAcum += margenXExt + anchoCarta/2;
				if(i==0)
					largoAcum = largoTapete - margenYExt - largoCarta/2;
				else
					largoAcum = margenYExt + largoCarta/2;
				Vector3 pos = new Vector3(anchoAcum, largoAcum, prof);
				this.instances[i].transform.position = pos;
				anchoAcum += anchoCarta/2;
			}
			else{
				anchoAcum += margenXInt + anchoCarta/2;
				Vector3 pos = new Vector3(anchoAcum, largoAcum, prof);
				this.instances[i].transform.position = pos;
				anchoAcum += anchoCarta/2;
			}
		}
	}
	
	private bool compruebaGiradas(){
		
		int girados = 0;
        GameObject girado1 = null, girado2 = null;
        for (int i = 0; i < this.instances.Length; i++){
            if (this.instances[i].GetComponent<Carta>().getGirado() && !this.instances[i].GetComponent<Carta>().getCorrecto()) {
                girados++;
                if (girados==1) girado1 = this.instances[i];
                else girado2 = this.instances[i];
            }
            if (girados > 1) { 
                if (girado1.tag == girado2.tag) {
                    girado1.GetComponent<Carta>().setCorrecto(true);
                    girado2.GetComponent<Carta>().setCorrecto(true);
                    this.punt++;
                    this.text.text = "Puntuacion: " + this.punt.ToString();
                }
                return true;
            }
		}
		return false;
		
	}
	
	IEnumerator Wait(){
		for(;;){
			if(compruebaGiradas()){
                yield return new WaitForSeconds(0.4f);
                for (int i = 0; i < this.instances.Length; i++){
					Carta comp = this.instances[i].GetComponent<Carta>();
                    //print("Girado: " + comp.getGirado());
                    //print("Correcto: " + comp.getCorrecto());
                    if (comp.getGirado() && !comp.getCorrecto()) {
                        comp.gira();
					}
				}
			}	
			else yield return new WaitForSeconds(1.0f);
		}
	}


    public void onCLick() {
        SceneManager.LoadScene("memory");
    }


}
