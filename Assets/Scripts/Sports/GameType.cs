using UnityEngine;

namespace CoachSimulator
{
    public abstract class GameType : MonoBehaviour, IGameType
    {
        public delegate void GameTypeEvent(ResultData data);
        public GameTypeEvent OnEndSport;

        public GameObject board;

        public bool unlocked = true;

        public abstract string title { get; }
        public abstract string description { get; }

        public abstract string disabledDescription { get; }

        public virtual void StartGame()
        {
            AudioManager.instance.PlayAmbient();
        }

        public virtual void EndGame()
        {

        }

        public abstract void UpdateGame();
    }
}
