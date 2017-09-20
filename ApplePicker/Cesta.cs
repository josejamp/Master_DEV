using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class Cesta : MonoBehaviour {

    private int numCapas;
    private GameObject[] capas;

    [SerializeField] private Text scoreLabel;

    void Start(){
        this.numCapas = 3;

        capas = new GameObject[this.numCapas];
        for (int i = 0; i < this.numCapas; i++)
        {
            capas[i] = GameObject.Find(getCapaFromNum(i));
        }

        scoreLabel.text = "0";
        
    }

    void Update(){
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z)); ;
        Vector3 pos = this.transform.position;
        pos.x = mousePos3D.x;
        this.transform.position = pos;
    }

    void OnCollisionEnter(Collision coll){
        GameObject colision = coll.gameObject;
        if (colision.tag == "Manzana"){
            Destroy(colision);
        }

        int punt = int.Parse(scoreLabel.text) + 100;
        scoreLabel.text = punt.ToString();
        
        
        if (punt > Puntuacion.punt){
            Puntuacion.punt = punt;
        }
        
        
    }

    public void destruyeCapa()
    {
        GameObject[] manzanas= GameObject.FindGameObjectsWithTag("Manzana");
        foreach (GameObject manzana in manzanas){
            Destroy(manzana);
        }

        if (this.numCapas > 0){
            Destroy(capas[(3-this.numCapas)]);
            if (this.numCapas > 1){
                // Como desaparece la capa de arriba hay que volver a calcular la nueva zona de colison,
                // para esto, se coge la colision de la nueva capa, se transforman su centro y tam a coord
                // globales, y luego a locales de la Cesta
                this.GetComponent<BoxCollider>().center = this.transform.InverseTransformPoint(this.capas[(3 - this.numCapas) + 1].transform.TransformPoint(this.capas[(3 - this.numCapas) + 1].GetComponent<BoxCollider>().center));
                this.GetComponent<BoxCollider>().size = this.transform.InverseTransformPoint(this.capas[(3 - this.numCapas) + 1].transform.TransformPoint(this.capas[(3 - this.numCapas) + 1].GetComponent<BoxCollider>().size));
            }
            else{
                SceneManager.LoadScene("tree");
            }
            this.numCapas -= 1;
        }
    }

    private string getCapaFromNum(int num){
        string capa = "";
        switch (num){
        case 0:{
             capa = "Capa1";
        } break;
        case 1:{
             capa = "Capa2";
        } break;
        case 2:{
            capa = "Capa3";
         } break;
        default: break;
        }
        return capa;
    }

}
