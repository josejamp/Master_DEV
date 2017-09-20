using UnityEngine;
using System.Collections;

public class CloudCrafter: MonoBehaviour {

    public GameObject[] cloudPrefabs;
    public int numClouds = 40;

    public Vector3 cloudPosMin; 
    public Vector3 cloudPosMax; 

    public float cloudScaleMin = 1; 
    public float cloudScaleMax = 5; 
    public float cloudSpeedMult = 0.5f; 


    public bool _____________________________;

    public GameObject[] nubes;


    void Awake(){
        this.nubes = new GameObject[this.numClouds];
        GameObject padre = GameObject.Find("PadreNubes");

        GameObject nube;

        for (int i = 0; i < this.numClouds; i++){

            int prefab = Random.Range(0, cloudPrefabs.Length);
            nube = Instantiate(cloudPrefabs[prefab]) as GameObject;

            Vector3 pos = new Vector3(Random.Range(this.cloudPosMin.x, this.cloudPosMax.x), Random.Range(this.cloudPosMin.y, this.cloudPosMax.y), 0);

            float scaleU = Random.value;
            float scaleVal = Mathf.Lerp(this.cloudScaleMin, this.cloudScaleMax, scaleU);
            pos.y = Mathf.Lerp(this.cloudPosMin.y, pos.y, scaleU);
            pos.z = 100 - 90 * scaleU;

            nube.transform.position = pos;
            nube.transform.localScale = Vector3.one * scaleVal;
            nube.transform.parent = padre.transform;

            this.nubes[i] = nube;
        }
    }

    void Update() {
        foreach (GameObject cloud in nubes){
            float scaleVal = cloud.transform.localScale.x;
            Vector3 pos = cloud.transform.position;
            pos.x -= scaleVal * Time.deltaTime * cloudSpeedMult;
            if (pos.x <= this.cloudPosMin.x){
                pos.x = this.cloudPosMax.x;
            }
            cloud.transform.position = pos;
        }
    }
}
