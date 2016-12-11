using System.IO;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace CoachSimulator
{
    public class Scores : MonoBehaviour
    {
        [Serializable]
        public class ScoreData
        {
            public List<string> names;
            public List<int> scores;

            public ScoreData()
            {
                names = new List<string>();
                scores = new List<int>();
            }

            public void Add(string name, int score)
            {
                names.Add(name);
                scores.Add(score);
            }

            public string GetString()
            {
                string str = "";

                for(int i = 0; i < names.Count; i++)
                {
                    str = string.Format("{0}{1} :: {2}\r\n", str, names[i], scores[i]);
                }

                return str;
            }
        }

        public static Scores instance = null;

        public Text text;

        private ScoreData _scores;
        private CanvasGroup _canvas;
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

            Unserialize(Application.dataPath + "/scores.json");
        }

        public void PushScore(string name, int score)
        {
            _scores.Add(name, score);
            Serialize(Application.dataPath + "/scores.json");
        }

        private void Start()
        {
            _canvas = GetComponent<CanvasGroup>();
            Hide();
        }

        public void OnClickBack()
        {
            AudioManager.PlayPloup();
            Hide();
            MainMenu.instance.Show();
        }

        public void Show()
        {
            _canvas.alpha = 1;
            _canvas.blocksRaycasts = true;
            text.text = _scores.GetString();
        }

        public void Hide()
        {
            _canvas.alpha = 0;
            _canvas.blocksRaycasts = false;
        }

        public void Serialize(string fullPath)
        {
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            string json = JsonUtility.ToJson(_scores, true);

            using (StreamWriter writer = new StreamWriter(File.Create(fullPath)))
            {
                writer.Write(json);
            }
        }

        public void Unserialize(string path)
        {
            _scores = JsonUtility.FromJson<ScoreData>(File.ReadAllText(path));
        }
    }
}