using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Puntuacion : MonoBehaviour {

    public static int punt = 100;

    void Awake(){
        PlayerPrefs.DeleteAll();
        if (PlayerPrefs.HasKey("HighScore")){
            punt = PlayerPrefs.GetInt("HighScore");
        }
        PlayerPrefs.SetInt("HighScore", punt);
    }

    void Update(){
        Text gt = this.GetComponent<Text>();
        gt.text = "High Score: " + punt;
        if (punt > PlayerPrefs.GetInt("HighScore")){
            PlayerPrefs.SetInt("HighScore", punt);
        }
    }
}
