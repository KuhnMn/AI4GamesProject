using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeanInfo : MonoBehaviour{
    public int health = 10;
    public string team = "good";

    // Start is called before the first frame update
    void Start(){
    }

    void Update(){
        if(health <= 0){
            Destroy(this.gameObject);
        }
    }

    public void getDamaged(int losedHealth){
        health -= losedHealth;
    }
}
