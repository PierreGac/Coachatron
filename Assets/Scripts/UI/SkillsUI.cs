using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace CoachSimulator
{
    public class SkillsUI : MonoBehaviour
    {
        public static SkillsUI instance = null;

        public CanvasGroup canvas;

        public Coach coach;

        public Text scoreText;
        public Text skillsPointsText;

        public Text skillTitle;
        public Text skillDescription;

        public Text xpText;
        public RectTransform xpImage;
        public RectTransform xpImageParent;

        public Text stateText;

        private float _progressBaseWidth;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
        }

        private void Start()
        {

            _progressBaseWidth = xpImageParent.sizeDelta.x;

            Hide();
        }


        public void OnExitSkill()
        {
            skillTitle.text = "";
            skillDescription.text = "";
            Coach.Say("");  
        }

        public void ShowSkill(Skills.SkillData data)
        {
            skillTitle.text = string.Format("<b>{0} ({1}pts)</b>", data.skillName, data.cost);
            skillDescription.text = data.description;
        }

        public void OnClickNext()
        {
            GameSelector.instance.Show();
            AudioManager.instance.music.Play();
            Hide();
            Coach.instance.Hide();
        }

        public void Show()
        {
            coach.Show();
            coach.choicesGameObject.SetActive(false);
            canvas.alpha = 1f;
            canvas.blocksRaycasts = true;
            // Update data:

            // Calculate levels:
            int levels = (int)coach.experience / 200;
            coach.level += levels;
            if(levels != 0)
            {
                MessageBox.instance.Show("LEVEL UP!\r\nTu deviens plus sexy et sportif, génial!");
                Trumpets.instance.Show();
            }
            coach.experience = coach.experience - (levels > 0 ? levels * 200 : 0);

            coach.skillsPoint += (levels * 2);
            skillsPointsText.text = string.Format("<b>Points disponible: {0}</b>", coach.skillsPoint);

            scoreText.text = string.Format("<b>Score: {0}</b>", coach.score);

            // Update XP bar
            float currentProgress = coach.experience / 200f;
            xpImage.sizeDelta = new Vector2(_progressBaseWidth * currentProgress, xpImage.sizeDelta.y);

            xpText.text = string.Format("{0}/200", (int)coach.experience);

            stateText.text = string.Format("Niveau: {0}\r\nPointure: {1}\r\nCharisme: {2}\r\nDernière douche: Hier", coach.level, Random.Range(35, 42), coach.charisma);
        }

        public void UpdateUI()
        {
            skillsPointsText.text = string.Format("<b>Points disponible: {0}</b>", coach.skillsPoint);

            stateText.text = string.Format("Niveau: {0}\r\nPointure: {1}\r\nCharisme: {2}\r\nDernière douche: Hier", coach.level, Random.Range(35, 42), coach.charisma);
        }

        public void Hide()
        {
            canvas.alpha = 0;
            canvas.blocksRaycasts = false;
        }
    }
}