using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderAI : MonoBehaviour{
    private float Timer = 0;
    public GameObject GameInfo;
    
    public string team;
    public int Mood;
    private bool MoodHasTrigger;
    public string ArmyChoice;
    public string Attitude;

    public int Priority;

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
    [SerializeField] private Image MoodBar;
    [SerializeField] private Text MoodText;


    // Start is called before the first frame update
    void Start(){
        Mood = (int) Random.Range(40,60);
        TotalUnitPoints = 10;
        team = gameObject.tag;
        FormationCap = 20;
        switch(Random.Range(0,4)){
            case 1: ArmyChoice = "Infantry"; break;
            case 2: ArmyChoice = "Archer"; break;
            case 3: ArmyChoice = "Cavalry"; break;
            case 4: ArmyChoice = "Militia"; break;
            default: ArmyChoice = "Balanced"; break;
        }
    }

    // Update is called once per frame
    void Update(){
        Timer += Time.deltaTime;
        
        //Mood handler
        MoodHandler();
        CheckAvaibleSpawnPoint();


        if(TotalUnitPoints>100){
            SendFormationToPos(SpawnInfantryDivision(SpawnPointList[0]), CapturePointList[Random.Range(0, 3)].transform.position);
            SendFormationToPos(SpawnArcherDivision(SpawnPointList[0]), CapturePointList[Random.Range(0, 3)].transform.position);
            SendFormationToPos(SpawnCavalryDivision(SpawnPointList[0]), CapturePointList[Random.Range(0, 3)].transform.position);
            SendFormationToPos(SpawnMilitiaDivision(SpawnPointList[0]), CapturePointList[Random.Range(0, 3)].transform.position);
            TotalUnitPoints -= 100;
        }

        /*
        1: Check Score
        2: Check Capture Points
        3: Checks Army
        4.1: Recrutement
        4.2: Launch Assault
        5: 
        */

        //2


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
    
    private GameObject SpawnFormation(GameObject location, int UnitId, int UnitNumber, string FormationName){
        GameObject Formation = Instantiate(FormationPrefab, location.transform.position, Quaternion.identity);
        Formation.GetComponent<Formation>().FormationName = FormationName;

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

    private GameObject SpawnDuelUnitFormation(GameObject location, int Unit1Id, int Unit1Number, int Unit2Id, int Unit2Number, string FormationName){
        GameObject Formation = Instantiate(FormationPrefab, location.transform.position, Quaternion.identity);
        Formation.GetComponent<Formation>().FormationName = FormationName;

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
        return SpawnFormation(location,0,12,"Infantry");
    }

    private GameObject SpawnArcherDivision(GameObject location){
        return SpawnFormation(location,1,9,"Archer");
    }

    private GameObject SpawnCavalryDivision(GameObject location){
        return SpawnFormation(location,2,9,"Cavalry");
    }

    private GameObject SpawnMilitiaDivision(GameObject location){
        return SpawnDuelUnitFormation(location,0,8,1,4,"Militia");
    }

    void MoodHandler(){
        if(GameInfo.GetComponent<GameInfo>().seconds % 5 == 0 && MoodHasTrigger){
            if(Mood < 100){
                Mood--;
            }
            MoodHasTrigger = false;
        }else if(GameInfo.GetComponent<GameInfo>().seconds % 5 != 0){
            MoodHasTrigger = true;
        }
        
        float moodBarAmount = 1.0f - ((Mood * 1.0f) / (100.0f));
        MoodBar.fillAmount = moodBarAmount;
        if(moodBarAmount * 100 < 30){
            MoodBar.color = new Color(0f,0f,1f);
            Attitude = "Defensive";
        }else if((moodBarAmount * 100) >= 30 && (moodBarAmount * 100 < 70)){
            MoodBar.color = new Color(0f,1f,0f);
            Attitude = "Neutral";
        }else{
            MoodBar.color = new Color(0.7169812f,0f,0.005623296f);
            Attitude = "Aggresive";
        }
    }


    List<int> GetArmyNumbers(){
        int nbInf = 0;
        int nbArch = 0;
        int nbCalv = 0;
        int nbMil = 0;
        List<int> formationList = new List<int>();
        foreach(GameObject formation in Formations){
            switch(formation.GetComponent<Formation>().FormationName){
                case "Infantry": nbInf++; break;
                case "Archer": nbArch++; break;
                case "Calvalry": nbCalv++; break;
                case "Militia": nbMil++; break;
            }
        }
        formationList.Add(nbInf);
        formationList.Add(nbArch);
        formationList.Add(nbCalv);
        formationList.Add(nbMil);
        return formationList;
    }

    void CheckAvaibleSpawnPoint(){
        foreach(GameObject point in CapturePointList){
            if(point.tag == this.tag && !SpawnPointList.Contains(point)){
                SpawnPointList.Add(point);
            }
            if(point.tag != this.tag && SpawnPointList.Contains(point)){
                SpawnPointList.Remove(point);
            }
        }
    }

    //Recutement
    void Recutement(){

    }

    //Tactics
    
}
