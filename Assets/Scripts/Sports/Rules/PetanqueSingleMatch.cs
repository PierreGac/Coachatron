using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CoachSimulator
{
    public class PetanqueSingleMatch : GameType
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

        public PastisGame pastisGame;

        public Transform[] balls;
        public float minBallSpeed = 2f, maxBallSpeed = 10f;
        public Transform player;
        public Transform otherPlayer;
        public Vector3 p1, p2;

        public AudioClip[] ballSounds;
        public Text score;

        private int _myScore, _theirScore;
        private bool _waitForBall = false;
        private Vector3 _targetPosition;
        private float _shotSpeed;

        private float _matchStartTime;

        //Coach stuff:
        public float minTimeBetweenCoachEvent = 3f;
        public float coachEventRandomTime = 5f;
        private float _nextCoachEvent = -1f;
        private bool _coachEvent = false;
        private int _currentBallID = 0;

        public float timeBetweenShots = 1.5f;
        private float _nextShotEvent = -1f;

        private int _maxScore = 5;

        private ResultData _data;

        private Vector3 _ballSpawn = new Vector3(-4.61f, -3.8f, 1.4f);

        public override void StartGame()
        {
            base.StartGame();
            pastisGame.OnEndGame = OnEndMiniGame;
            _matchStartTime = Time.time;

            _myScore = 0;
            _theirScore = 0;

            score.text = "<b>Score: 0/0 </b>";

            _targetPosition = PickupRandomTargetPosition();
            _shotSpeed = Random.Range(minBallSpeed, maxBallSpeed);
            _nextCoachEvent = Time.time + Random.Range(0, coachEventRandomTime);
            Coach.instance.OnEndChoice = OnEndCoachEvent;

            SportUI.instance.EnableSport("Petanque");

            for (int i = 0; i < balls.Length; i++)
            {
                balls[i].position = _ballSpawn;
            }
            _currentBallID = 0;
            _waitForBall = true;
            _nextShotEvent = Time.time + timeBetweenShots;
        }

        private void OnEndCoachEvent()
        {
            _coachEvent = false;
            _nextCoachEvent = Time.time + minTimeBetweenCoachEvent + Random.Range(0, coachEventRandomTime);
        }

        private void OnEndMiniGame()
        {
            SportManager.staticSelectedSport.isActive = true;
            board.SetActive(true);
        }

        private void OnEndMiniGameAndGame()
        {
            OnEndSport(_data);
        }

        public override void UpdateGame()
        {
            if (_coachEvent || pastisGame.active)
            {
                return;
            }
            if(_nextShotEvent != -1)
            {
                if (Time.time >= _nextShotEvent)
                {
                    _nextShotEvent = -1f;
                }
                return;
            }
            if (!_waitForBall)
            {
                if(_nextShotEvent != -1)
                {
                    return;
                }
                if (_currentBallID >= balls.Length)
                {
                    
                    // End of round
                    _currentBallID = 0;

                    // Me or them ?
                    if (Random.Range(0, 2) == 0)
                    {
                        _theirScore++;
                    }
                    else
                    {
                        _myScore++;
                    }

                    if(_theirScore == _myScore && _myScore != _maxScore)
                    {
                        // Play "rencontre" + rnd mini game 

                        if (Random.Range(0, 5) == 1)
                        {
                            board.SetActive(false);
                            SportManager.staticSelectedSport.isActive = false;
                            pastisGame.StartGame();
                        }
                    }
  

                    score.text = string.Format("<b>Score: {0}/{1}</b>", _myScore, _theirScore);
                    if (_theirScore == _maxScore || _myScore == _maxScore)
                    {
                        // End: Pause the sequence and display end results
                        _data = new ResultData();
                        _data.matchTime = Time.time - _matchStartTime;
                        _data.title = "Résultat du matche de petanque";
                        _data.results = new Dictionary<string, int>();
                        _data.results.Add("moi", _myScore);
                        _data.results.Add("eux", _theirScore);

                        _data.teams = new Dictionary<string, List<string>>();
                        _data.teams.Add("moi", new List<string>());
                        _data.teams["moi"].Add(SportManager.staticSelectedSport.selectedPlayer.name);
                        _data.teams.Add("eux", new List<string>());
                        _data.teams["eux"].Add(PetanquePlayer.GetRandomPlayerName());

                        if (_myScore > _theirScore)
                        {
                            Coach.instance.score += 500;
                            Coach.instance.experience += 500;
                            SportManager.staticSelectedSport.UnlockRule(typeof(PetanqueChampionship));

                            pastisGame.OnEndGame = OnEndMiniGameAndGame;
                            board.SetActive(false);
                            SportManager.staticSelectedSport.isActive = false;
                            pastisGame.StartGame();
                            return;
                        }

                        OnEndSport(_data);
                        return;
                    }
                    else
                    {
                        ResetBalls();
                    }
                }
                // Pickup a new position
                _targetPosition = PickupRandomTargetPosition();
                _shotSpeed = Random.Range(minBallSpeed, maxBallSpeed);
                _waitForBall = true;
            }
            else
            {
                balls[_currentBallID].position = Vector3.Lerp(balls[_currentBallID].position, _targetPosition, Time.deltaTime * _shotSpeed);

                if (Vector3.Distance(balls[_currentBallID].position, _targetPosition) < 0.1f)
                {
                    _waitForBall = false;
                    _nextShotEvent = Time.time + timeBetweenShots;
                    //Play ball sound:
                    AudioManager.Play(ballSounds[Random.Range(0, ballSounds.Length)]);
                    _currentBallID++;
                }
            }

            // Coach:
            if (Time.time >= _nextCoachEvent)
            {
                _coachEvent = true;
                Coach.instance.Choice(SportChoices.instance.PickRandomChoice());
            }

        }

        private void ResetBalls()
        {
            for (int i = 0; i < balls.Length; i++)
            {
                balls[i].position = _ballSpawn;
            }
        }

        private Vector3 PickupRandomTargetPosition()
        {
            return new Vector3(Random.Range(p1.x, p2.x), Random.Range(p1.y, p2.y), Random.Range(p1.z, p2.z));
        }
    }
}