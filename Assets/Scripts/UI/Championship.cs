using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace CoachSimulator
{
    public class Championship : MonoBehaviour
    {
        public delegate void ChampionshipEvent();

        public ChampionshipEvent OnNextStage;

        public static Championship instance = null;

        public SportManager sportManager;

        public GameObject T1, T2, T3, T4, L2, L3, L4;

        public Image[] playerImages;

        private int _currentStage = 0;
        private CanvasGroup _canvas;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                DestroyImmediate(gameObject);
            }
        }

        private void Start()
        {
            _canvas = GetComponent<CanvasGroup>();
            Hide();
        }

        public void Show(bool newtState)
        {
            _canvas.alpha = 1;
            _canvas.blocksRaycasts = true;
            if (newtState)
            {
                for (int i = 0; i < playerImages.Length; i++)
                {
                    playerImages[i].sprite = sportManager.selectedSport.selectedPlayer.playerSprite;
                }
                T2.SetActive(false);
                T3.SetActive(false);
                T4.SetActive(false);
                L2.SetActive(false);
                L3.SetActive(false);
                L4.SetActive(false);
                _currentStage = 0;
            }
            switch (_currentStage)
            {
                case 0:
                    T1.SetActive(true);
                    break;
                case 1:
                    T2.SetActive(true);
                    L2.SetActive(true);
                    break;
                case 2:
                    T3.SetActive(true);
                    L3.SetActive(true);
                    break;
                case 3:
                    T4.SetActive(true);
                    L4.SetActive(true);
                    break;
                default:
                    break;
            }
            _currentStage++;
        }

        public void Hide()
        {
            if(OnNextStage != null)
            {
                OnNextStage();
            }
            _canvas.alpha = 0;
            _canvas.blocksRaycasts = false;
        }
    }
}