using UnityEngine;
using System.Collections;

namespace CoachSimulator
{
    public class Trumpets : MonoBehaviour
    {
        public static Trumpets instance = null;

        public bool active;
        public AudioClip[] clips;

        public Transform[] t1, t2;

        private CanvasGroup _canvas;

        private float _nextSwitchEvent = -1f;
        private float _interval = 0.5f;

        private float _lifeTime = 8f;
        private float _hideEvent;
        private bool _state = false;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            _canvas = GetComponent<CanvasGroup>();
            Hide();
        }


        private void Update()
        {
            if (!active)
            {
                return;
            }

            if (Time.time >= _nextSwitchEvent)
            {
                if (_state)
                {
                    for (int i = 0; i < t1.Length; i++)
                    {
                        t1[i].localEulerAngles = Vector3.zero;
                    }
                    for (int i = 0; i < t2.Length; i++)
                    {
                        t2[i].localEulerAngles = Vector3.up * 180f;
                    }
                }
                else
                {
                    for (int i = 0; i < t1.Length; i++)
                    {
                        t1[i].localEulerAngles = Vector3.forward * 65f;
                    }
                    for (int i = 0; i < t2.Length; i++)
                    {
                        t2[i].localEulerAngles = new Vector3(0, 180, 65);
                    }
                }

                _state = !_state;
                _nextSwitchEvent = Time.time + _interval;
            }

            if(Time.time >= _hideEvent)
            {
                Hide();
            }
        }

        public void Show()
        {
            _canvas.alpha = 1;
            active = true;
            _nextSwitchEvent = Time.time + _interval;
            _hideEvent = Time.time + _lifeTime;
            _state = false;
            AudioManager.Play(clips[Random.Range(0, clips.Length)]);
        }

        public void Hide()
        {
            _canvas.alpha = 0;
            active = false;
        }
    }
}