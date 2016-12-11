using System;
using UnityEngine;

namespace CoachSimulator
{
    public abstract class Sport : MonoBehaviour, ISport, ISportUI
    {
        public bool unlocked;

        public bool isActive = false;

        public GameType[] rules;
        [HideInInspector]
        public GameType selectedRule;

        public ISportPlayer[] players;

        public ResultScreen resultScreen;

        public ISportPlayer selectedPlayer = null;

        public GameObject board;

        public abstract Sprite sprite { get; set; }
        public abstract string sportName { get; }
        public abstract string description { get; set; }

        public virtual void Start()
        {
            Transform playerTransform = transform.FindChild("Players");
            if (playerTransform == null)
            {
                Debug.LogError("Unable to find the player transform");
                return;
            }
            players = new ISportPlayer[playerTransform.childCount];
            for (int i = 0; i < playerTransform.childCount; i++)
            {
                players[i] = playerTransform.GetChild(i).GetComponent<ISportPlayer>();
                if (players[i] == null)
                {
                    Debug.LogError("Error while getting player " + i);
                }
            }

            resultScreen.Hide();
            board.SetActive(false);
        }

        public virtual void OnEndSport(ResultData data)
        {
            isActive = false;
            board.SetActive(false);
            selectedRule.EndGame();
            Coach.instance.ForceHide();
            resultScreen.DisplayResultMenu(data);
        }

        public void UnlockRule(Type ruleType)
        {
            for (int i = 0; i < rules.Length; i++)
            {
                if (rules[i].GetType().Equals(ruleType))
                {
                    if (!rules[i].unlocked)
                    {
                        rules[i].unlocked = true;
                        MessageBox.instance.Show(string.Format("Nouveau type de jeu débloqué ! \r\n=>{0}", rules[i].title));
                    }
                    return;
                }
            }
        }
    }
}