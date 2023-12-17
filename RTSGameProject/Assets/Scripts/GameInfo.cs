using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameInfo : MonoBehaviour{
    private float Timer = 0;
    [SerializeField] private Text ClockText;
    private float clock;
    public int seconds;
    public int minutes;
    public int hours;

    public int GBScore;
    public int BBScore;
    public int GBPoints;
    public int BBPoints;

    public List<GameObject> CapturePointList = new List<GameObject>();

    [SerializeField] private Text GBScoreText;
    [SerializeField] private Text BBScoreText;

    // Start is called before the first frame update
    void Start(){
        GBScore = BBScore = GBPoints = BBPoints = 0;
    }

    // Update is called once per frame
    void Update(){
        Timer += Time.deltaTime;
        clock += Time.deltaTime;

        if(GBScore >= 1000){
            SceneManager.LoadScene("GBVictory");
        }
        if(BBScore >= 1000){
            SceneManager.LoadScene("BBVictory");
        }
        
        if(Timer>1){
            GBPoints = BBPoints = 0;

            foreach(GameObject CpPoints in CapturePointList){
                if(CpPoints.tag == "GoodBean"){
                    GBPoints++;
                }
                if(CpPoints.tag == "BadBean"){
                    BBPoints++;
                }
            }
            GBScore += GBPoints;
            BBScore += BBPoints;

            GBScoreText.text = GBScore.ToString();
            BBScoreText.text = BBScore.ToString();

            Timer = 0;
        }

        seconds = Mathf.RoundToInt(clock);
        if(seconds > 60){
            minutes++;
            seconds = 0;
            clock = 0;
        }
        if(minutes > 60){
            hours++;
            minutes = 0;
        }
        ClockText.text = minutes.ToString() + ":" + seconds.ToString();
    }
}
