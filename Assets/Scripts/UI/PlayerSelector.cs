using UnityEngine;
using UnityEngine.UI;

namespace CoachSimulator
{
    public class PlayerSelector : MonoBehaviour
    {
        public static PlayerSelector instance = null;


        public SportManager sportManager;

        public Text description;
        public Image playerImage;

        private int _currentPlayerID = 0;
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

        #region Button events
        public void OnClickNextPlayer()
        {
            AudioManager.PlayPloup();
            if (_currentPlayerID == sportManager.selectedSport.players.Length - 1)
            {
                _currentPlayerID = 0;
            }
            else
            {
                _currentPlayerID++;
            }
            UpdateUI();
        }
        public void OnClickPreviousPlayer()
        {
            AudioManager.PlayPloup();
            if (_currentPlayerID == 0)
            {
                _currentPlayerID = sportManager.selectedSport.players.Length - 1;
            }
            else
            {
                _currentPlayerID--;
            }
            UpdateUI();
        }

        #endregion

        #region Menu navigation events
        public void Back()
        {
            AudioManager.PlayPloup();
            GameSelector.instance.Show();
            Hide();
        }

        public void Next()
        {
            AudioManager.PlayPloup();
            AudioManager.instance.music.Stop();
            sportManager.selectedSport.selectedPlayer = sportManager.selectedSport.players[_currentPlayerID];
            // Start sport
            sportManager.selectedSport.selectedRule.StartGame();
            sportManager.selectedSport.isActive = true;

            Coach.instance.SetSkillRunningState(true);

            GameManager.instance.gameState = GameManager.GameState.started;

            Hide();
        }
        #endregion

        public void UpdateUI()
        {
            if(sportManager.selectedSport.players.Length == 0)
            {
                return;
            }
            playerImage.sprite = sportManager.selectedSport.players[_currentPlayerID].playerSprite;

            description.text = sportManager.selectedSport.players[_currentPlayerID].GetDescription();
        }

        public void Show()
        {
            _currentPlayerID = 0;
            UpdateUI();
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