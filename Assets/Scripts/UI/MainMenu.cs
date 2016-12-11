using UnityEngine;

namespace CoachSimulator
{
    public class MainMenu : MonoBehaviour
    {
        public static MainMenu instance = null;

        public TitleScreen titleScreen;

        private CanvasGroup _canvas;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            _canvas = GetComponent<CanvasGroup>();
            Show();
        }

        #region Button events
        public void Quit()
        {
            Application.Quit();
        }

        public void Play()
        {
            AudioManager.PlayPloup();
            titleScreen.gameObject.SetActive(false);
            Hide();
            GameSelector.instance.Show();
        }

        public void OnClickOptions()
        {
            AudioManager.PlayPloup();
            Options.instance.Show();
            Hide();
        }

        public void OnClickScores()
        {
            AudioManager.PlayPloup();
            Scores.instance.Show();
            Hide();
        }
        #endregion

        public void Show()
        {
            _canvas.alpha = 1;
            _canvas.blocksRaycasts = true;
        }

        public void Hide()
        {
            _canvas.alpha = 0f;
            _canvas.blocksRaycasts = false;
        }
    }
}