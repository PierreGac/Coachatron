using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CoachSimulator
{
    public class CurlingSingleMatch : GameType
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

        public SocksGame socksGame;

        public Transform broom;
        public Transform[] balls;
        private Vector3 _targetPosition;
        public float minBallSpeed = 2f, maxBallSpeed = 10f;
        public Transform player;
        public Transform otherPlayer;
        public Vector3 dSideStart, dSideEnd;
        public Vector3 uSideStart, uSideEnd;
        private Vector3 _dDir, _uDir;
        private float _dMagnitude = -1f, _uMagnitude = -1f;

        public AudioClip[] ballSounds;
        public Text score;

        private Vector3 _targetBroomPos;

        private int _myScore, _theirScore;
        private bool _waitForBall = false;
        private bool _spawnBall;
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

        private int _maxScore = 3;

        private ResultData _data;

        private Vector3 _ballSpawn = new Vector3(0, -15,0);

        public override void StartGame()
        {
            base.StartGame();
            socksGame.OnEndGame = OnEndMiniGame;
            _matchStartTime = Time.time;

            _myScore = 0;
            _theirScore = 0;

            score.text = "<b>Score: 0/0 </b>";

            _dDir = dSideEnd - dSideStart;
            _dMagnitude = _dDir.magnitude;
            _dDir.Normalize();

            _uDir = uSideEnd - uSideStart;
            _uMagnitude = _uDir.magnitude;
            _uDir.Normalize();

            _shotSpeed = Random.Range(minBallSpeed, maxBallSpeed);
            _nextCoachEvent = Time.time + Random.Range(0, coachEventRandomTime);
            Coach.instance.OnEndChoice = OnEndCoachEvent;

            for(int i = 0; i < balls.Length; i++)
            {
                balls[i].position = _ballSpawn;
            }

            balls[0].position = PickupRandomSpawnPoint();
            broom.SetParent(balls[0]);
            broom.localPosition = Vector3.zero;
            _currentBallID = 0;
            _targetPosition = PickupRandomTargetPosition();

            SportUI.instance.EnableSport("Curling");

            _targetBroomPos = new Vector3(1, 0, 0);

            AudioManager.Play(ballSounds[Random.Range(0, ballSounds.Length)]);

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
            OnEndSport(_data);
        }

        private void OnEndMiniGameAndGame()
        {
            OnEndSport(_data);
        }

        public override void UpdateGame()
        {
            if (_coachEvent || socksGame.active)
            {
                return;
            }
            if(_nextShotEvent != -1)
            {
                if (Time.time >= _nextShotEvent)
                {
                    AudioManager.Play(ballSounds[Random.Range(0, ballSounds.Length)]);
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
                    broom.SetParent(balls[0]);
                    broom.localPosition = Vector3.zero;
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
  

                    score.text = string.Format("<b>Score: {0}/{1}</b>", _myScore, _theirScore);
                    if (_theirScore == _maxScore || _myScore == _maxScore)
                    {
                        // End: Pause the sequence and display end results
                        _data = new ResultData();
                        _data.matchTime = Time.time - _matchStartTime;
                        _data.title = "Résultat du match de curling";
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
                            //SportManager.staticSelectedSport.UnlockRule(typeof(PetanqueChampionship));

                            socksGame.OnEndGame = OnEndMiniGameAndGame;
                            board.SetActive(false);
                            SportManager.staticSelectedSport.isActive = false;
                            socksGame.StartGame();
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
                    _currentBallID++;
                    if(_currentBallID < balls.Length)
                    {
                        broom.SetParent(balls[_currentBallID]);
                        broom.localPosition = Vector3.zero;

                        balls[_currentBallID].position = PickupRandomSpawnPoint();
                    }
                }
            }

            // Coach:
            if (Time.time >= _nextCoachEvent)
            {
                _coachEvent = true;
                Coach.instance.Choice(SportChoices.instance.PickRandomChoice());
            }

            broom.localPosition = Vector3.Lerp(broom.localPosition, _targetBroomPos, Time.deltaTime * 10f);
            if(Vector3.Distance(broom.localPosition, _targetBroomPos) < 0.1f)
            {
                _targetBroomPos = -_targetBroomPos;
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
            return uSideStart + _uDir * Random.Range(0, _uMagnitude);
        }

        private Vector3 PickupRandomSpawnPoint()
        {
            return dSideStart + _dDir * Random.Range(0, _dMagnitude);
        }
    }
}