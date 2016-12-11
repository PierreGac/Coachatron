using UnityEngine;
using System.IO;

namespace CoachSimulator
{
    public class SportChoices : MonoBehaviour
    {
        public static SportChoices instance = null;

        public CoachChoiceArray choices;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            // Gets the choices
            string path = string.Format("{0}/Choices/choices.json", Application.dataPath);
            choices = new CoachChoiceArray();
            if (File.Exists(path))
            {
                choices.Unserialize(path);
            }
            /*else
            {
                choices.entries = new CoachChoiceEntry[10];
                choices.Serialize(path);
            }*/
        }

        public CoachChoice PickRandomChoice()
        {
            CoachChoice choice = new CoachChoice();
            choice.left = choices.entries[Random.Range(0, choices.Length)];
            choice.up = choices.entries[Random.Range(0, choices.Length)];
            choice.right = choices.entries[Random.Range(0, choices.Length)];
            choice.down = choices.entries[Random.Range(0, choices.Length)];

            return choice;
        }

    }
}