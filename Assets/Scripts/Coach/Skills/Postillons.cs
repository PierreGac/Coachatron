using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CoachSimulator.Skills
{
    public class Postillons : SkillData
    {
        public Postillons highRank;
        public GameObject postillons;
        public float rate = 1.5f;
        public int maxCalls = 3;

        private float _nextEvent;
        private int _count = 0;

        private bool _animate = false;

        public override void OnOverlay()
        {
            base.OnOverlay();
        }

        public override void EnableSkill()
        {
            if (isEnoughPoints())
            {
                base.EnableSkill();
                if (highRank == null || !highRank.active)
                {
                    Coach.instance.SayEvent += AnimatePostillons;
                }
            }
        }

        public override void DisableSkill()
        {
            base.DisableSkill();
            if (highRank == null || !highRank.active)
            {
                Coach.instance.SayEvent -= AnimatePostillons;
            }
        }

        private void AnimatePostillons()
        {
            _animate = true;
            _nextEvent = Time.time + rate;
            _count = 0;
            postillons.SetActive(true);
        }

        private void Update()
        {
            if(isRunning && active && _animate)
            {
                if(Time.time >= _nextEvent)
                {
                    _count++;
                    if(_count >= maxCalls)
                    {
                        _animate = false;
                        postillons.SetActive(false);
                        return;
                    }

                    if(_count % 2 == 0)
                    {
                        postillons.SetActive(false);
                    }
                    else
                    {
                        postillons.SetActive(true);
                    }
                    _nextEvent = Time.time + rate;
                }
            }
        }
    }
}