using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public float Speed = 5.0f;
    private int DestinationIndex;
	
    private Rigidbody _rb;
	
    // Start is called before the first frame update
    void Awake()
    {
        DestinationIndex = 0;
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        (int, Vector3) destinationInfo = PhaseManager.Instance.GetDestination(DestinationIndex);
        DestinationIndex = destinationInfo.Item1;
        if (MoveToDestination(destinationInfo.Item2))
        {
            DestinationIndex++;
        }
    }

    public bool MoveToDestination(Vector3 destination)
    {
        Vector3 nextPosition = Vector3.MoveTowards(transform.position, destination, Speed * Time.deltaTime);
        _rb.MovePosition(nextPosition);
        _rb.MoveRotation(Quaternion.LookRotation((nextPosition - transform.position).normalized));

        if (Vector3.Distance(destination, transform.position) <= 1.0f)
        {
            return true;
        }

        return false;
    }
}
