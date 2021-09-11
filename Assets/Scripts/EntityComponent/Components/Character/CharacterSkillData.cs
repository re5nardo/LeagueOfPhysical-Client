﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entity;
using GameFramework;

public class CharacterSkillData : MonoEntityComponentBase
{
    private int basicAttackSkillID;
    private int activeSkill1ID;
    private int activeSkill2ID;
    private int ultimateSkillID;

    public override void Initialize(EntityCreationData entityCreationData)
    {
        base.Initialize(entityCreationData);

        foreach (int skillID in (Entity as Character).MasterData.SkillIDs)
        {
            MasterData.Skill masterSkill = MasterDataManager.Instance.GetMasterData<MasterData.Skill>(skillID);

            switch (masterSkill.SkillType)
            {
                case "BasicAttackSkill":
                    basicAttackSkillID = skillID;
                    break;

                case "ActiveSkill1":
                    activeSkill1ID = skillID;
                    break;

                case "ActiveSkill2":
                    activeSkill2ID = skillID;
                    break;

                case "UltimateSkill":
                    ultimateSkillID = skillID;
                    break;
            }
        }
    }

    public int BasicAttackSkillID { get { return basicAttackSkillID; } }
}
