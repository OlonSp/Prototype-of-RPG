using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageBots : MonoBehaviour
{

    /// <summary>
    /// —писок мест возрождени€
    /// </summary>
    [SerializeField] List<GameObject> spawnPlaces = new List<GameObject>();
    /// <summary>
    /// Ўаблон бота
    /// </summary>
    [SerializeField] GameObject EnemyPrefab;
    //игрок
    [SerializeField] GameObject player;

    /// <summary>
    /// —писок текущих ботов
    /// </summary>
    [SerializeField] List<GameObject> bots = new List<GameObject>();

    [Tooltip("¬рем€ возрождени€ в секундах")]
    public float RespawnTime;

    // Use this for initialization
    void Start()
    {
        //Ќа каждой точке возрождени€ по€вл€етс€ бот
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
        //если бот убит запускаетс€ возрождение и мЄртвый бот исключаетс€ из списка текущих ботов 
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
        
        //проверка того, что точка возрождени€ не зан€та другим ботом
        foreach (var botik in bots)
            if (spawnPlaces[index].GetComponent<Collider>().bounds.Contains(botik.transform.position))
                return;
        foreach (var botik in bots)
            if (Vector3.Distance(botik.transform.position, spawnPlaces[index].transform.position) < 1.5f)
                return;
        //создание бота
        var bot = GameObject.Instantiate(EnemyPrefab);
        bot.transform.position = spawnPlaces[index].transform.position;
        bot.GetComponent<EnemyUi>().target = player;
        bots.Add(bot);
    }

    IEnumerator SpawnBots()
    {
        //врем€ до респавна бота
        yield return new WaitForSeconds(RespawnTime);
        //спавн бота
        SpawnBot();
    }
}

