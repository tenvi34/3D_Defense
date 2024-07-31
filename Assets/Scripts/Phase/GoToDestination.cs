using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class GoToDestination : Action
{
	private int DestinationIndex;
	private MonsterController _monster;
	
	public override void OnAwake()
	{
		base.OnAwake();
		_monster = gameObject.GetComponent<MonsterController>();
	}
	
	public override void OnStart()
	{
		DestinationIndex = 0;
	}

	public override TaskStatus OnUpdate()
	{
		(int, Vector3) destinationInfo = PhaseManager.Instance.GetDestination(DestinationIndex);
		DestinationIndex = destinationInfo.Item1;
		if (_monster.MoveToDestination(destinationInfo.Item2))
		{
			DestinationIndex++;
		}
		
		return TaskStatus.Success;
	}
}