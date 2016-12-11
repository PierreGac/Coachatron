using UnityEngine;

namespace CoachSimulator
{
    public class Petanque : Sport
    {
        public override string sportName
        {
            get
            {
                return "Petanque";
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


        public Vector3 p1, p2;
        public Transform[] balls;
        public AudioClip[] ballClips;

        public override void Start()
        {
            base.Start();



            for (int i = 0; i < rules.Length; i++)
            {
                if (rules[i] is PetanqueSingleMatch)
                {
                    PetanqueSingleMatch r = rules[i] as PetanqueSingleMatch;
                    r.p1 = p1;
                    r.p2 = p2;
                    r.balls = new Transform[balls.Length];
                    balls.CopyTo(r.balls, 0);
                    r.ballSounds = new AudioClip[ballClips.Length];
                    ballClips.CopyTo(r.ballSounds, 0);
                }

                if (rules[i] is PetanqueChampionship)
                {
                    PetanqueChampionship r = rules[i] as PetanqueChampionship;
                    r.p1 = p1;
                    r.p2 = p2;
                    r.balls = new Transform[balls.Length];
                    balls.CopyTo(r.balls, 0);
                    r.ballSounds = new AudioClip[ballClips.Length];
                    ballClips.CopyTo(r.ballSounds, 0);
                }

                rules[i].OnEndSport = OnEndSport;
            }
        }

        private void Update()
        {
            if (!isActive)
            {
                return;
            }

            if (!board.activeInHierarchy)
            {
                board.SetActive(true);
            }

            selectedRule.UpdateGame();
        }
    }
}