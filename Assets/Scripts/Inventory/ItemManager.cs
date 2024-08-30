using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        ItemEffectManager itemEffectManager = itemWrappingObject.AddComponent<ItemEffectManager>();
        
        wrappingItem.itemSprite = item.itemSprite;
        wrappingItem.itemEffectManager = itemEffectManager;
        itemEffectManager.effectType = item.itemEffectManager.effectType;
        itemEffectManager.effectValue = item.itemEffectManager.effectValue;
        itemEffectManager.effectDuration = item.itemEffectManager.effectDuration;
        
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
                //Instantiate(itemPrefab, hit.position, Quaternion.identity);
                GameObject newItem = Instantiate(itemPrefab, hit.position, Quaternion.identity);
                Item itemComponent = newItem.GetComponent<Item>();
                ItemEffectManager itemEffect = newItem.GetComponent<ItemEffectManager>();
                SetRandomEffect(itemEffect);
            }
            
            float randomTime = Random.Range(1.0f, 10.0f);
            yield return new WaitForSeconds(randomTime);
        }
    }

    private void SetRandomEffect(ItemEffectManager itemEffect)
    {
        itemEffect.effectType = Random.value < 0.5f ? ItemEffectManager.EffectType.Heal : ItemEffectManager.EffectType.AttackBoost;
        
        switch (itemEffect.effectType)
        {
            case ItemEffectManager.EffectType.Heal:
                itemEffect.effectValue = 30f;
                break;
            case ItemEffectManager.EffectType.AttackBoost:
                itemEffect.effectValue = Random.Range(0.1f, 0.3f); // 10% ~ 30% 증가
                itemEffect.effectDuration = 30f;
                break;
        }
    }
    
    public void UseItem(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= Inventory.Count || Inventory[slotIndex] == null)
            return;

        Item item = Inventory[slotIndex];
        item.ItemUse();

        Inventory[slotIndex] = null;
        Ui_Inventory.SetSpriteToSlot(slotIndex, null);

        Destroy(item.gameObject);
    }

    Vector3 GetRandomPointInTriangle(Vector3 vertex1, Vector3 vertex2, Vector3 vertex3)
    {
        // 삼각형 내부의 랜덤한 점을 찾기 위한 두 개의 랜덤 가중치 생성
        float weight1 = Random.value;
        float weight2 = Random.value;

        // 생성된 점이 삼각형 내부에 있도록 보정
        if (weight1 + weight2 > 1)
        {
            weight1 = 1 - weight1;
            weight2 = 1 - weight2;
        }

        // 남은 가중치 계산
        float weight3 = 1 - weight1 - weight2;

        // 각 꼭지점에 가중치를 적용하여 랜덤한 내부 점 계산
        Vector3 randomPoint = weight1 * vertex1 + weight2 * vertex2 + weight3 * vertex3;

        return randomPoint;
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

    public void OnItemSlotClick()
    {
        Debug.Log("아이템 슬롯 클릭");
    }
}
