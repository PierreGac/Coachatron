using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace CoachSimulator
{
    public class PastisGame : MonoBehaviour
    {
        public delegate void PastisEvent();


        public PastisEvent OnEndGame;
        private const float FACTOR = 0.1f;

        public bool active = false;

        public GameObject board;
        public Transform levelObject;

        public GameObject ui;
        public Button button;
        public Text key;

        public AudioClip[] clips;
        public AudioClip ambientSound;

        private float _nextClipEvent;

        public float timeMin = 1f, timeMax = 2f;

        private float _inputEndEvent, _keyStartTime, _estimatedDuration;

        private List<KeyCode> qteInputs;

        private KeyCode _activeKey;

        // Use this for initialization
        void Start()
        {
            qteInputs = new List<KeyCode>();
            // Setup inputs:
            for (int i = 97; i < 123; i++)
            {
                qteInputs.Add((KeyCode)i);
            }
            Hide();
        }


        // Update is called once per frame
        void Update()
        {
            if (!active)
            {
                return;
            }

            if (Time.time >= _nextClipEvent)
            {
                AudioManager.Play(clips[Random.Range(0, clips.Length)]);

                _nextClipEvent = Time.time + Random.Range(1, 3);
            }

            if (Time.time < _inputEndEvent)
            {
                if (Input.anyKeyDown)
                {
                    if (Input.GetKeyDown(_activeKey))
                    {
                        // Correct!
                        float progress = (Time.time - _keyStartTime) / _estimatedDuration * FACTOR;

                        levelObject.localScale -= Vector3.up * progress;

                        if (levelObject.localScale.y <= 0)
                        {
                            // End
                            active = false;
                            button.gameObject.SetActive(true);
                        }


                        PickupRandomKey();
                    }
                    else
                    {
                        // Miss!
                        if ((levelObject.localScale + Vector3.up * 0.05f).y < 1)
                        {
                            levelObject.localScale += Vector3.up * 0.05f;
                        }
                        else
                        {
                            levelObject.localScale = Vector3.one;
                        }

                        PickupRandomKey();
                    }
                }
            }
            else
            {
                // Miss!
                if ((levelObject.localScale + Vector3.up * 0.05f).y < 1)
                {
                    levelObject.localScale += Vector3.up * 0.05f;
                }
                else
                {
                    levelObject.localScale = Vector3.one;
                }

                PickupRandomKey();
            }
        }

        public void StartGame()
        {
            AudioManager.instance.ambient.clip = ambientSound;
            AudioManager.instance.ambient.Play();
            button.gameObject.SetActive(false);
            active = true;
            levelObject.localScale = Vector3.one;
            PickupRandomKey();
            _nextClipEvent = Time.time + Random.Range(1, 3);
            Show();
        }

        public void EndGame()
        {
            OnEndGame();
            active = false;
            Hide();
        }

        private void PickupRandomKey()
        {
            _keyStartTime = Time.time;

            _inputEndEvent = Time.time + Random.Range(timeMin, timeMax);
            _estimatedDuration = _inputEndEvent - _keyStartTime;
            _activeKey = qteInputs[Random.Range(0, qteInputs.Count)];

            // Update UI
            key.text = _activeKey.ToString();
        }


        public void Show()
        {
            board.SetActive(true);
            ui.SetActive(true);
        }

        public void Hide()
        {
            ui.SetActive(false);
            board.SetActive(false);
        }
    }
}