using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CoachSimulator.Skills
{
    public class Casquette : SkillData
    {

        public GameObject casquette;

        public override void OnOverlay()
        {
            base.OnOverlay();
        }

        public override void EnableSkill()
        {
            if (isEnoughPoints())
            {
                base.EnableSkill();
                casquette.SetActive(true);
            }
        }

        public override void DisableSkill()
        {
            base.DisableSkill();
            casquette.SetActive(false);
        }
    }
}