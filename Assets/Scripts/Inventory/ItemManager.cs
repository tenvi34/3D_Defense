using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class ItemManager : SceneSingleton<ItemManager>
{
    public GameObject itemPrefab;
    public GameObject player;

    public UI_Inventory Ui_Inventory;
    public List<Item> Inventory = new();
    
    public void AddItem(Item item)
    {
        GameObject itemWrappingObject = new GameObject();
        Item wrappingItem = itemWrappingObject.AddComponent<Item>();
        wrappingItem.itemSprite = item.itemSprite;
        
        int settedIndex = -1;
        for (int i = 0; i < Inventory.Count; ++i)
        {
            if (Inventory[i] == null)
            {
                Inventory[i] = wrappingItem;
                settedIndex = i;
                break;
            }
        }
        
        UpdateSlotUIs(wrappingItem, settedIndex);
    }

    private void UpdateSlotUIs(Item item, int settedIndex)
    {
        if (settedIndex >= 0)
            Ui_Inventory.SetSpriteToSlot(settedIndex, item.itemSprite);
    }

    IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            NavMeshTriangulation newMeshData = NavMesh.CalculateTriangulation();

            int randomIndex = Random.Range(0, newMeshData.indices.Length - 3);

            Vector3 v1 = newMeshData.vertices[newMeshData.indices[randomIndex]];
            Vector3 v2 = newMeshData.vertices[newMeshData.indices[randomIndex + 1]];
            Vector3 v3 = newMeshData.vertices[newMeshData.indices[randomIndex + 2]];

            Vector3 center = GetRandomPointInTriangle(v1, v2, v3);

            NavMeshHit hit;
            int layer = 1 << NavMesh.GetAreaFromName("Walkable");
            if (NavMesh.SamplePosition(center, out hit, 1000.0f, layer))
            {
                
                Instantiate(itemPrefab, hit.position, Quaternion.identity);
            }
            
            float randomTime = Random.Range(1.0f, 10.0f);
            yield return new WaitForSeconds(randomTime);
        }
    }

    Vector3 GetRandomPointInTriangle(Vector3 v1, Vector3 v2, Vector3 v3)
    {
        float r1 = Random.value;
        float r2 = Random.value;
        if (r1 + r2 > 1)
        {
            r1 = 1 - r1;
            r2 = 1 - r2;
        }

        return v1 + r1 * (v2 - v1) + r2 * (v3 - v1);
    }
    
    void Start()
    {
        for (int i = 0; i < Ui_Inventory.SlotCount; ++i)
        {
            Inventory.Add(null);
        }
        
        StartCoroutine(SpawnCoroutine());
    }

    void Update()
    {
        // 클릭해서 아이템 사용 기능 구현
        
    }
}
