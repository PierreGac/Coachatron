using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CoachSimulator.Skills
{
    public class Bathrobe : SkillData
    {

        public GameObject bathrobe;

        public override void OnOverlay()
        {
            base.OnOverlay();
        }

        public override void EnableSkill()
        {
            if (isEnoughPoints())
            {
                base.EnableSkill();
                bathrobe.SetActive(true);
            }
        }

        public override void DisableSkill()
        {
            base.DisableSkill();
            bathrobe.SetActive(false);
        }
    }
}