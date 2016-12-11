using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CoachSimulator
{
    public class SumoMatch : GameType
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

        public AudioClip[] sumoClips;
        public AudioClip[] steps;
        public AudioClip sumoLose;

        public Transform player01, player02;

        public Vector3 player1P1, player1P2;
        public Vector3 player2P1, player2P2;
        public Vector3 fightArea;

        public float fightMinRate = 3f, fightMaxRate = 8f;

        private bool _player01Animation = false, _player02Animation = false;
        private bool _player01Rotation = false, _player02Rotation = false;
        private int _player01RotCount = 0, _player02RotCount = 0;
        private int _player01MaxRot = 0, _player02MaxRot = 0;

        private int _fightRotCount = 0;

        private Vector3 _targetPos01, _targetPos02;
        private Vector3 _targetRot01, _targetRot02;

        private Vector3 _dir01, _dir02;
        private float _magnitude01 = -1f, _magnitude02 = -1f;

        public Text score;

        private int _myScore, _theirScore;

        private float _matchStartTime;

        private bool _roundOver = false;
        private bool _player01Winner = false;
        private bool _isFighting = false;

        #region Events
        private float _nextRotEvent01, _nextRotEvent02;
        private float _nextFightEvent = -1f;
        private float _nextRoundEvent = -1f;
        #endregion

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

            _dir01 = player1P2 - player1P1;
            _magnitude01 = _dir01.magnitude;
            _dir01.Normalize();

            _dir02 = player2P2 - player2P1;
            _magnitude02 = _dir02.magnitude;
            _dir02.Normalize();

            score.text = "<b>Score: 0/0 </b>";

            _isFighting = false;

            _targetPos01 = PickupRandomTargetPosition(true);
            _targetRot01 = new Vector3(0, 0, 40);
            _targetPos02 = PickupRandomTargetPosition(false);
            _targetRot02 = new Vector3(0, 0, 40);

            _nextCoachEvent = Time.time + Random.Range(0, coachEventRandomTime);
            Coach.instance.OnEndChoice = OnEndCoachEvent;

            SportUI.instance.EnableSport("Sumo");

            // Setup the next fight event:
            _nextFightEvent = Time.time + Random.Range(fightMinRate, fightMaxRate);

            _player01Animation = true;
            _player02Animation = true;

            _player01Rotation = false;
            _player02Rotation = false;

            _fightRotCount = 0;

            _roundOver = false;
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

            if (_roundOver)
            {
                //Debug.Log("Round over.. waiting");
                if (Time.time >= _nextRoundEvent)
                {
                    _roundOver = false;
                    _fightRotCount = 0;
                    _nextFightEvent = Time.time + Random.Range(fightMinRate, fightMaxRate);
                    _player01Animation = true;
                    _player01Rotation = false;
                    _targetPos01 = PickupRandomTargetPosition(true);
                    _player02Animation = true;
                    _player02Rotation = false;
                    _targetPos02 = PickupRandomTargetPosition(false);

                    player01.localEulerAngles = Vector3.zero;
                    player02.localEulerAngles = Vector3.zero;

                    _player01RotCount = 0;
                    _player02RotCount = 0;
                }
                else
                {
                    return;
                }
            }

            #region FightEvent
            if (Time.time >= _nextFightEvent && !_isFighting)
            {
                // Processing the fight event
                player01.position = fightArea;
                player02.position = fightArea;

                _player01MaxRot = Random.Range(3, 6);
                _player02MaxRot = _player01MaxRot;

                _player01RotCount = 0;
                _player02RotCount = 0;

                _player01Animation = false;
                _player01Rotation = true;
                _player02Animation = false;
                _player02Rotation = true;

                _nextRotEvent01 = Time.time + 0.5f;
                _nextRotEvent02 = Time.time + 0.5f;

                _fightRotCount = 0;

                _isFighting = true;

                AudioManager.Play(steps[0]);

            }

            if (_isFighting && _fightRotCount >= _player01MaxRot)
            {
                _isFighting = false;
                _fightRotCount = 0;
                _nextFightEvent = Time.time + Random.Range(fightMinRate, fightMaxRate) + _player01MaxRot / 2f;
                _roundOver = true;
                // Who's winner ?
                _player01Winner = Random.Range(0, 2) == 0 ? true : false;
                if (_player01Winner)
                {
                    player02.localEulerAngles = new Vector3(0, 0, 180);
                    _myScore++;
                }
                else
                {
                    AudioManager.Play(sumoLose);
                    player01.localEulerAngles = new Vector3(0, 0, 180);
                    _theirScore++;
                }
                _nextRoundEvent = Time.time + 2f;
                score.text = string.Format("<b>Score: {0}/{1}</b>", _myScore, _theirScore);

                if (_myScore == 5 || _theirScore == 5)
                {
                    // End of match
                    // End: Pause the sequence and display end results
                    ResultData data = new ResultData();
                    data.matchTime = Time.time - _matchStartTime;
                    data.title = "Résultat du tripotage de bourlets";
                    data.results = new Dictionary<string, int>();
                    data.results.Add("moi", _myScore);
                    data.results.Add("lui", _theirScore);

                    data.teams = new Dictionary<string, List<string>>();
                    data.teams.Add("moi", new List<string>());
                    data.teams["moi"].Add(SportManager.staticSelectedSport.selectedPlayer.name);
                    data.teams.Add("lui", new List<string>());
                    data.teams["lui"].Add(SumoPlayer.GetRandomPlayerName());

                    Coach.instance.score += 200f;
                    Coach.instance.experience += 200f;

                    OnEndSport(data);
                    return;
                }
            }
            #endregion

            #region Player01
            // If the player is moving horizontaly
            if (_player01Animation && !_isFighting)
            {
                if (Random.Range(0, 100) < 10)
                {
                    AudioManager.Play(sumoClips[Random.Range(0, sumoClips.Length)]);
                }
                // Updates player's position
                player01.position = Vector3.Lerp(player01.position, _targetPos01, Time.deltaTime * 3f);

                // If the destination is nearly reached
                if (Vector3.Distance(player01.position, _targetPos01) < 0.4f)
                {
                    // Randomly choose to animate the rotation 50%
                    _player01Rotation = Random.Range(0, 2) == 0 ? true : false;

                    if (_player01Rotation)
                    {
                        // Play the rotation
                        _player01RotCount = 0;
                        _player01Animation = false;
                        _nextRotEvent01 = Time.time + 0.5f;
                        _targetRot01 = -_targetRot01;
                        _player01MaxRot = Random.Range(2, 6);
                    }
                    else
                    {
                        // Pickup another position
                        _targetPos01 = PickupRandomTargetPosition(true);
                    }
                }
            }

            // If player rotation
            if (_player01Rotation && Time.time >= _nextRotEvent01)
            {
                if (_isFighting)
                {
                    _fightRotCount++;
                }

                if (Random.Range(0, 100) < 20)
                {
                    AudioManager.Play(steps[Random.Range(1, steps.Length)]);
                }

                if (Random.Range(0, 100) < 20 && _isFighting)
                {
                    AudioManager.Play(sumoClips[Random.Range(0, sumoClips.Length)]);
                }

                // Incrementing the rotation count
                _player01RotCount++;
                player01.localEulerAngles = _targetRot01;

                // Exit condition is the number of rotations
                if (_player01RotCount == _player01MaxRot && !_isFighting)
                {
                    _player01Rotation = false;
                    _player01Animation = true;
                    player01.localEulerAngles = Vector3.zero;
                    _targetPos01 = PickupRandomTargetPosition(true);
                }
                else
                {
                    // Setup a next rotation event
                    _nextRotEvent01 = Time.time + 0.5f;
                    _targetRot01 = -_targetRot01;
                }
            }
            #endregion

            #region Player02
            // If the player is moving horizontaly
            if (_player02Animation && !_isFighting)
            {
                if (Random.Range(0, 100) < 10)
                {
                    AudioManager.Play(sumoClips[Random.Range(0, sumoClips.Length)]);
                }
                // Updates player's position
                player02.position = Vector3.Lerp(player02.position, _targetPos02, Time.deltaTime * 3f);

                // If the destination is nearly reached
                if (Vector3.Distance(player02.position, _targetPos02) < 0.4f)
                {
                    // Randomly choose to animate the rotation 50%
                    _player02Rotation = Random.Range(0, 2) == 0 ? true : false;

                    if (_player02Rotation)
                    {
                        // Play the rotation
                        _player02RotCount = 0;
                        _player02Animation = false;
                        _nextRotEvent02 = Time.time + 0.5f;
                        _targetRot02 = -_targetRot02;
                        _player02MaxRot = Random.Range(2, 6);
                    }
                    else
                    {
                        // Pickup another position
                        _targetPos02 = PickupRandomTargetPosition(false);
                    }
                }
            }

            // If player rotation
            if (_player02Rotation && Time.time >= _nextRotEvent02)
            {
                if(Random.Range(0, 100) < 20)
                {
                    AudioManager.Play(steps[Random.Range(1, steps.Length)]);
                }

                if (Random.Range(0, 100) < 20 && _isFighting)
                {
                    AudioManager.Play(sumoClips[Random.Range(0, sumoClips.Length)]);
                }
                // Incrementing the rotation count
                _player02RotCount++;
                player02.localEulerAngles = _targetRot02;

                // Exit condition is the number of rotations
                if (_player02RotCount == _player02MaxRot && !_isFighting)
                {
                    _player02Rotation = false;
                    _player02Animation = true;
                    player02.localEulerAngles = Vector3.zero;
                    _targetPos02 = PickupRandomTargetPosition(false);
                }
                else
                {
                    // Setup a next rotation event
                    _nextRotEvent02 = Time.time + 0.5f;
                    _targetRot02 = -_targetRot02;
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

        private Vector3 PickupRandomTargetPosition(bool isPlayer01)
        {
            if (isPlayer01)
            {
                return player1P1 + _dir01 * Random.Range(0, _magnitude01);
            }
            else
            {
                return player2P1 + _dir02 * Random.Range(0, _magnitude02);
            }
        }
    }
}