using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageBots : MonoBehaviour
{

    /// <summary>
    /// ������ ���� �����������
    /// </summary>
    [SerializeField] List<GameObject> spawnPlaces = new List<GameObject>();
    /// <summary>
    /// ������ ����
    /// </summary>
    [SerializeField] GameObject EnemyPrefab;
    //�����
    [SerializeField] GameObject player;

    /// <summary>
    /// ������ ������� �����
    /// </summary>
    [SerializeField] List<GameObject> bots = new List<GameObject>();

    [Tooltip("����� ����������� � ��������")]
    public float RespawnTime;

    // Use this for initialization
    void Start()
    {
        //�� ������ ����� ����������� ���������� ���
        foreach(var spawnPlace in spawnPlaces)
        {
            var bot = GameObject.Instantiate(EnemyPrefab);
            bot.transform.position = spawnPlace.transform.position;
            bot.GetComponent<EnemyUi>().target = player;
            bots.Add(bot);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //���� ��� ���� ����������� ����������� � ������ ��� ����������� �� ������ ������� ����� 
        for (var i = 0; i < bots.Count; i++)
            if (bots[i].GetComponent<EnemyUi>().isAlive == false)
            {
                StartCoroutine(SpawnBots());
                bots.RemoveAt(i);
            }

    }

    void SpawnBot()
    {
        int index = Random.Range(0, spawnPlaces.Count);
        
        //�������� ����, ��� ����� ����������� �� ������ ������ �����
        foreach (var botik in bots)
            if (spawnPlaces[index].GetComponent<Collider>().bounds.Contains(botik.transform.position))
                return;
        foreach (var botik in bots)
            if (Vector3.Distance(botik.transform.position, spawnPlaces[index].transform.position) < 1.5f)
                return;
        //�������� ����
        var bot = GameObject.Instantiate(EnemyPrefab);
        bot.transform.position = spawnPlaces[index].transform.position;
        bot.GetComponent<EnemyUi>().target = player;
        bots.Add(bot);
    }

    IEnumerator SpawnBots()
    {
        //����� �� �������� ����
        yield return new WaitForSeconds(RespawnTime);
        //����� ����
        SpawnBot();
    }
}

