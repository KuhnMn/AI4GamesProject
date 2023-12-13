using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Formation : MonoBehaviour{
    
    
    public List<GameObject> unitList = new List<GameObject>();
    

    //public String status;
    //public bool fighting;
    /*
    Moving towards
    engaged -> stop ToFollow
    retreat
    */

    private int _unitWidth;
    public float _unitSpace = 2.5f;

    // Start is called before the first frame update
    void Start(){
        _unitWidth = Mathf.RoundToInt(Mathf.Sqrt(unitList.Count));
    }

    // Update is called once per frame
    void Update(){
        var offset = new Vector3((this.transform.position.x - (_unitWidth * _unitSpace) * 0.5f), 0, (this.transform.position.z - (_unitWidth * _unitSpace) * 0.5f));
        //var length = Mathf.Sqrt(Mathf.Pow(this.transform.position.x - offset.x,2) + Mathf.Pow(this.transform.position.y - offset.y, 2));
        //var pos = new Vector3((offset.x + Mathf.Cos(this.transform.rotation.y) * length), 0, (offset.z + Mathf.Sin(this.transform.rotation.y * length)));
        var pos = offset;
        /*
        Debug.Log("Original");
        Debug.Log(offset.x);
        Debug.Log(offset.z);
        Debug.Log("Angle");
        Debug.Log(this.transform.rotation.y);
        Debug.Log(Mathf.Cos(this.transform.rotation.y));
        Debug.Log(Mathf.Sin(this.transform.rotation.y));
        Debug.Log("New");
        Debug.Log(offset.x + Mathf.Cos(this.transform.rotation.y));
        Debug.Log(offset.z + Mathf.Sin(this.transform.rotation.y));
        Debug.Log("Over");*/
        
        int i = 0;
        float addedX = 0;
        //var pos = offset;

        foreach(GameObject unit in unitList){
            if(unit.GetComponent<MoveTo>() == null){
                unit.AddComponent<MoveTo>().goal = pos;
            }
            unit.GetComponent<MoveTo>().goal = pos;
            unit.GetComponent<MoveTo>().speed = unit.GetComponent<UnitStats>().moveSpeed * 2;
            i++;
            addedX += _unitSpace;
            if(i >= _unitWidth){
                pos += new Vector3(-addedX,0,_unitSpace);
                i = 0;
                addedX = 0;
            }
            pos.x += _unitSpace;
        }
/*
        if(fighting == true){
            foreach(GameObject unit in unitList){

            }
        }*/

        if(unitList.Count == 0){
            Destroy(gameObject);
        }
    }
}
