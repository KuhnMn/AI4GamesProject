using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeanSpawner : MonoBehaviour
{
    public GameObject beanPrefab;
    public UnitStats.Team team;

    public bool spawningEnabled;
    private float spawnTimer;
    private List<GameObject> backUpList;
    // Start is called before the first frame update
    void Start()
    {
        backUpList = new List<GameObject>();
        for (int i = 0; i < 5; i++)
        {
            GameObject bean = Instantiate(beanPrefab, transform.position + transform.up * -3, Quaternion.identity);
            bean.SetActive(false);
            backUpList.Add(bean);
        }
        this.spawnTimer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= 5.0f && spawningEnabled)
        {
            spawnTimer = 0.0f;
            foreach (var bean in backUpList)
            {
                if (!bean.activeSelf)
                {
                    bean.SetActive(true);
                    bean.GetComponent<UnitStats>().SetTeam(team);
                    bean.transform.position = transform.position + transform.up * 2;
                    bean.GetComponentsInChildren<IdleState>()[0].startPatrol = true;
                    break;
                }
            }
        }
    }
}
