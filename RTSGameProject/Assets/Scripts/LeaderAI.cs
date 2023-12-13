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
    public int FormationCap = 0;
    public List<GameObject> AvaibleUnits = new List<GameObject>();
    public List<GameObject> Formations = new List<GameObject>();
    public List<GameObject> CapturePointList = new List<GameObject>();
    public List<GameObject> SpawnPointList = new List<GameObject>();

    public List<GameObject> UnitTypeList = new List<GameObject>();
    public GameObject FormationPrefab;

    public int TotalUnitPoints;
    public int UnitPoints;
    [SerializeField] private Text UnitPointsText;
    [SerializeField] private Text MoodText;


    // Start is called before the first frame update
    void Start(){
        Mood = (int) Random.Range(40,60);
        TotalUnitPoints = 10;
        team = gameObject.tag;
        FormationCap = 20;
    }

    // Update is called once per frame
    void Update(){
        Timer += Time.deltaTime;

        if(TotalUnitPoints>100){
            SendFormationToPos(SpawnInfantryDivision(SpawnPointList[0]), CapturePointList[Random.Range(0, 3)].transform.position);
            SendFormationToPos(SpawnArcherDivision(SpawnPointList[0]), CapturePointList[Random.Range(0, 3)].transform.position);
            SendFormationToPos(SpawnCavalryDivision(SpawnPointList[0]), CapturePointList[Random.Range(0, 3)].transform.position);
            SendFormationToPos(SpawnMilitiaDivision(SpawnPointList[0]), CapturePointList[Random.Range(0, 3)].transform.position);
            TotalUnitPoints -= 100;
        }

        //Add unitpoints per second
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
    /*
    private GameObject SpawnUnit(int UnitNumber,Vector3 position){
        GameObject Bean = Instantiate(UnitTypeList[UnitNumber], position, Quaternion.identity);
        return Bean;
    }*/
    
    private GameObject SpawnFormation(GameObject location, int UnitId, int UnitNumber){
        GameObject Formation = Instantiate(FormationPrefab, location.transform.position, Quaternion.identity);
        for(int i = 0; i < UnitNumber; i++){
            GameObject Bean = Instantiate(UnitTypeList[UnitId], location.transform.position, Quaternion.identity);
            AvaibleUnits.Add(Bean);
            Bean.AddComponent<MoveTo>().goal = location.transform.position; //MoveTo
            Bean.GetComponent<UnitStats>().InFormation = Formation;
        }
        Formations.Add(Formation);

        List<GameObject> AvaibleUnitsCopie = new List<GameObject>(AvaibleUnits);
        foreach(GameObject unit in AvaibleUnitsCopie){
            Destroy(unit.GetComponent<MoveTo>());
            Formation.GetComponent<Formation>().unitList.Add(unit);
            AvaibleUnits.Remove(unit);
        }
        AvaibleUnitsCopie.Clear();

        Formation.AddComponent<MoveTo>().goal = CapturePointList[Random.Range(0, 3)].transform.position; //goal
        Formation.GetComponent<MoveTo>().speed = UnitTypeList[UnitId].GetComponent<UnitStats>().moveSpeed;
        return Formation;
    }

    private GameObject SpawnDuelUnitFormation(GameObject location, int Unit1Id, int Unit1Number, int Unit2Id, int Unit2Number){
        GameObject Formation = Instantiate(FormationPrefab, location.transform.position, Quaternion.identity);

        for(int i = 0; i < Unit1Number; i++){
            GameObject Bean = Instantiate(UnitTypeList[Unit1Id], location.transform.position, Quaternion.identity);
            AvaibleUnits.Add(Bean);
            Bean.AddComponent<MoveTo>().goal = location.transform.position;
            Bean.GetComponent<UnitStats>().InFormation = Formation;
        }
        for(int i = 0; i < Unit2Number; i++){
            GameObject Bean = Instantiate(UnitTypeList[Unit2Id], location.transform.position, Quaternion.identity);
            AvaibleUnits.Add(Bean);
            Bean.AddComponent<MoveTo>().goal = location.transform.position;
            Bean.GetComponent<UnitStats>().InFormation = Formation;
        }
            
        Formations.Add(Formation);
        List<GameObject> AvaibleUnitsCopie = new List<GameObject>(AvaibleUnits);
        foreach(GameObject unit in AvaibleUnitsCopie){
            Destroy(unit.GetComponent<MoveTo>());
            Formation.GetComponent<Formation>().unitList.Add(unit);
            AvaibleUnits.Remove(unit);
        }
        AvaibleUnitsCopie.Clear();

        Formation.AddComponent<MoveTo>().goal = CapturePointList[Random.Range(0, 3)].transform.position;
        float unit1Speed = UnitTypeList[Unit2Id].GetComponent<UnitStats>().moveSpeed;
        float unit2Speed = UnitTypeList[Unit2Id].GetComponent<UnitStats>().moveSpeed;
        if(unit1Speed < unit2Speed){
            Formation.GetComponent<MoveTo>().speed = unit1Speed;
        }else{
            Formation.GetComponent<MoveTo>().speed = unit2Speed;
        }
        return Formation;
    }

    void SendFormationToPos(GameObject Formation, Vector3 Destination){
        if(Formation.GetComponent<MoveTo>().goal == null){
            Formation.AddComponent<MoveTo>().goal = Destination;
        }
        Formation.GetComponent<MoveTo>().goal = Destination;
    }

    private GameObject SpawnInfantryDivision(GameObject location){
        return SpawnFormation(location,0,12);
    }

    private GameObject SpawnArcherDivision(GameObject location){
        return SpawnFormation(location,1,9);
    }

    private GameObject SpawnCavalryDivision(GameObject location){
        return SpawnFormation(location,2,9);
    }

    private GameObject SpawnMilitiaDivision(GameObject location){
        return SpawnDuelUnitFormation(location,0,8,1,4);
    }

    void lostUnit(){

    }
}
