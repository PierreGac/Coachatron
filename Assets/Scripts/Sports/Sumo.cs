using System;
using UnityEngine;

namespace CoachSimulator
{
    public class Sumo : Sport
    {
        public override string sportName
        {
            get
            {
                return "Sumo";
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


        public Vector3 player1P1, player1P2;
        public Vector3 player2P1, player2P2;
        public Vector3 fightArea;

        public AudioClip[] sumoSounds;

        public override void Start()
        {
            base.Start();

            for (int i = 0; i < rules.Length; i++)
            {
                if(rules[i] is SumoMatch)
                {
                    SumoMatch r = rules[i] as SumoMatch;
                    r.player1P1 = player1P1;
                    r.player1P2 = player1P2;
                    r.player2P1 = player2P1;
                    r.player2P2 = player2P2;
                    r.fightArea = fightArea;
                }

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