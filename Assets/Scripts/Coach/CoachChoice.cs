using UnityEngine;
using System;
using System.IO;

namespace CoachSimulator
{
    /// <summary>
    /// Contains all the choices froma json file
    /// </summary>
    [Serializable]
    public class CoachChoiceArray
    {
        public CoachChoiceEntry this[int index]
        {
            get
            {
                return entries[index];
            }
            set
            {
                entries[index] = value;
            }
        }
        public CoachChoiceEntry[] entries;

        public int Length
        {
            get
            {
                return entries.Length;
            }
        }

        public void Serialize(string fullPath)
        {
            if (File.Exists(fullPath))
            {
                return;
            }

            string json = JsonUtility.ToJson(this, true);

            using (StreamWriter writer = new StreamWriter(File.Create(fullPath)))
            {
                writer.Write(json);
            }
        }

        public void Unserialize(string path)
        {
            CoachChoiceArray obj = JsonUtility.FromJson<CoachChoiceArray>(File.ReadAllText(path));
            entries = obj.entries;
        }
    }

    [Serializable]
    public class CoachChoice
    {
        public CoachChoiceEntry left, up, right, down;

        public float GetAverageTime()
        {
            return (left.time + up.time + right.time + down.time) / 4f;
        }

        public static void Serialize(CoachChoice choice, string fullPath)
        {
            if(File.Exists(fullPath))
            {
                return;
            }

            string json = JsonUtility.ToJson(choice, true);

            using (StreamWriter writer = new StreamWriter(File.Create(fullPath)))
            {
                writer.Write(json);
            }
        }

        public static CoachChoice Unserialize(string json)
        {
            return JsonUtility.FromJson<CoachChoice>(json);
        }
    }

    

    [Serializable]
    public class CoachChoiceEntry
    {
        public string ID;
        public string choiceText;
        public string sayText;
        public float pointsMin, pointsMax;
        public float expMin, expMax;
        public float time = 3f;
    }
}