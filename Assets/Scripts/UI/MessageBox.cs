using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace CoachSimulator
{
    public class MessageBox : MonoBehaviour
    {
        public delegate void MessageBoxEvent();

        public MessageBoxEvent OnClose;

        public static MessageBox instance = null;
        private CanvasGroup _canvas;

        public Text text;

        private void Awake()
        {
            instance = this;
        }
        // Use this for initialization
        void Start()
        {
            _canvas = GetComponent<CanvasGroup>();
            Hide();
        }

        public void Show(string text)
        {
            //Time.timeScale = 0;
            _canvas.alpha = 1f;
            _canvas.blocksRaycasts = true;
            this.text.text = text;
        }


        public void Hide()
        {
            if(OnClose != null)
            {
                OnClose();
                OnClose = null;
            }
            //Time.timeScale = 1;
            _canvas.alpha = 0f;
            _canvas.blocksRaycasts = false;
        }
    }
}