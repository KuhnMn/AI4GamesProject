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
/*
        //prototype spawn units
        if(TotalUnitPoints>10){
            GameObject Bean = SpawnUnit(2,SpawnPointList[0].transform.position);
            AvaibleUnits.Add(Bean);
            Bean.AddComponent<MoveTo>().goal = SpawnPointList[0].transform.position;
            TotalUnitPoints -= 10;
        }
        //prototype spawn formation
        if(AvaibleUnits.Count == 9){
            GameObject Formation = SpawnFormation(SpawnPointList[0].transform.position);
            Formations.Add(Formation);
            List<GameObject> AvaibleUnitsCopie = new List<GameObject>(AvaibleUnits);  // !!! NOT WELL IMPLEMMENTED !!!
            foreach(GameObject unit in AvaibleUnitsCopie){
                Destroy(unit.GetComponent<MoveTo>());
                Formation.GetComponent<Formation>().unitList.Add(unit);
                AvaibleUnits.Remove(unit);
            }
            AvaibleUnitsCopie.Clear();
            int ranNum = Random.Range(0, 3);
            Formation.AddComponent<MoveTo>().goal = CapturePointList[Random.Range(0, 3)].transform.position;
            Formation.GetComponent<MoveTo>().speed = 5;
        }*/
        if(TotalUnitPoints>100){
            SendFormationToPos(CapturePointList[Random.Range(0, 3)], spawnInfantryDivision(SpawnPointList[0]));
            SendFormationToPos(CapturePointList[Random.Range(0, 3)], spawnArcherDivision(SpawnPointList[0]));
            SendFormationToPos(CapturePointList[Random.Range(0, 3)], spawnCavalryDivision(SpawnPointList[0]));
            SendFormationToPos(CapturePointList[Random.Range(0, 3)], spawnMilitiaDivision(SpawnPointList[0]));
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

    private GameObject SpawnUnit(int UnitNumber,Vector3 position){
        GameObject Bean = Instantiate(UnitTypeList[UnitNumber], position, Quaternion.identity);
        return Bean;
    }
    
    private GameObject SpawnFormation(Vector3 position){
        GameObject Formation = Instantiate(FormationPrefab, position, Quaternion.identity);
        return Formation;
    }

    void SendFormationToPos(GameObject Destination, GameObject Formation){
        Formation.GetComponent<MoveTo>().goal = Destination.transform.position;
    }

    void lostUnit(){

    }

    /*void LostFormation(GameObject Formation){
        Formations.Remove(Formation);
        Destroy(Formation);
    }*/

    private GameObject spawnInfantryDivision(GameObject location){
        GameObject Formation = SpawnFormation(location.transform.position);
        for(int i = 0; i < 12; i++){
            GameObject Bean = SpawnUnit(0,location.transform.position);
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
        Formation.GetComponent<MoveTo>().speed = 2;
        return Formation;
    }

    private GameObject spawnArcherDivision(GameObject location){
        GameObject Formation = SpawnFormation(location.transform.position);
        for(int i = 0; i < 9; i++){
            GameObject Bean = SpawnUnit(1,location.transform.position);
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
        Formation.GetComponent<MoveTo>().speed = 1;
        return Formation;
    }

    private GameObject spawnCavalryDivision(GameObject location){
        GameObject Formation = SpawnFormation(location.transform.position);
        for(int i = 0; i < 9; i++){
            GameObject Bean = SpawnUnit(2,location.transform.position);
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
        Formation.GetComponent<MoveTo>().speed = 4;
        return Formation;
    }

    private GameObject spawnMilitiaDivision(GameObject location){
        GameObject Formation = SpawnFormation(location.transform.position);
        for(int i = 0; i < 8; i++){
            GameObject Bean = SpawnUnit(0,location.transform.position);
            AvaibleUnits.Add(Bean);
            Bean.AddComponent<MoveTo>().goal = location.transform.position;
            Bean.GetComponent<UnitStats>().InFormation = Formation;
        }
        for(int i = 0; i < 4; i++){
            GameObject Bean = SpawnUnit(1,location.transform.position);
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
        Formation.GetComponent<MoveTo>().speed = 0.5f;
        return Formation;
    }
}
