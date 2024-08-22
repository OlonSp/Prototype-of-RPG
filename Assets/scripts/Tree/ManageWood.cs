using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageWood : MonoBehaviour
{
    // ������ ���� �����������
    [SerializeField] List<GameObject> spawnPlaces = new List<GameObject>();

    // ������ ������
    [SerializeField] GameObject WoodPrefab;

    // ������ ������� ��������
    [SerializeField] List<GameObject> woods = new List<GameObject>();

    //������ � ���������
    [SerializeField] InventoryManager inventory;

    [Tooltip("����� ����������� � ��������")]
    public float RespawnTime;

    private void Start()
    {
        //�� ������ ����� ����������� ���������� ������
        foreach (var spawnPlace in spawnPlaces)
        {
            //�������� ������
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
        //���� ������ ��������, ��� �������� �� ������ �������� � ���������� ������ ������ �����
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

        //�������� ����, ��� ����� ����������� �� ������ ������ �������
        foreach (var plant in woods)
            if (spawnPlaces[index].GetComponent<Collider>().bounds.Contains(plant.transform.position))
                return;
        foreach (var plant in woods)
            if (Vector3.Distance(plant.transform.position, spawnPlaces[index].transform.position) < 1.5f)
                return;
        //�������� ������
        var wood = GameObject.Instantiate(WoodPrefab);
        Vector3 newWoodTransform = spawnPlaces[index].transform.position;
        newWoodTransform.y -= 1f;
        wood.transform.position = newWoodTransform;
        wood.GetComponent<WoodAI>().inventory = inventory;
        woods.Add(wood);
    }

    IEnumerator SpawnWoods()
    {
        //����� �� �������� ������
        yield return new WaitForSeconds(RespawnTime);
        //����� ������
        SpawnWood();
    }
}
