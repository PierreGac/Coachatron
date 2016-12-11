using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CoachSimulator
{
    public class PingPongDouble : GameType
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

        public Transform ball01, ball02;
        public float minBallSpeed = 2f, maxBallSpeed = 10f;
        public Transform player01, player02;
        public Transform otherPlayer01, otherPlayer02;
        public Vector3 dSideStart, dSideEnd;
        public Vector3 uSideStart, uSideEnd;
        private Vector3 _dDir, _uDir;
        private float _dMagnitude = -1f, _uMagnitude = -1f;
        public AudioClip[] ballSounds;

        public Text score;

        private int _myScore, _theirScore;
        private bool _waitForBall01 = false, _waitForBall02;
        private Vector3 _targetPosition01, _targetPosition02;
        private bool _isDownSide01 = true, _isDownSide02 = false;
        private float _shotSpeed01, _shotSpeed02;

        private float _matchStartTime;

        //Coach stuff:
        public float minTimeBetweenCoachEvent = 3f;
        public float coachEventRandomTime = 5f;
        private float _nextCoachEvent = -1f;
        private bool _coachEvent = false;



        public override void StartGame()
        {
            base.StartGame();
            _matchStartTime = Time.time;

            _myScore = 0;
            _theirScore = 0;

            _dDir = dSideEnd - dSideStart;
            _dMagnitude = _dDir.magnitude;
            _dDir.Normalize();

            _uDir = uSideEnd - uSideStart;
            _uMagnitude = _uDir.magnitude;
            _uDir.Normalize();

            score.text = "<b>Score: 0/0 </b>";

            _targetPosition01 = PickupRandomTargetPosition(!_isDownSide01);
            _isDownSide01 = !_isDownSide01;
            _shotSpeed01 = Random.Range(minBallSpeed, maxBallSpeed);

            _targetPosition02 = PickupRandomTargetPosition(!_isDownSide02);
            _isDownSide02 = !_isDownSide02;
            _shotSpeed02 = Random.Range(minBallSpeed, maxBallSpeed);

            _nextCoachEvent = Time.time + Random.Range(0, coachEventRandomTime);
            Coach.instance.OnEndChoice = OnEndCoachEvent;

            SportUI.instance.EnableSport("PingPong");
            _waitForBall01 = true;
            _waitForBall02 = true;
            AudioManager.Play(ballSounds[Random.Range(0, ballSounds.Length)]);
        }

        private void OnEndCoachEvent()
        {
            _coachEvent = false;
            _nextCoachEvent = Time.time + minTimeBetweenCoachEvent + Random.Range(0, coachEventRandomTime);
        }

        public override void UpdateGame()
        {
            if (_coachEvent)
            {
                return;
            }

            #region Ball01
            if (!_waitForBall01)
            {
                // Pickup a new position
                _targetPosition01 = PickupRandomTargetPosition(!_isDownSide01);
                _isDownSide01 = !_isDownSide01;
                _shotSpeed01 = Random.Range(minBallSpeed, maxBallSpeed);
                _waitForBall01 = true;
                AudioManager.Play(ballSounds[Random.Range(0, ballSounds.Length)]);

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
                        // End: Pause the sequence and display end results
                        ResultData data = new ResultData();
                        data.matchTime = Time.time - _matchStartTime;
                        data.title = "Résultat du matche de pingpong";
                        data.results = new Dictionary<string, int>();
                        data.results.Add("moi", _myScore);
                        data.results.Add("eux", _theirScore);

                        data.teams = new Dictionary<string, List<string>>();
                        data.teams.Add("moi", new List<string>());
                        data.teams["moi"].Add(SportManager.staticSelectedSport.selectedPlayer.name);
                        data.teams.Add("eux", new List<string>());
                        data.teams["eux"].Add(PingPongPlayer.GetRandomPlayerName());

                        if (_myScore > _theirScore)
                        {
                            Coach.instance.score += 500;
                            Coach.instance.experience += 500;
                            SportManager.staticSelectedSport.UnlockRule(typeof(PingPongChampionship));
                            Trumpets.instance.Show();
                        }

                        OnEndSport(data);
                    }
                }
            }
            else
            {
                ball01.position = Vector3.Lerp(ball01.position, _targetPosition01, Time.deltaTime * _shotSpeed01);

                if (Vector3.Distance(ball01.position, _targetPosition01) < 0.1f)
                {
                    _waitForBall01 = false;
                }
            }
            #endregion

            #region Ball02
            if (!_waitForBall02)
            {
                // Pickup a new position
                _targetPosition02 = PickupRandomTargetPosition(!_isDownSide02);
                _isDownSide02 = !_isDownSide02;
                _shotSpeed02 = Random.Range(minBallSpeed, maxBallSpeed);
                _waitForBall02 = true;
                AudioManager.Play(ballSounds[Random.Range(0, ballSounds.Length)]);

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
                    if (_theirScore == 11 || _myScore == 11)
                    {
                        // End: Pause the sequence and display end results
                        ResultData data = new ResultData();
                        data.matchTime = Time.time - _matchStartTime;
                        data.title = "Résultat du matche de pingpong";
                        data.results = new Dictionary<string, int>();
                        data.results.Add("moi", _myScore);
                        data.results.Add("eux", _theirScore);

                        data.teams = new Dictionary<string, List<string>>();
                        data.teams.Add("moi", new List<string>());
                        data.teams["moi"].Add(SportManager.staticSelectedSport.selectedPlayer.name);
                        data.teams.Add("eux", new List<string>());
                        data.teams["eux"].Add(PingPongPlayer.GetRandomPlayerName());

                        OnEndSport(data);
                    }
                }
            }
            else
            {
                ball02.position = Vector3.Lerp(ball02.position, _targetPosition02, Time.deltaTime * _shotSpeed02);

                if (Vector3.Distance(ball02.position, _targetPosition02) < 0.1f)
                {
                    _waitForBall02 = false;
                }
            }
            #endregion

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