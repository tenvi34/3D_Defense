using System.Collections;
using UnityEngine;

public class Item : MonoBehaviour
{
    private GameObject Player;
    public float TraceDistance = 0.0f;
    public float ArrivedDistance = 0.0f;
    public bool StartedTrace = false;
    public Sprite itemSprite;

    IEnumerator TraceProcess()
    {
        float normalizedTime = 0.0f;
        float timeFactor = 0.5f;
        
        while (true)
        {
            Vector3 newItemPosition = Vector3.Slerp(transform.position, Player.transform.position, normalizedTime);
            transform.position = newItemPosition;
            
            if (ArrivedDistance >= (Player.transform.position-transform.position).magnitude)
            {
                transform.position = Player.transform.position;
                Player.GetComponent<PlayerController>().ArriveItem(gameObject.GetComponent<Item>());
                Destroy(gameObject);
                yield break;
            }
            
            normalizedTime += Time.deltaTime * timeFactor;
            yield return null;
        }
    }

    void Update()
    {
        if (StartedTrace)
            return;

        var players = PlayerEnemySpawnController.Instance.GetCharacterInstances();
        
        foreach (var player in players)
        {
            float length = (player.transform.position - transform.position).magnitude;
            if (TraceDistance >= length)
            {
                StartedTrace = true;
                Player = player;
                StartCoroutine(TraceProcess());
            }   
        }
    }
}
