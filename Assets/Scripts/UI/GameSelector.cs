using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace CoachSimulator
{
    public class GameSelector : MonoBehaviour
    {
        public static GameSelector instance = null;

        public SportManager sportManager;
        private int _currentSportID = 0;
        private int _currentGameType = 0;

        public Text sportTitle;
        public Text gameType;
        public Text description;
        public Image sportImage;

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
            // Display 0th element:
            UpdateUI();
            Hide();
        }

        #region Button events
        public void OnClickNextSport()
        {
            AudioManager.PlayPloup();
            if (_currentSportID == sportManager.sports.Length - 1)
            {
                _currentSportID = 0;
            }
            else
            {
                _currentSportID++;
            }
            _currentGameType = 0;
            UpdateUI();
        }
        public void OnClickPreviousSport()
        {
            AudioManager.PlayPloup();
            if (_currentSportID == 0)
            {
                _currentSportID = sportManager.sports.Length - 1;
            }
            else
            {
                _currentSportID--;
            }
            _currentGameType = 0;
            UpdateUI();
        }

        public void OnClickNextGameType()
        {
            AudioManager.PlayPloup();
            if (sportManager.sports[_currentSportID].rules.Length == 0)
            {
                return;
            }
            if (_currentGameType == sportManager.sports[_currentSportID].rules.Length - 1)
            {
                _currentGameType = 0;
            }
            else
            {
                _currentGameType++;
            }
            UpdateUI();
        }
        #endregion

        #region Menu navigation events
        public void Back()
        {
            AudioManager.PlayPloup();
            Hide();
            MainMenu.instance.Show();
        }

        public void Next()
        {
            if (!sportManager.sports[_currentSportID].rules[_currentGameType].unlocked)
            {
                return;
            }
            AudioManager.PlayPloup();
            sportManager.selectedSport = sportManager.sports[_currentSportID];
            SportManager.staticSelectedSport = sportManager.sports[_currentSportID];
            sportManager.selectedSport.selectedRule = sportManager.sports[_currentSportID].rules[_currentGameType];
            Hide();
            PlayerSelector.instance.Show();
        }
        #endregion

        public void UpdateUI()
        {
            sportTitle.text = sportManager.sports[_currentSportID].sportName;
            gameType.text = sportManager.sports[_currentSportID].rules.Length > 0 ? sportManager.sports[_currentSportID].rules[_currentGameType].title : "Pas de règle";
            description.text = string.Format("<b>Description:</b>\r\n{0}\r\n<b>Règles:</b>\r\n{1}",
                sportManager.sports[_currentSportID].description,
                sportManager.sports[_currentSportID].rules.Length > 0 ? sportManager.sports[_currentSportID].rules[_currentGameType].description : "Aucune règle");
            sportImage.sprite = sportManager.sports[_currentSportID].sprite;
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