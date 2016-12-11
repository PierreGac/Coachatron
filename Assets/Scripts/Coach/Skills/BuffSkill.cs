using UnityEngine;
using UnityEngine.UI;

namespace CoachSimulator.Skills
{
    public class BuffSkill : SkillData
    {

        public GameObject button;
        public float coolDownMin = 10f;
        public float coolDownMax = 20f;

        public AudioClip[] clips;

        public int xpMin = 20, xpMax = 50;
        public int scoreMin = 20, scoreMax = 50;

        public float _nextDrinkAvailable = -1f;

        private void Start()
        {
            Button btn = button.GetComponent<Button>();
            btn.onClick.AddListener(OnAction);
        }

        public override void OnOverlay()
        {
            base.OnOverlay();
        }

        public override void RunSkill()
        {
            base.RunSkill();
            if (active)
            {
                button.SetActive(true);
                _nextDrinkAvailable = Time.time + Random.Range(10f, 20f);
            }
        }

        public override void StopSkill()
        {
            base.StopSkill();

            button.SetActive(false);
        }

        public void Update()
        {
            if (isRunning && active)
            {
                Debug.Log("Update");
                if (Time.time >= _nextDrinkAvailable)
                {
                    button.SetActive(true);
                }
            }
        }

        public void OnAction()
        {
            Coach.instance.experience += Random.Range(xpMin, xpMax);
            Coach.instance.score += Random.Range(scoreMin, scoreMax);

            _nextDrinkAvailable = Time.time + Random.Range(coolDownMin, coolDownMax);
            if (clips.Length > 0)
            {
                AudioManager.Play(clips[Random.Range(0, clips.Length)]);
            }

            button.SetActive(false);
        }
    }
}