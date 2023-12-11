using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapturePoints : MonoBehaviour{
    public string CapturedBy;
    public int ScoreBB = 50;
    public int ScoreGB = 50;

    private int NbOfGB = 0;
    private int NbOfBB = 0;

    private float Timer = 0;
    public int points;

    public Material[] material;
    Renderer rend;


    // Start is called before the first frame update
    void Start(){
        rend= GetComponent<Renderer>();
        rend.enabled = true;
        rend.sharedMaterial = material[0];
        points = 5;
        ScoreBB = 50;
        ScoreGB = 50;
        CapturedBy = "Neutral";
    }

    // Update is called once per frame
    void Update(){
        //Timer
        Timer += Time.deltaTime;

        if(ScoreBB==100){
            CapturedBy = "BadBean";
            rend.sharedMaterial = material[2];
        }else if(ScoreGB==100){
            CapturedBy = "GoodBean";
            rend.sharedMaterial = material[1];
        }else{
            if(Timer>1){
                if(NbOfBB>NbOfGB){
                    ScoreBB += points;
                    ScoreGB -= points;
                }
                if(NbOfBB<NbOfGB){
                    ScoreBB -= points;
                    ScoreGB += points;
                }
                Timer = 0;
            }
        }
    }

    void OnTriggerEnter(Collider collider){
        if (collider.gameObject.tag == "BadBean"){
            Debug.Log("BadBean has entered");
            NbOfBB++;
        }
        if (collider.gameObject.tag == "GoodBean"){
            Debug.Log("GoodBean has entered");
            NbOfGB++;
        }
    }

    void OnTriggerExit(Collider collider){
        Debug.Log("Has collided");
        if (collider.gameObject.tag == "BadBean"){
            Debug.Log("BadBean has left");
            NbOfBB--;
        }
        if (collider.gameObject.tag == "GoodBean"){
            Debug.Log("GoodBean has left");
            NbOfGB--;
        }
    }
}