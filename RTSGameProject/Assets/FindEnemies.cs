using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindEnemies : MonoBehaviour
{
    public UnitStats unitStats;
    public List<(float, GameObject)> enemiesInRange;
    public int numberOfEnemiesInRange;
    // Start is called before the first frame update
    void Start()
    {
        this.enemiesInRange = new List<(float Distance, GameObject Object)>();
        InvokeRepeating("CheckForEnemies", 0f, 0.5f);
        numberOfEnemiesInRange = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void CheckForEnemies()
    {
        var possibleEnemies = new List<GameObject>();
        var newEnemiesInRange = new List<(float, GameObject)>();
        Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, unitStats.GetVisionRange());
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.tag == unitStats.GetEnemyTag())
            {
                possibleEnemies.Add(hitCollider.gameObject);
            }
        }


        foreach (var enemy in possibleEnemies)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, enemy.transform.position - transform.position, out hit, unitStats.GetVisionRange()))
            {
                if (hit.collider.gameObject == enemy)
                {
                    newEnemiesInRange.Add((Vector3.Distance(transform.position, enemy.transform.position), enemy));
                }
            }   
        }

        newEnemiesInRange.Sort((x, y) => x.Item1.CompareTo(y.Item1));
        this.enemiesInRange = newEnemiesInRange;
        this.numberOfEnemiesInRange = enemiesInRange.Count;
    }

}
