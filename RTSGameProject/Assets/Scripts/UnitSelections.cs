using System.Collections.Generic;
using UnityEngine;

public class UnitSelections : MonoBehaviour{

    public List<GameObject> unitList = new List<GameObject>();
    public List<GameObject> unitSelected = new List<GameObject>();

    private static UnitSelections _instance;
    public static UnitSelections Instance{ get { return _instance; } }

    void Awake(){
        if(_instance != null && _instance != this){
            Destroy(this.gameObject);
        }else{
            _instance = this;
        }
    }

    public void clickSelect(GameObject unitToAdd){
        deselectAll();
        unitSelected.Add(unitToAdd);
    }

    public void shiftClickSelect(GameObject unitToAdd){
        if(!unitSelected.Contains(unitToAdd)){
            unitSelected.Add(unitToAdd);
        }else{
            unitSelected.Remove(unitToAdd);
        }
    }

    public void dragSelect(GameObject unitToAdd){
        
    }

    public void deselectAll(){
        unitSelected.Clear();
    }

    public void deselect(GameObject unitToDeselect){

    }
}
