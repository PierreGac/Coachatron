using UnityEngine;

namespace CoachSimulator
{
    public class PingPong : Sport
    {
        public override string sportName
        {
            get
            {
                return "PingPong";
            }
        }

        [SerializeField]
        private string _description;
        public override string description
        {
            get
            {
                return _description;
            }

            set
            {
                _description = value;
            }
        }

        [SerializeField]
        private Sprite _sprite;
        public override Sprite sprite
        {
            get
            {
                return _sprite;
            }

            set
            {
                _sprite = value;
            }
        }
        public Vector3 dSideStart, dSideEnd;
        public Vector3 uSideStart, uSideEnd;

        public AudioClip[] ballSounds;

        public override void Start()
        {
            base.Start();

            for(int i = 0; i < rules.Length; i++)
            {
                if(rules[i] is PingPongSingleMatch)
                {
                    PingPongSingleMatch r = rules[i] as PingPongSingleMatch;
                    r.dSideStart = dSideStart;
                    r.dSideEnd = dSideEnd;
                    r.uSideStart = uSideStart;
                    r.uSideEnd = uSideEnd;
                    r.ballSounds = new AudioClip[ballSounds.Length];
                    ballSounds.CopyTo(r.ballSounds, 0);
                }

                if (rules[i] is PingPongChampionship)
                {
                    PingPongChampionship r = rules[i] as PingPongChampionship;
                    r.dSideStart = dSideStart;
                    r.dSideEnd = dSideEnd;
                    r.uSideStart = uSideStart;
                    r.uSideEnd = uSideEnd;
                    r.ballSounds = new AudioClip[ballSounds.Length];
                    ballSounds.CopyTo(r.ballSounds, 0);
                }

                if (rules[i] is PingPongDouble)
                {
                    PingPongDouble r = rules[i] as PingPongDouble;
                    r.dSideStart = dSideStart;
                    r.dSideEnd = dSideEnd;
                    r.uSideStart = uSideStart;
                    r.uSideEnd = uSideEnd;
                    r.ballSounds = new AudioClip[ballSounds.Length];
                    ballSounds.CopyTo(r.ballSounds, 0);
                }
            }

            for (int i = 0; i < rules.Length; i++)
            {
                rules[i].OnEndSport = OnEndSport;
            }
        }

        private void Update()
        {
            if(!isActive)
            {
                return;
            }

            if(!board.activeInHierarchy)
            {
                board.SetActive(true);
            }

            selectedRule.UpdateGame();
        }
    }
}