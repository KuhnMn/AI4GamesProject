using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CapturePoints : MonoBehaviour{
    public string CapturedBy;
    public bool IsContested;
    public int ScoreBB = 50;
    public int ScoreGB = 50;

    public int NbOfGB = 0;
    public int NbOfBB = 0;

    private float Timer = 0;
    public int points;

    public Material[] material;
    [SerializeField] private RawImage Icon;
    private Color Green;
    private Color Red;

    Renderer rend;


    // Start is called before the first frame update
    void Start(){
        Green = new Color(0.2666667f,0.764706f,0.3686275f,1f);
        Red = new Color(0.7960784f,0.1254902f,0.1372549f,255f);

        rend= GetComponent<Renderer>();
        rend.enabled = true;
        rend.sharedMaterial = material[0];
        IsContested = true;

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
            transform.gameObject.tag = CapturedBy = "BadBean";
            rend.sharedMaterial = material[2];
            Icon.color = Red;
        }else if(ScoreGB==100){
            transform.gameObject.tag = CapturedBy = "GoodBean";
            rend.sharedMaterial = material[1];
            Icon.color = Green;
        }

        if(ScoreBB == 0 || ScoreBB == 100){
            IsContested = false;
        }else{
            IsContested = true;
        }
        
        if(Timer>1){
            if(ScoreBB<100){
                if(NbOfBB>NbOfGB){
                    ScoreBB += points;
                    ScoreGB -= points;   
                }
            }
            if(ScoreGB<100){
                if(NbOfBB<NbOfGB){
                    ScoreBB -= points;
                    ScoreGB += points;
                }
            }
            Timer = 0;
        }  
    }

    void OnTriggerEnter(Collider collider){
        if (collider.gameObject.tag == "BadBean"){
            NbOfBB++;
        }
        if (collider.gameObject.tag == "GoodBean"){
            NbOfGB++;
        }
    }

    void OnTriggerExit(Collider collider){
        if (collider.gameObject.tag == "BadBean"){
            NbOfBB--;
        }
        if (collider.gameObject.tag == "GoodBean"){
            NbOfGB--;
        }
    }
}