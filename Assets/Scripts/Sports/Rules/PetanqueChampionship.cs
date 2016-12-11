using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CoachSimulator
{
    public class PetanqueChampionship : GameType
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

        public Championship championshipUI;
        private int _currentTier = 0;
        private bool _championshipScreen = false;

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

        private Vector3 _ballSpawn = new Vector3(-4.61f, -3.8f, 1.4f);

        private ResultData _data;

        private int _maxScore = 5;
        private bool _skipMiniGame = false;

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

                Trumpets.instance.Show();

                MessageBox.instance.Show("On gagné le tournoi de pétanque du camping, c'est ti pas impeccable ça?");
                MessageBox.instance.OnClose = OnExitAfterWin;
            }
        }

        private void OnExitAfterWin()
        {
            pastisGame.OnEndGame = OnEndMiniGameAndGame;
            board.SetActive(false);
            SportManager.staticSelectedSport.isActive = false;
            pastisGame.StartGame();
            //OnEndSport(_data);
        }

        private void OnEndMiniGameAndGame()
        {
            OnEndSport(_data);
        }

        public override void StartGame()
        {
            base.StartGame();
            _skipMiniGame = false;
            pastisGame.OnEndGame = OnEndMiniGame;
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
            _skipMiniGame = true;
        }

        public override void UpdateGame()
        {
            if (_coachEvent || _championshipScreen || pastisGame.active)
            {
                return;
            }
            if (_nextShotEvent != -1)
            {
                if (Time.time >= _nextShotEvent)
                {
                    _nextShotEvent = -1f;
                }
                return;
            }
            if (!_waitForBall)
            {
                if (_nextShotEvent != -1)
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

                    if (_theirScore == _myScore && _myScore != _maxScore)
                    {
                        // Play "rencontre" + rnd mini game 
                        if (Random.Range(0, 5) == 1)
                        {
                            board.SetActive(false);
                            SportManager.staticSelectedSport.isActive = false;
                            pastisGame.StartGame();
                        }
                    }

                    _skipMiniGame = false;

                    score.text = string.Format("<b>Score: {0}/{1}</b>", _myScore, _theirScore);
                    if (_theirScore == 11 || _myScore == 11)
                    {
                        if (_myScore > _theirScore)
                        {
                            championshipUI.Show(false);
                            _currentTier++;
                            _championshipScreen = true;
                            Coach.instance.ForceHide();
                        }
                        else
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


                            }
                            OnEndSport(_data);
                        }
                        return;
                    }
                    else
                    {
                        for (int i = 0; i < balls.Length; i++)
                        {
                            balls[i].position = _ballSpawn;
                        }
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

        private Vector3 PickupRandomTargetPosition()
        {
            return new Vector3(Random.Range(p1.x, p2.x), Random.Range(p1.y, p2.y), Random.Range(p1.z, p2.z));
        }
    }
}