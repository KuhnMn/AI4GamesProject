using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderAI : MonoBehaviour{
    private float Timer = 0;
    
    public string team;
    public int Mood;

    public int Priority;

    //public int HighPriority;
    //public int LowPriority;

    public List<GameObject> Army = new List<GameObject>();
    public List<GameObject> Formations = new List<GameObject>();
    public GameObject FormationPrefab;
    public List<GameObject> CapturePointList = new List<GameObject>();
    //public List<GameObject> SpawnPointList = new List<GameObject>();
    public List<GameObject> UnitTypeList = new List<GameObject>();

    public int TotalUnitPoints;
    public int UnitPoints;
    [SerializeField] private Text UnitPointsText;
    [SerializeField] private Text MoodText;


    // Start is called before the first frame update
    void Start(){
        Mood = (int) Random.Range(40,60);
        TotalUnitPoints = 10;
        team = gameObject.tag;
    }

    // Update is called once per frame
    void Update(){
        Timer += Time.deltaTime;

        if(Timer>1){
            UnitPoints = 4;
            foreach(GameObject CpPoints in CapturePointList){
                if(CpPoints.tag == team){
                    UnitPoints--;
                }
            }
            TotalUnitPoints += UnitPoints;

            UnitPointsText.text = TotalUnitPoints.ToString();

            Timer = 0;
        }
    }

    void SpawnUnit(int UnitNumber,Vector3 position){
        GameObject Bean = Instantiate(UnitTypeList[UnitNumber], position, Quaternion.identity);
        Bean.GetComponentsInChildren<IdleState>()[0].startPatrol = true;
        Army.Add(Bean);
    }
    
    void SpawnFormation(Vector3 position){
        GameObject Formation = Instantiate(FormationPrefab, position, Quaternion.identity);
        Formations.Add(Formation);
    }
}
