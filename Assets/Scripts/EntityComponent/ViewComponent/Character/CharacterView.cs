using Entity;

public class CharacterView : EntityBasicView
{
	private PlayerHealthBar m_HealthBar = null;

	#region MonoBehaviour
	private void LateUpdate()
	{
		Character character = Entity as Character;
		if (character != null)
		{
			Animator_SetFloat("Speed", character.Velocity.magnitude);
			Animator_SetBool("Alive", character.IsAlive);
		}
	}
	#endregion

	public void CreateHealthBar()
	{
		if (m_HealthBar != null)
		{
			m_HealthBar.Clear();
			if (ResourcePool.HasInstance())
			{
				ResourcePool.Instance.ReturnResource(m_HealthBar.gameObject);
			}
			m_HealthBar = null;
		}

		m_HealthBar = HealthBarFactory.Instance.CreateHealthBar(HealthBarType.Player) as PlayerHealthBar;
		m_HealthBar.SetTarget(Entity as Character);
	}

	public void ClearHealthBar()
	{
		if (m_HealthBar != null)
		{
			m_HealthBar.Clear();
			if (ResourcePool.HasInstance())
			{
				ResourcePool.Instance.ReturnResource(m_HealthBar.gameObject);
			}
			m_HealthBar = null;
		}
	}
}
