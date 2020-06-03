using GameFramework;

public class CharacterStatusController : MonoControllerComponentBase
{
	public void OnSelectableFirstStatusCount(int nSelectableFirstStatusCount)
	{
		CharacterStatusModel statusModel = Entity.GetComponent<CharacterStatusModel>();

		statusModel.SelectableFirstStatusCount = nSelectableFirstStatusCount;
	}

	public void OnCharacterStatusChange(FirstStatus firstStatus, SecondStatus secondStatus)
	{
		CharacterStatusModel statusModel = Entity.GetComponent<CharacterStatusModel>();

		statusModel.SetFirstStatus(firstStatus);
		statusModel.SetSecondStatus(secondStatus);
	}
}
