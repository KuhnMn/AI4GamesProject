using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Formation : MonoBehaviour{
    
    public List<UnityEngine.AI.NavMeshAgent> unitList = new List<UnityEngine.AI.NavMeshAgent>();
    public Transform ToFollow;

    private int _unitWidth;
    public float _unitSpace = 2.5f;

    // Start is called before the first frame update
    void Start(){
        _unitWidth = Mathf.RoundToInt(Mathf.Sqrt(unitList.Count));
    }

    // Update is called once per frame
    void Update(){
        var offset = new Vector3(ToFollow.position.x - (_unitWidth * 0.5f * _unitSpace), 0, ToFollow.position.z - (_unitWidth * 0.5f * _unitSpace));
         
        int i = 0;
        float addedX = 0;
        var pos = offset;

        foreach(UnityEngine.AI.NavMeshAgent unit in unitList){
            unit.SetDestination(pos);
            i++;
            addedX += _unitSpace;
            if(i >= _unitWidth){
                pos += new Vector3(-addedX,0,_unitSpace);
                i = 0;
                addedX = 0;
            }
            pos.x += _unitSpace;
        }

        if(unitList.Count == 0){
            Destroy(gameObject);
        }
    }
}
