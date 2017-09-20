using UnityEngine;
using System.Collections;

public class Carta : MonoBehaviour {


    private bool girado;
    private bool correcto;
    private GameObject reverso;

	// Use this for initialization
	void Start () {
	    this.girado = false;
        this.correcto = false;
        this.reverso = this.transform.Find("card_back").gameObject;
	}
	
	// Update is called once per frame
	void Update () {


	}

    void OnMouseDown(){
        
        if(!this.girado)
        {
            this.reverso.SetActive(false);
            this.girado = true;
        }

    }
	
	public bool getGirado(){
		return this.girado;	
	}
	
	public void setGirado(bool girado){
		this.girado = girado;	
	}

    public bool getCorrecto() {
        return this.correcto;
    }

    public void setCorrecto(bool correcto) {
        this.correcto = correcto;
    }

    public void gira(){
		this.girado = !this.girado;
		this.reverso.SetActive(!this.girado);
	}
}
