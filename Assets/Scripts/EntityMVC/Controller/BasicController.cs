using Behavior;
using State;
using GameFramework;

public class BasicController : MonoControllerComponentBase
{
	#region Behavior
	//public void Move(Vector3 vec3Destination)
	//{
	//	Move oldMove = Entity.GetComponent<Move>();
	//	if (oldMove != null)
	//	{
	//		oldMove.SetDestination(vec3Destination);
	//	}
	//	else
	//	{
	//		Move move = BehaviorFactory.Instance.CreateBehavior(gameObject, MasterDataDefine.BehaviorID.MOVE) as Move;
	//		Entity.AttachComponent(move);
	//		move.SetData(MasterDataDefine.BehaviorID.MOVE, vec3Destination);
	//		move.onBehaviorEnd += BehaviorHelper.BehaviorDestroyer;

	//		move.StartBehavior();
	//	}

	//	Rotation oldRotation = Entity.GetComponent<Rotation>();
	//	if (oldRotation != null)
	//	{
	//		oldRotation.SetDestination(vec3Destination);
	//	}
	//	else
	//	{
	//		Rotation rotation = BehaviorFactory.Instance.CreateBehavior(gameObject, MasterDataDefine.BehaviorID.ROTATION) as Rotation;
	//		Entity.AttachComponent(rotation);
	//		rotation.SetData(MasterDataDefine.BehaviorID.ROTATION, vec3Destination);
	//		rotation.onBehaviorEnd += BehaviorHelper.BehaviorDestroyer;

	//		rotation.StartBehavior();
	//	}
	//}

	//public void Die()
	//{
	//	var behaviors = Entity.GetComponents<BehaviorBase>();
	//	foreach (var behavior in behaviors)
	//	{
	//		if (behavior.IsPlaying())
	//			behavior.StopBehavior();
	//	}

	//	var states = Entity.GetComponents<StateBase>();
	//	foreach (var state in states)
	//	{
	//		if (state.IsPlaying())
	//			state.StopState();
	//	}
	//}

	public void StartBehavior(int nBehaviorMasterID)
	{
		BehaviorBase behavior = BehaviorFactory.Instance.CreateBehavior(gameObject, nBehaviorMasterID);
		Entity.AttachComponent(behavior);
		behavior.SetData(nBehaviorMasterID);
		behavior.onBehaviorEnd += BehaviorHelper.BehaviorDestroyer;

		behavior.StartBehavior();
	}
	#endregion

	#region State
	public void StartState(int nStateMasterID)
	{
		StateBase state = StateFactory.Instance.CreateState(gameObject, nStateMasterID);
		Entity.AttachComponent(state);
		state.SetData(nStateMasterID);
		state.onStateEnd += StateHelper.StateDestroyer;

		state.StartState();
	}
	#endregion
}
