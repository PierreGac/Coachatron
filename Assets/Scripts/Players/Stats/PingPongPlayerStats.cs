using UnityEngine;
using System;

namespace CoachSimulator
{
    [Serializable]
    public class PingPongPlayerStats
    {
        public Sprite sprite;
        public string name;
        public int age;

        public int experience;

        public Color color;

        public enum Pillow
        {
            Polochon,
            Traversein,
            Planche,
            Oreiller,
            Fesses,
        }

        public Pillow pillow;

        public enum Politic
        {
            Chocolatine,
            PainAuChocolat,
        }
        public Politic politic;


        public string shortDescription;

        public string GetDescription()
        {
            string str = "";

            str = string.Format("<b>Nom:</b>{0} \r\n<b>Age:</b> {1}\r\nPratique le pingpong depuis {2} ans\r\n", name, age, experience);
            str = string.Format("{0}<b>Couleur favorite:</b> {1}\r\n", str, color.ToString());
            str = string.Format("{0}<b>Oreiller de voyage:</b> {1}\r\n", str, pillow.ToString());
            str = string.Format("{0}<b>Affinité politique:</b> {1}\r\n", str, politic.ToString());
            str = string.Format("{0}<i>{1}</i>", str, shortDescription);
            return str;
        }
    }
}