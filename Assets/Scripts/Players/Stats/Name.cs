using UnityEngine;
using System;
using System.IO;

namespace CoachSimulator
{
    [Serializable]
    public class Names
    {

        public string[] names;

        public string this[int index]
        {
            get
            {
                return names[index];
            }
            set
            {
                names[index] = value;
            }
        }

        public int Length
        {
            get
            {
                return names.Length;
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
            Names obj = JsonUtility.FromJson<Names>(File.ReadAllText(path));
            names = obj.names;
        }
    }
}