using UnityEngine;
using System.Collections;

namespace CoachSimulator
{
    public class PauseMenu : MonoBehaviour
    {
        public static PauseMenu instance = null;
        private CanvasGroup _canvas;
        private void Awake()
        {
            if (instance == null)
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
            Hide();
        }

        public void OnClickResume()
        {
            GameManager.instance.gameState = GameManager.GameState.started;
            Hide();
        }

        public void OnClickMainMenu()
        {
            GameManager.instance.gameState = GameManager.GameState.waitForStart;
            Hide();
        }

        public void OnClickQuit()
        {
            Application.Quit();
        }

        public void Show()
        {
            Time.timeScale = 0f;
            _canvas.alpha = 1;
            _canvas.blocksRaycasts = true;
        }

        public void Hide()
        {
            Time.timeScale = 1f;
            _canvas.alpha = 0;
            _canvas.blocksRaycasts = false;
        }
    }
}
