using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnemySpawnController : SceneSingleton<PlayerEnemySpawnController>
{
    public List<GameObject> Characters = new();
    private Dictionary<int, GameObject> CharacterInstances = new();

    public List<GameObject> Monsters = new();
    private Dictionary<int, GameObject> MonsterInstances = new();

    // 소환 가능한 플레이어 캐릭터 수
    public int MaxCharacterSpawn { get; private set; } = 1;
    
    public GameObject GetNewCharacter(Vector3 position, Quaternion rotation)
    {
        if (CharacterInstances.Count >= MaxCharacterSpawn)
        {
            Debug.Log("소환 가능한 수 초과");
            return null;
        }
        
        // Debug.Log("플레이어 캐릭터 생성");
        GameObject prefab = Characters[Random.Range(0, Characters.Count)];
        GameObject instance = Instantiate(prefab, position, rotation);
        CharacterInstances.Add(instance.GetInstanceID(), instance);
        return instance;
    }
    
    public GameObject GetNewEnemy(Vector3 position, Quaternion rotation)
    {
        // Debug.Log("몬스터 생성");
        Vector3 randomPos = PhaseManager.Instance.GetRandomSpawnPosition();
        GameObject prefab = Monsters[Random.Range(0, Monsters.Count)];
        //GameObject instance = Instantiate(prefab, position, rotation); // 고정 위치 생성
        GameObject instance = Instantiate(prefab, randomPos, rotation); // 랜덤 위치 생성
        
        // 가장 가까운 Destination으로 이동
        EnemyController enemyController = instance.GetComponent<EnemyController>();
        enemyController.SetNearDestination();
        
        MonsterInstances.Add(instance.GetInstanceID(), instance);
        return instance;
    }

    public void IncreaseMaxCharacterCount(int count)
    {
        MaxCharacterSpawn += count;
    }
}
