using UnityEngine;
using System;

namespace CoachSimulator
{
    [Serializable]
    public class PetanquePlayerStats
    {
        public Sprite sprite;
        public string name;
        public int age;

        public int experience;

        public enum LiverState
        {
            Good,
            Bad,
            Ah,
        }
        public LiverState liverState;

        public string liverStateString
        {
            get
            {
                switch (liverState)
                {
                    case LiverState.Good:
                        return "Bon etat";
                    case LiverState.Bad:
                        return "Mauvais etat";
                    case LiverState.Ah:
                        return "Ah?";
                    default:
                        return "Hein?";
                }
            }
        }

        public string shortDescription;

        public string GetDescription()
        {
            string str = "";

            str = string.Format("Nom: {0} \r\nAge: {1}\r\nÉtat du foi: {2}\r\nPratique la pétanque depuis {3} ans\r\n", name, age, liverStateString, experience);

            str = string.Format("{0}<i>{1}</i>", str, shortDescription);
            return str;
        }
    }
}