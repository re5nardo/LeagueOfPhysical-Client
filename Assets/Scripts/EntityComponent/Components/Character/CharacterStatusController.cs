using GameFramework;

public class CharacterStatusController : MonoComponentBase
{
	public void OnSelectableFirstStatusCount(int nSelectableFirstStatusCount)
	{
		CharacterStatusData characterStatusData = Entity.GetComponent<CharacterStatusData>();

        characterStatusData.SelectableFirstStatusCount = nSelectableFirstStatusCount;
	}

	public void OnCharacterStatusChange(FirstStatus firstStatus, SecondStatus secondStatus)
	{
        CharacterStatusData characterStatusData = Entity.GetComponent<CharacterStatusData>();

        characterStatusData.SetFirstStatus(firstStatus);
        characterStatusData.SetSecondStatus(secondStatus);
	}
}
