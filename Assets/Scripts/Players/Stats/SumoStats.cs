using UnityEngine;
using System;

namespace CoachSimulator
{
    [Serializable]
    public class SumoStats
    {
        public Sprite sprite;
        public string name;
        public int age;

        public int experience;

        public float mass;

        public string favoriteFood;

        public enum RedOrBlue
        {
            Rouge,
            Bleu,
        }

        public RedOrBlue redOrBlue;

        public string shortDescription;

        public string GetDescription()
        {
            string str = "";

            str = string.Format("<b>Nom:</b> {0} \r\n<b>Age:</b> {1}\r\nPratique le pingpong depuis {2} ans\r\n", name, age, experience);
            str = string.Format("{0}<b>Plat préféré:</b> {1}\r\n", str, favoriteFood);
            str = string.Format("{0}<b>Bleu ou rouge?:</b> {1}\r\n", str, redOrBlue.ToString());
            str = string.Format("{0}<i>{1}</i>", str, shortDescription);
            return str;
        }
    }
}