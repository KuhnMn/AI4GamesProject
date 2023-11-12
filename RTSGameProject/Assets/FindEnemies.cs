using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindEnemies : MonoBehaviour
{
    public string enemyTag;
    public float visionRange;
    public List<(float, GameObject)> enemiesInRange;
    // Start is called before the first frame update
    void Start()
    {
        this.enemiesInRange = new List<(float Distance, GameObject Object)>();
        InvokeRepeating("CheckForEnemies", 0f, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void CheckForEnemies()
    {
        var possibleEnemies = new List<GameObject>();
        Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, visionRange);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.tag == enemyTag)
            {
                possibleEnemies.Add(hitCollider.gameObject);
            }
        }

        foreach (var enemy in possibleEnemies)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, enemy.transform.position - transform.position, out hit, visionRange))
            {
                if (hit.collider.gameObject == enemy)
                {
                    this.enemiesInRange.Add((Vector3.Distance(transform.position, enemy.transform.position), enemy));
                }
            }   
        }

        this.enemiesInRange.Sort((x, y) => x.Item1.CompareTo(y.Item1));
        foreach (var enemy in this.enemiesInRange)
        {
            Debug.Log(enemy.Item1);
            Debug.Log(enemy.Item2);
        }
    }

}
