using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CoachSimulator.Skills
{
    public class SkillData : MonoBehaviour
    {
        public bool active = false;

        public bool isRunning = false;

        public string skillName;
        public string description;

        public string coachText;
        public string coachSelectText;
        public string coachDisableText;

        private Image _background;

        public int cost;

        private void Awake()
        {
            _background = GetComponent<Image>();

            EventTrigger trigger = gameObject.AddComponent<EventTrigger>();

            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerEnter;
            entry.callback.AddListener((data) => { OnOverlay(); });
            trigger.triggers.Add(entry);

            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerExit;
            entry.callback.AddListener((data) => { SkillsUI.instance.OnExitSkill(); });
            trigger.triggers.Add(entry);

            Button button = GetComponent<Button>();

            button.onClick.AddListener(EnableSkill);
        }

        public virtual void RunSkill()
        {
            if (active)
            {
                isRunning = true;
            }
        }

        public virtual void StopSkill()
        {
            isRunning = false;
        }

        protected bool isEnoughPoints()
        {
            return Coach.instance.skillsPoint >= cost;
        }


        public virtual void OnOverlay()
        {
            SkillsUI.instance.ShowSkill(this);
            Coach.Say(coachText);
        }

        public virtual void EnableSkill()
        {
            if(Coach.instance.skillsPoint >= cost)
            {
                Coach.instance.skillsPoint -= cost;
                active = true;

                _background.color = Color.green;


                Coach.instance.charisma++;

                SkillsUI.instance.UpdateUI();

                Button button = GetComponent<Button>();
                button.onClick.RemoveListener(EnableSkill);
                button.onClick.AddListener(DisableSkill);
            }
        }

        public virtual void DisableSkill()
        {
            Coach.instance.skillsPoint += cost;
            active = false;

            _background.color = Color.white;

            Button button = GetComponent<Button>();
            button.onClick.RemoveListener(DisableSkill);
            button.onClick.AddListener(EnableSkill);

            Coach.instance.charisma--;
            SkillsUI.instance.UpdateUI();
        }
    }
}