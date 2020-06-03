using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MasterData;
using System.Linq;
using GameFramework;

public class MasterDataManager : MonoSingleton<MasterDataManager>
{
    private Dictionary<System.Type, Dictionary<int, IMasterData>> m_dicMasterData = new Dictionary<System.Type, Dictionary<int, IMasterData>>();

    protected override void Awake()
    {
        base.Awake();

        Initialize();
    }

    private void Initialize()
    {
        SetCharacter();
        SetBehavior();
        SetSkill();
        SetEmotionExpression();
        SetGameItem();
		SetAbility();
	}

    private void SetCharacter()
    {
        m_dicMasterData.Add(typeof(Character), new Dictionary<int, IMasterData>());

        List<List<string>> csv = Util.ReadCSV("GeneratedCSVFile/Character");

        for (int i = 2; i < csv.Count; ++i)
        {
            Character master = new Character();
            master.SetData(csv[i]);

            m_dicMasterData[typeof(Character)].Add(master.ID, master);
        }
    }

    private void SetBehavior()
    {
        m_dicMasterData.Add(typeof(MasterData.Behavior), new Dictionary<int, IMasterData>());

        List<List<string>> csv = Util.ReadCSV("GeneratedCSVFile/Behavior");

        for (int i = 2; i < csv.Count; ++i)
        {
            MasterData.Behavior master = new MasterData.Behavior();
            master.SetData(csv[i]);

            m_dicMasterData[typeof(MasterData.Behavior)].Add(master.ID, master);
        }
    }

    private void SetSkill()
    {
        m_dicMasterData.Add(typeof(MasterData.Skill), new Dictionary<int, IMasterData>());

        List<List<string>> csv = Util.ReadCSV("GeneratedCSVFile/Skill");

        for (int i = 2; i < csv.Count; ++i)
        {
            MasterData.Skill master = new MasterData.Skill();
            master.SetData(csv[i]);

            m_dicMasterData[typeof(MasterData.Skill)].Add(master.ID, master);
        }
    }

    private void SetEmotionExpression()
    {
        m_dicMasterData.Add(typeof(MasterData.EmotionExpression), new Dictionary<int, IMasterData>());

        List<List<string>> csv = Util.ReadCSV("GeneratedCSVFile/EmotionExpression");

        for (int i = 2; i < csv.Count; ++i)
        {
            MasterData.EmotionExpression master = new MasterData.EmotionExpression();
            master.SetData(csv[i]);

            m_dicMasterData[typeof(MasterData.EmotionExpression)].Add(master.ID, master);
        }
    }

    private void SetGameItem()
    {
        m_dicMasterData.Add(typeof(MasterData.GameItem), new Dictionary<int, IMasterData>());

        List<List<string>> csv = Util.ReadCSV("GeneratedCSVFile/GameItem");

        for (int i = 2; i < csv.Count; ++i)
        {
            MasterData.GameItem master = new MasterData.GameItem();
            master.SetData(csv[i]);

            m_dicMasterData[typeof(MasterData.GameItem)].Add(master.ID, master);
        }
    }

	private void SetAbility()
	{
		m_dicMasterData.Add(typeof(MasterData.Ability), new Dictionary<int, IMasterData>());

		List<List<string>> csv = Util.ReadCSV("GeneratedCSVFile/Ability");

		for (int i = 2; i < csv.Count; ++i)
		{
			MasterData.Ability master = new MasterData.Ability();
			master.SetData(csv[i]);

			m_dicMasterData[typeof(MasterData.Ability)].Add(master.ID, master);
		}
	}

	public T GetMasterData<T>(int nKey) where T : IMasterData
    {
		Dictionary<int, IMasterData> datas = null;

		if (m_dicMasterData.TryGetValue(typeof(T), out datas))
		{
			IMasterData data = default;

			if (datas.TryGetValue(nKey, out data))
			{
				return (T)data;
			}
		}

		return default;
    }

    public List<T> GetAllMasterData<T>() where T : IMasterData
    {
		Dictionary<int, IMasterData> datas = null;

		if (m_dicMasterData.TryGetValue(typeof(T), out datas))
		{
			return datas.Values.Cast<T>().ToList();
		}

		return default;
    }
}
