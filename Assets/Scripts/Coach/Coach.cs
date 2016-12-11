using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace CoachSimulator
{
    public class Coach : MonoBehaviour
    {
        public static Coach instance = null;

        public delegate void EndChoice();
        public delegate void OnSay();

        public OnSay SayEvent;
        public EndChoice OnEndChoice;
        public CoachAudio coachAudio;

        public Skills.SkillData[] skills;

        #region Stats
        public float score = 0f;

        public int level = 1;
        public float experience = 0f;
        public float skillsPoint = 0f;

        public int charisma = 12;

        #endregion

        public Text text;
        public Image coachSprite;

        public Text choiceLeft, choiceUp, choiceRight, choiceDown;
        public RectTransform progressTime;
        public RectTransform progressTimeParent;

        public GameObject choicesGameObject;

        public float timeBeforeExit = 3f;

        private bool _waitChoice = false;
        public bool waitChoice
        {
            get
            {
                return _waitChoice;
            }
        }
        private float _endTime, _startTime;
        private float _currentProgress;
        private float _progressBaseWidth;
        private float _exitEvent = -1f;

        private bool _isPaused = false;

        private CoachChoice _activeChoice;
        private CoachChoiceArray _globalChoices;
        private CoachChoiceArray _resultChoices;

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
            _progressBaseWidth = progressTimeParent.sizeDelta.x;

            _globalChoices = new CoachChoiceArray();
            _resultChoices = new CoachChoiceArray();

            string path = string.Format("{0}/Choices/global.json", Application.dataPath);
            if (File.Exists(path))
            {
                _globalChoices.Unserialize(path);
            }

            path = string.Format("{0}/Choices/results.json", Application.dataPath);
            if (File.Exists(path))
            {
                _resultChoices.Unserialize(path);
            }

            experience = 0f;
            score = 0f;

            SetSkillRunningState(false);

            Hide();
        }

        private void Update()
        {
            if(GameManager.instance.gameState == GameManager.GameState.pauseMenu)
            { 
                _isPaused = true;
                return;
            }
            else
            {
                if(_isPaused)
                {
                    _isPaused = false;
                }
            }

            if (_waitChoice)
            {
                if (Mathf.Approximately(_exitEvent, -1f))
                {
                    _currentProgress = (_endTime - Time.time) / (_endTime - _startTime);
                    progressTime.sizeDelta = new Vector2(_progressBaseWidth * _currentProgress, progressTime.sizeDelta.y);

                    if (_currentProgress <= 0)
                    {
                        // Time over !
                        DisplaySayText(_globalChoices[Random.Range(0, _globalChoices.Length)]);
                    }
                }
                else
                {
                    if(Time.time >= _exitEvent)
                    {
                        _exitEvent = -1f;
                        _waitChoice = false;
                        _activeChoice = null;

                        if (OnEndChoice != null)
                        {
                            OnEndChoice();
                        }
                        Hide();
                    }
                }
            }
        }

        public void ForceHide()
        {
            _exitEvent = -1f;
            _waitChoice = false;
            _activeChoice = null;
            Hide();
        }

        public CoachChoiceEntry GetResultChoice()
        {
            return _resultChoices[Random.Range(0, _resultChoices.Length)];
        }

        public void OnClickLeft()
        {
            Debug.Log("Click left");
            DisplaySayText(_activeChoice.left);
        }

        public void OnClickUp()
        {
            Debug.Log("Click up");
            DisplaySayText(_activeChoice.up);
        }

        public void OnClickRight()
        {
            Debug.Log("Click right");
            DisplaySayText(_activeChoice.right);
        }

        public void OnClickDown()
        {
            Debug.Log("Click down");
            DisplaySayText(_activeChoice.down);
        }

        public void DisplaySayText(CoachChoiceEntry entry)
        {
            choicesGameObject.SetActive(false);
            text.text = entry.sayText;

            experience += Random.Range(entry.expMin, entry.expMax);
            score += Random.Range(entry.pointsMin, entry.pointsMax);

            _exitEvent = Time.time + timeBeforeExit;

            if (SayEvent != null)
            {
                SayEvent();
            }

            AudioManager.PlayCoach(coachAudio.GetRandomReactionClip());
        }

        public void SetSkillRunningState(bool state)
        {
            for(int i = 0; i < skills.Length; i++)
            {
                if (state)
                {
                    skills[i].RunSkill();
                }
                else
                {
                    skills[i].StopSkill();
                }
            }
        }

        public void Choice(CoachChoice choice)
        {
            _activeChoice = choice;
            Show();
            text.text = "Que dire?";
            choicesGameObject.SetActive(true);
            choiceLeft.text = choice.left.choiceText;
            choiceUp.text = choice.up.choiceText;
            choiceRight.text = choice.right.choiceText;
            choiceDown.text = choice.down.choiceText;
            _currentProgress = 1f;
            _waitChoice = true;
            _endTime = Time.time + choice.GetAverageTime();
            _startTime = Time.time;
            progressTime.sizeDelta = new Vector2(_progressBaseWidth, progressTime.sizeDelta.y);
        }

        public static void Say(string text)
        {
            instance.text.text = text;
            if(instance.SayEvent != null)
            {
                instance.SayEvent();
            }
        }

        public void Show()
        {
            _canvas.alpha = 1f;
            _canvas.blocksRaycasts = true;
        }


        public void Hide()
        {
            _canvas.alpha = 0f;
            _canvas.blocksRaycasts = false;
        }
    }
}