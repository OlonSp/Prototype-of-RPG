using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageWood : MonoBehaviour
{
    // —писок мест возрождени€
    [SerializeField] List<GameObject> spawnPlaces = new List<GameObject>();

    // Ўаблон дерева
    [SerializeField] GameObject WoodPrefab;

    // —писок текущих деревьев
    [SerializeField] List<GameObject> woods = new List<GameObject>();

    //доступ к инвентарю
    [SerializeField] InventoryManager inventory;

    [Tooltip("¬рем€ возрождени€ в секундах")]
    public float RespawnTime;

    private void Start()
    {
        //Ќа каждой точке возрождени€ по€вл€етс€ дерево
        foreach (var spawnPlace in spawnPlaces)
        {
            //создание дерева
            var wood = GameObject.Instantiate(WoodPrefab);
            Vector3 newWoodTransform = spawnPlace.transform.position;
            newWoodTransform.y -= 1f;
            wood.transform.position = newWoodTransform;
            wood.GetComponent<WoodAI>().inventory = inventory;
            woods.Add(wood);
        }
    }

    // Update is called once per frame
    void Update()
    {   
        //если дерево срублено, оно исчезает из списка деревьев и начинаетс€ отсчЄт спавна новых
        for (var i = 0; i < woods.Count; i++)
            if (woods[i].GetComponent<WoodAI>().isExist == false)
            {
                StartCoroutine(SpawnWoods());
                woods.RemoveAt(i);
            }

    }

    void SpawnWood()
    {
        int index = Random.Range(0, spawnPlaces.Count);

        //проверка того, что точка возрождени€ не зан€та другим деревом
        foreach (var plant in woods)
            if (spawnPlaces[index].GetComponent<Collider>().bounds.Contains(plant.transform.position))
                return;
        foreach (var plant in woods)
            if (Vector3.Distance(plant.transform.position, spawnPlaces[index].transform.position) < 1.5f)
                return;
        //создание дерева
        var wood = GameObject.Instantiate(WoodPrefab);
        Vector3 newWoodTransform = spawnPlaces[index].transform.position;
        newWoodTransform.y -= 1f;
        wood.transform.position = newWoodTransform;
        wood.GetComponent<WoodAI>().inventory = inventory;
        woods.Add(wood);
    }

    IEnumerator SpawnWoods()
    {
        //врем€ до респавна дерева
        yield return new WaitForSeconds(RespawnTime);
        //спавн дерева
        SpawnWood();
    }
}
