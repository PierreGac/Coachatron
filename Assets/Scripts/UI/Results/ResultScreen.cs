using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace CoachSimulator
{
    public class ResultScreen : MonoBehaviour
    {
        protected CanvasGroup _canvas;

        public Text title;
        public Text time;
        public Text coachComment;
        public Text teams;
        public Text result;


        private void Awake()
        {
            _canvas = GetComponent<CanvasGroup>();
        }

        public virtual void DisplayResultMenu(ResultData data)
        {
            Show();

            title.text = data.title;
            time.text = string.Format("Temps du match: {0}s", data.matchTime);

            teams.text = data.GetTeamsDescription();
            result.text = data.GetTeamsResults();

            coachComment.text = string.Format("Commentaire du coach: <i>{0}</i>", Coach.instance.GetResultChoice().sayText);

            Coach.instance.SetSkillRunningState(false);
        }

        public virtual void OnClickNext()
        {
            AudioManager.PlayPloup();
            SkillsUI.instance.Show();
            Hide();
        }

        public void Show()
        {
            _canvas.alpha = 1;
            _canvas.blocksRaycasts = true;
        }

        public void Hide()
        {
            _canvas.alpha = 0;
            _canvas.blocksRaycasts = false;
        }
    }
}