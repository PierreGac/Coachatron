using UnityEngine;
using System.Collections.Generic;

namespace CoachSimulator
{
    public class ResultData
    {
        public Dictionary<string, List<string>> teams;

        public float matchTime;

        public string title;

        public Dictionary<string, int> results;

        public string GetTeamsDescription()
        {
            string str = "";
            string team;

            foreach(KeyValuePair<string, List<string>> entry in teams)
            {
                team = "Equipes:";
                for(int i = 0; i < entry.Value.Count; i++)
                {
                    team = string.Format("{0} {1}", team, entry.Value[i]);
                }
                str = string.Format("{0}{1}: {2}\r\n", str, entry.Key, team);
            }

            return str;
        }

        public string GetTeamsResults()
        {
            string str = "";


            foreach (KeyValuePair<string, int> entry in results)
            {
                str = string.Format("{0}{1}: {2}\r\n", str, entry.Key, entry.Value);
            }

            return str;
        }
    }
}