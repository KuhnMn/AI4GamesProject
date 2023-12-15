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
    //public string ArmyChoice;
    public string Attitude;
    private bool DecHasTrigger;


    public int FormationCap = 0;
    public List<GameObject> AvaibleUnits = new List<GameObject>();
    public List<GameObject> Formations = new List<GameObject>();
    //public List<GameObject> Army = new List<GameObject>();
    //public List<List<GameObject>> Armys = new List<List<GameObject>>();
    public List<GameObject> CapturePointList = new List<GameObject>();
    public List<GameObject> SpawnPointList = new List<GameObject>();

    public List<GameObject> UnitTypeList = new List<GameObject>();
    public GameObject FormationPrefab;

    public int TotalUnitPoints;
    public int UnitPoints;
    [SerializeField] private Text UnitPointsText;
    [SerializeField] private Image MoodBar;
    [SerializeField] private Text MoodText;

    //Memory
    //public int LastAction;
    public GameObject Objective;
    //public List<GameObject> CPDefended = new List<GameObject>();
    public GameObject Reinforcing;
    public bool IsAttacking;


    // Start is called before the first frame update
    void Start(){
        MoodHasTrigger = DecHasTrigger = true;
        Mood = (int) Random.Range(20,80);
        IsAttacking = true;
        TotalUnitPoints = 150;
        team = gameObject.tag;
        FormationCap = 20;
        Reinforcing = SpawnPointList[0];
        Objective = CapturePointList[0];
        /*switch(Random.Range(0,4)){
            case 1: ArmyChoice = "Infantry"; break;
            case 2: ArmyChoice = "Archer"; break;
            case 3: ArmyChoice = "Cavalry"; break;
            case 4: ArmyChoice = "Militia"; break;
            default: ArmyChoice = "Balanced"; break;
        }*/
    }

    // Update is called once per frame
    void Update(){
        Timer += Time.deltaTime;
        
        //Mood handler
        MoodHandler();
        CheckAvaibleSpawnPoint();
        RecruteMilitia();
        if(TotalUnitPoints>120){
            RecruteArmy();
        }
        /*
        if(TotalUnitPoints>100){
            //SendFormationToPos(SpawnInfantryDivision(SpawnPointList[0]), CapturePointList[Random.Range(0, 3)].transform.position);
            //SendFormationToPos(SpawnArcherDivision(SpawnPointList[0]), CapturePointList[Random.Range(0, 3)].transform.position);
            SendFormationToPos(SpawnCavalryDivision(SpawnPointList[0]), CapturePointList[Random.Range(0, 3)].transform.position);
            //SendFormationToPos(SpawnMilitiaDivision(SpawnPointList[0]), CapturePointList[Random.Range(0, 3)].transform.position);
            TotalUnitPoints -= 100;
        }*/
        

        if(GameInfo.GetComponent<GameInfo>().seconds!=0 && GameInfo.GetComponent<GameInfo>().seconds % 40 == 0 && DecHasTrigger){
            ChangeObjective();
            MakeDecision();
            DecHasTrigger = false;
        }else if(GameInfo.GetComponent<GameInfo>().seconds % 400 != 0){
            DecHasTrigger = true;
        }

        if(IsAttacking){
            SendAllFormationToObjective();
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

        Formation.AddComponent<MoveTo>().goal = location.transform.position; //goal
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

        Formation.AddComponent<MoveTo>().goal = location.transform.position;
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
        }else{
            Formation.GetComponent<MoveTo>().goal = Destination;
        }
    }

    void SendAllFormationToObjective(){
        foreach(GameObject form in Formations){
            if(form.GetComponent<Formation>().FormationName != "Militia"){
                foreach(GameObject Formation in Formations){
                    if(form.GetComponent<MoveTo>().goal == null){
                        form.AddComponent<MoveTo>().goal = Objective.transform.position;
                    }else{
                        form.GetComponent<MoveTo>().goal = Objective.transform.position;
                    }
                }
            }
        }
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
        if(Mood > 100){
            Mood = 100;
        }
        if(Mood < 0){
            Mood = 0;
        }

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
                Mood -= 20;                                                                                         //--- Mood Change When Gain
            }
            if(point.tag != this.tag && SpawnPointList.Contains(point)){
                Mood += 20;                                                                                         //--- Mood Change When Lost
                SpawnPointList.Remove(point);
                switch(Attitude){
                    case "Defensive": 
                        Objective = point;
                        IsAttacking = true;
                        break;
                    case "Neutral":
                        if(Random.Range(0,10)<7){
                            Objective = point;
                            IsAttacking = true;
                        };
                        break;
                    case "Aggresive":
                        if(Random.Range(0,10)<2){
                            Objective = point;
                            IsAttacking = true;
                        };
                        break;
                }
            }
        }
    }

    bool CheckIfDefended(GameObject CapturePoint){
        bool IsDefended = false;
        foreach(GameObject Formation in Formations){
            if(Formation.GetComponent<Formation>().FormationName == "Militia" && Formation.GetComponent<MoveTo>().goal == CapturePoint.transform.position){
                IsDefended = true;
            }
        }
        return IsDefended;
    }

    //Recrutement
    void RecruteMilitia(){
        foreach(GameObject point in SpawnPointList){
            if(!CheckIfDefended(point) && TotalUnitPoints > 70 && point != SpawnPointList[0] && Formations.Count <= FormationCap){
                SpawnMilitiaDivision(point);
                TotalUnitPoints -= 70;
            }
        }
    }

    /*void RecruteScouts(GameObject CP){
        if(TotalUnitPoints > 120 && CP != SpawnPointList[0] && Formations.Count <= FormationCap){
            SpawnCavalryDivision(Reinforcing);
            TotalUnitPoints -= 120;
        }
    }*/
    
    void RecruteArmy(){
        switch(Random.Range(0,3)){
            case 0: 
                if(TotalUnitPoints > 80 && Formations.Count <= FormationCap){
                    SpawnInfantryDivision(Reinforcing);
                    TotalUnitPoints -= 80;
                };
                break;
            case 1: 
                if(TotalUnitPoints > 100 && Formations.Count <= FormationCap){
                    SpawnArcherDivision(Reinforcing);
                    TotalUnitPoints -= 100;
                };
                break;
            default:
                if(TotalUnitPoints > 120 && Formations.Count <= FormationCap){
                    SpawnCavalryDivision(Reinforcing);
                    TotalUnitPoints -= 120;
                };
                break;
        }
    }

    void MakeDecision(){
        switch(Attitude){
            case "Defensive":
                if(Random.Range(0,10)<2){
                    SendAllFormationToObjective();
                    IsAttacking = true;
                }else{
                    IsAttacking = false;
                }
                break;
            case "Neutral":
                if(Random.Range(0,10)<5){
                    SendAllFormationToObjective();
                    IsAttacking = true;
                }else{
                    IsAttacking = false;
                }
                break;
            case "Aggresive":
                if(Random.Range(0,10)<8){
                    SendAllFormationToObjective();
                    IsAttacking = true;
                }else{
                    IsAttacking = false;
                }
                break;
        }
    }

    void ChangeObjective(){
        bool needsDefend = false;
        if(!SpawnPointList.Contains(CapturePointList[0])){
            Objective = CapturePointList[0];
        }
        foreach(GameObject point in SpawnPointList){
            if(point != SpawnPointList[0] && point.GetComponent<CapturePoints>().IsContested){
                switch(Attitude){
                    case "Defensive": 
                        if(Random.Range(0,10)<8){
                            Objective = point;
                            IsAttacking = false;
                        }
                        break;
                    case "Neutral":
                        if(Random.Range(0,1)<1){
                            Objective = point;
                            IsAttacking = false;
                        }
                        break;
                    case "Aggresive":
                        if(Random.Range(0,10)<2){
                            Objective = point;
                            IsAttacking = false;
                        }
                        break;
                }
                needsDefend = true;
            }
        }
        if(!needsDefend){
            foreach(GameObject point in CapturePointList){
                if(point.GetComponent<CapturePoints>().tag != team && point != SpawnPointList[0]){
                    switch(Attitude){
                        case "Aggresive":
                            if(Random.Range(0,10)<8){Objective = point;}
                            break;
                        case "Neutral":
                            if(Random.Range(0,1)<1){Objective = point;}
                            break;
                        default: 
                            Objective = SpawnPointList[SpawnPointList.Count-1];
                            break;
                    }
                }
            }
        }
    }

    void ChangeReinforcing(){
        Reinforcing = SpawnPointList[SpawnPointList.Count-1];
    }
}
