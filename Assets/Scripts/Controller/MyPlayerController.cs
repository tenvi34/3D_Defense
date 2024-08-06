using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayerController : SceneSingleton<MyPlayerController>
{
    public List<GameObject> Characters = new();
    private Dictionary<int, GameObject> CharacterInstances = new();

    public List<GameObject> Monsters = new();
    private Dictionary<int, GameObject> MonsterInstances = new();
    
    public GameObject GetNewCharacter(Vector3 position, Quaternion rotation)
    {
        // Debug.Log("플레이어 캐릭터 생성");
        GameObject prefab = Characters[Random.Range(0, Characters.Count)];
        GameObject instance = Instantiate(prefab, position, rotation);
        CharacterInstances.Add(instance.GetInstanceID(), instance);
        return instance;
    }
    
    public GameObject GetNewMonster(Vector3 position, Quaternion rotation)
    {
        // Debug.Log("몬스터 생성");
        GameObject prefab = Monsters[Random.Range(0, Monsters.Count)];
        GameObject instance = Instantiate(prefab, position, rotation);
        MonsterInstances.Add(instance.GetInstanceID(), instance);
        return instance;
    }
}
