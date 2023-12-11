using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameInfo : MonoBehaviour{
    private float Timer = 0;

    public int GBScore;
    public int BBScore;
    public int GBPoints;
    public int BBPoints;

    public int TotalGBUnitPoints;
    public int TotalBBUnitPoints;
    public int GBUnitPoints;
    public int BBUnitPoints;

    public List<GameObject> CapturePointList = new List<GameObject>();

    [SerializeField] private Text GBScoreText;
    [SerializeField] private Text BBScoreText;
    
    [SerializeField] private Text GBUnitPointsText;
    [SerializeField] private Text BBUnitPointsText;

    // Start is called before the first frame update
    void Start(){
        GBScore = BBScore = GBPoints = BBPoints = 0;
        TotalGBUnitPoints = TotalBBUnitPoints = 10;
        GBUnitPoints = BBUnitPoints = 0;
    }

    // Update is called once per frame
    void Update(){
        Timer += Time.deltaTime;

        if(GBScore >= 1000 || BBScore >= 1000){
            Debug.Log("VICTORY");
        }
        
        if(Timer>1){
            GBPoints = BBPoints = 0;
            GBUnitPoints = BBUnitPoints = 4;

            foreach(GameObject CpPoints in CapturePointList){
                if(CpPoints.tag == "GoodBean"){
                    GBPoints++;
                    GBUnitPoints--;
                }
                if(CpPoints.tag == "BadBean"){
                    BBPoints++;
                    BBUnitPoints--;
                }
            }
            GBScore += GBPoints;
            BBScore += BBPoints;
            TotalGBUnitPoints += GBUnitPoints;
            TotalBBUnitPoints += BBUnitPoints;

            GBScoreText.text = GBScore.ToString();
            BBScoreText.text = BBScore.ToString();

            GBUnitPointsText.text = TotalGBUnitPoints.ToString();
            BBUnitPointsText.text = TotalBBUnitPoints.ToString();

            Timer = 0;
        }
    }
}
