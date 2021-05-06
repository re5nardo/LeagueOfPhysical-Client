using GameFramework;

public class CharacterStatusController : MonoComponentBase
{
	public void OnSelectableFirstStatusCount(int nSelectableFirstStatusCount)
	{
		CharacterStatusData characterStatusData = Entity.GetEntityComponent<CharacterStatusData>();

        characterStatusData.SelectableFirstStatusCount = nSelectableFirstStatusCount;
	}

	public void OnCharacterStatusChange(FirstStatus firstStatus, SecondStatus secondStatus)
	{
        CharacterStatusData characterStatusData = Entity.GetEntityComponent<CharacterStatusData>();

        characterStatusData.SetFirstStatus(firstStatus);
        characterStatusData.SetSecondStatus(secondStatus);
	}
}
