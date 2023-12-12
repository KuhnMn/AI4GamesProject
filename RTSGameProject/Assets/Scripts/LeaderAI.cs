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

    public List<GameObject> AvaibleUnits = new List<GameObject>();
    public List<GameObject> Formations = new List<GameObject>();
    public List<GameObject> CapturePointList = new List<GameObject>();
    //public List<GameObject> SpawnPointList = new List<GameObject>();
    
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
    }

    // Update is called once per frame
    void Update(){
        Timer += Time.deltaTime;

        //prototype spawn units
        if(TotalUnitPoints>10){
            GameObject Bean = SpawnUnit(2,CapturePointList[1].transform.position);
            AvaibleUnits.Add(Bean);
            Bean.AddComponent<MoveTo>().goal = CapturePointList[0].transform.position;
            TotalUnitPoints -= 10;
        }
        //prototype spawn formation
        if(AvaibleUnits.Count == 9){
            GameObject Formation = SpawnFormation(CapturePointList[1].transform.position);
            Formations.Add(Formation);
            List<GameObject> AvaibleUnitsCopie = new List<GameObject>(AvaibleUnits);  // !!! NOT WELL IMPLEMMENTED !!!
            foreach(GameObject unit in AvaibleUnitsCopie){
                Destroy(unit.GetComponent<MoveTo>());
                Formation.GetComponent<Formation>().unitList.Add(unit);
                AvaibleUnits.Remove(unit);
            }
            AvaibleUnitsCopie.Clear();
            Formation.AddComponent<MoveTo>().goal = CapturePointList[2].transform.position;
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

    GameObject SpawnUnit(int UnitNumber,Vector3 position){
        GameObject Bean = Instantiate(UnitTypeList[UnitNumber], position, Quaternion.identity);
        return Bean;
    }
    
    GameObject SpawnFormation(Vector3 position){
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
}
