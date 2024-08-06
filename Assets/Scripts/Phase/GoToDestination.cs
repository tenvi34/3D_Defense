using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class GoToDestination : Action
{
	private int DestinationIndex;
	private EnemyController _enemy;
	
	public override void OnAwake()
	{
		base.OnAwake();
		_enemy = gameObject.GetComponent<EnemyController>();
	}
	
	public override void OnStart()
	{
		DestinationIndex = 0;
	}

	public override TaskStatus OnUpdate()
	{
		(int, Vector3) destinationInfo = PhaseManager.Instance.GetDestination(DestinationIndex);
		DestinationIndex = destinationInfo.Item1;
		if (_enemy.MoveToDestination(destinationInfo.Item2))
		{
			DestinationIndex++;
		}
		
		return TaskStatus.Success;
	}
}