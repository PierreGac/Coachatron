using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace CoachSimulator
{
    public class Options : MonoBehaviour
    {
        public static Options instance = null;
        public Dropdown qualityDropdown;

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

        public void OnClickApply()
        {
            QualitySettings.SetQualityLevel(qualityDropdown.value);

            Hide();
            MainMenu.instance.Show();
        }

        public void OnClickCancel()
        {
            Hide();
            MainMenu.instance.Show();
        }

        public void Show()
        {
            _canvas.alpha = 1;
            _canvas.blocksRaycasts = true;
            qualityDropdown.value = QualitySettings.GetQualityLevel();
        }

        public void Hide()
        {
            _canvas.alpha = 0;
            _canvas.blocksRaycasts = false;
        }
    }
}