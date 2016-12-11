using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CoachSimulator
{
    public class PingPongChampionship : GameType
    {
        [SerializeField]
        private string _title;
        public override string title
        {
            get
            {
                return _title;
            }
        }

        [SerializeField]
        private string _description;
        public override string description
        {
            get
            {
                if (unlocked)
                {
                    return _description;
                }
                else
                {
                    return _disabledDescription;
                }
            }
        }

        [SerializeField]
        private string _disabledDescription;
        public override string disabledDescription
        {
            get
            {
                return _description;
            }
        }

        public int maxScore = 11;

        public Championship championshipUI;
        private int _currentTier = 0;
        private bool _championshipScreen = false;

        public Transform ball;
        public float minBallSpeed = 2f, maxBallSpeed = 10f;
        public Transform player;
        public Transform otherPlayer;
        public Vector3 dSideStart, dSideEnd;
        public Vector3 uSideStart, uSideEnd;
        private Vector3 _dDir, _uDir;
        private float _dMagnitude = -1f, _uMagnitude = -1f;

        public Text score;
        public AudioClip[] ballSounds;

        private int _myScore, _theirScore;
        private bool _waitForBall = false;
        private Vector3 _targetPosition;
        private bool _isDownSide = true;
        private float _shotSpeed;

        private float _matchStartTime;

        //Coach stuff:
        public float minTimeBetweenCoachEvent = 3f;
        public float coachEventRandomTime = 5f;
        private float _nextCoachEvent = -1f;
        private bool _coachEvent = false;

        private ResultData _data;

        public void OnCloseChampionship()
        {
            _championshipScreen = false;
            if (_currentTier < 4)
            {
                StartGame();
            }
            else
            {
                // WIN !
                // End: Pause the sequence and display end results
                _data = new ResultData();
                _data.matchTime = Time.time - _matchStartTime;
                _data.title = "Tournoi de ping pong gagné !";
                _data.results = new Dictionary<string, int>();
                _data.results.Add("moi", _myScore);
                _data.results.Add("eux", _theirScore);
                
                _data.teams = new Dictionary<string, List<string>>();
                _data.teams.Add("moi", new List<string>());
                _data.teams["moi"].Add(SportManager.staticSelectedSport.selectedPlayer.name);
                _data.teams.Add("eux", new List<string>());
                _data.teams["eux"].Add(PingPongPlayer.GetRandomPlayerName());


                Coach.instance.score += 2000;
                Coach.instance.experience += 2000;

                MessageBox.instance.Show("Tu as gagné le tournoi de pongping !!! C'est génial, direction la buvette!");
                MessageBox.instance.OnClose = OnExitAfterWin;
            }
        }


        public override void StartGame()
        {
            base.StartGame();
            championshipUI.OnNextStage = OnCloseChampionship;
            if (_currentTier == 0)
            {
                _currentTier++;
                _championshipScreen = true;
                championshipUI.Show(true);
                return;
            }
            _matchStartTime = Time.time;

            _currentTier = 0;
            _myScore = 0;
            _theirScore = 0;

            _dDir = dSideEnd - dSideStart;
            _dMagnitude = _dDir.magnitude;
            _dDir.Normalize();

            _uDir = uSideEnd - uSideStart;
            _uMagnitude = _uDir.magnitude;
            _uDir.Normalize();

            score.text = "<b>Score: 0/0 </b>";

            _targetPosition = PickupRandomTargetPosition(!_isDownSide);
            _isDownSide = !_isDownSide;
            _shotSpeed = Random.Range(minBallSpeed, maxBallSpeed);
            _nextCoachEvent = Time.time + Random.Range(0, coachEventRandomTime);
            Coach.instance.OnEndChoice = OnEndCoachEvent;

            SportUI.instance.EnableSport("PingPong");
            _waitForBall = true;
            AudioManager.Play(ballSounds[Random.Range(0, ballSounds.Length)]);
        }

        private void OnEndCoachEvent()
        {
            _coachEvent = false;
            _nextCoachEvent = Time.time + minTimeBetweenCoachEvent + Random.Range(0, coachEventRandomTime);
        }

        private void OnExitAfterWin()
        {
            OnEndSport(_data);
        }

        public override void UpdateGame()
        {
            if (_coachEvent || _championshipScreen)
            {
                return;
            }
            if (!_waitForBall)
            {
                // Pickup a new position
                _targetPosition = PickupRandomTargetPosition(!_isDownSide);
                AudioManager.Play(ballSounds[Random.Range(0, ballSounds.Length)]);
                _isDownSide = !_isDownSide;
                _shotSpeed = Random.Range(minBallSpeed, maxBallSpeed);
                _waitForBall = true;

                // End of trajectory, random end of shot
                if (Random.Range(0, 5) == 1)
                {
                    // Me or them ?
                    if (Random.Range(0, 2) == 0)
                    {
                        _theirScore++;
                    }
                    else
                    {
                        _myScore++;
                    }
                    score.text = string.Format("<b>Score: {0}/{1}</b>", _myScore, _theirScore);

                    if (_theirScore == maxScore || _myScore == maxScore)
                    {
                        if (_myScore > _theirScore)
                        {
                            championshipUI.Show(false);
                            _currentTier++;
                            _championshipScreen = true;

                            Coach.instance.score += 500;
                            Coach.instance.experience += 500;

                            Trumpets.instance.Show();

                            Coach.instance.ForceHide();
                            return;
                        }
                        else
                        {
                            // End: Pause the sequence and display end results
                            _data = new ResultData();
                            _data.matchTime = Time.time - _matchStartTime;
                            _data.title = "Echec du tournoi de pingpong...";
                            _data.results = new Dictionary<string, int>();
                            _data.results.Add("moi", _myScore);
                            _data.results.Add("eux", _theirScore);
                            
                            _data.teams = new Dictionary<string, List<string>>();
                            _data.teams.Add("moi", new List<string>());
                            _data.teams["moi"].Add(SportManager.staticSelectedSport.selectedPlayer.name);
                            _data.teams.Add("eux", new List<string>());
                            _data.teams["eux"].Add(PingPongPlayer.GetRandomPlayerName());

                            OnEndSport(_data);
                        }
                    }
                }
            }
            else
            {
                ball.position = Vector3.Lerp(ball.position, _targetPosition, Time.deltaTime * _shotSpeed);

                if (Vector3.Distance(ball.position, _targetPosition) < 0.1f)
                {
                    _waitForBall = false;
                }
            }

            // Coach:
            if (Time.time >= _nextCoachEvent)
            {
                _coachEvent = true;
                Coach.instance.Choice(SportChoices.instance.PickRandomChoice());
            }

        }

        private Vector3 PickupRandomTargetPosition(bool isDown)
        {
            if (isDown)
            {
                return dSideStart + _dDir * Random.Range(0, _dMagnitude);
            }
            else
            {
                return uSideStart + _uDir * Random.Range(0, _uMagnitude);
            }
        }
    }
}