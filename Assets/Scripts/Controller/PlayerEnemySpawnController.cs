using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerEnemySpawnController : SceneSingleton<PlayerEnemySpawnController>
{
    public List<GameObject> Characters = new();
    private Dictionary<int, GameObject> CharacterInstances = new();

    public List<GameObject> Monsters = new();
    private Dictionary<int, GameObject> MonsterInstances = new();

    // 소환 가능한 플레이어 캐릭터 수
    public int MaxCharacterSpawn { get; private set; } = 1;

    public List<GameObject> GetCharacterInstances()
    {
        return CharacterInstances.Values.ToList();
    }
    
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
        // 2024-08-26 -> 랜덤 위치 및 가까운 목적지로 이등 구현
        // Debug.Log("몬스터 생성");
        
        Vector3 randomPos = PhaseManager.Instance.GetRandomSpawnPosition(); // 랜덤 위치 가져오기
        GameObject prefab = Monsters[Random.Range(0, Monsters.Count)]; // 적 종류에서 랜덤으로 가져오기
        
        //GameObject instance = Instantiate(prefab, position, rotation); // 고정 위치 생성
        GameObject instance = Instantiate(prefab, randomPos, rotation); // 랜덤 위치 생성
        
        // 가장 가까운 Destination으로 이동
        EnemyController enemyController = instance.GetComponent<EnemyController>();
        enemyController.SetNearDestination();
        
        MonsterInstances.Add(instance.GetInstanceID(), instance);
        return instance;
    }

    // 플레이어 소환 가능한 횟수 증가 (적 X)
    public void IncreaseMaxCharacterCount(int count)
    {
        MaxCharacterSpawn += count;
    }
}
