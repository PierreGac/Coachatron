using UnityEngine;
using System;

namespace CoachSimulator
{
    [Serializable]
    public class CurlingPlayerStats
    {
        public Sprite sprite;
        public string name;
        public int age;

        public int experience;

        public string quote;

        public enum Dog
        {
            BergerSuisse,
            Tequel,
            Levrier,
            Poney,
        }

        public Dog dog;

        public bool isBrico;

        public string shortDescription;

        public string GetDescription()
        {
            string str = "";

            str = string.Format("<b>Nom:</b>{0} \r\n<b>Age:</b> {1}\r\nPratique le curling depuis {2} ans\r\n", name, age, experience);
            str = string.Format("{0}<b>Citation favorite:</b> {1}\r\n", str, quote);
            str = string.Format("{0}<b>Rece de chien préféré:</b> {1}\r\n", str, dog.ToString());
            str = string.Format("{0}<b>Est bricoleur?:</b> {1}\r\n", str, isBrico == true ? "Oui" : "Non");
            str = string.Format("{0}<i>{1}</i>", str, shortDescription);
            return str;
        }
    }
}