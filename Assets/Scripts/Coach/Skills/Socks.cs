using UnityEngine;

namespace CoachSimulator.Skills
{
    public class Socks : SkillData
    {

        public GameObject socks;

        public override void OnOverlay()
        {
            base.OnOverlay();
        }

        public override void EnableSkill()
        {
            if (isEnoughPoints())
            {
                base.EnableSkill();
                socks.SetActive(true);
            }
        }

        public override void DisableSkill()
        {
            base.DisableSkill();
            socks.SetActive(false);
        }
    }
}