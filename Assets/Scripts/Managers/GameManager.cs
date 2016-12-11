using UnityEngine;

namespace CoachSimulator
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance = null;

        public enum GameState
        {
            waitForStart,
            started,
            pauseMenu,
        }

        public GameState gameState = GameState.waitForStart;

        public float timeWhenPause = 0f;

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

        private void Update()
        {
            switch (gameState)
            {
                case GameState.waitForStart:
                    break;
                case GameState.started:
                    if(Input.GetKeyDown(KeyCode.Escape))
                    {
                        // Show pause menu
                        PauseMenu.instance.Show();
                        gameState = GameState.pauseMenu;
                    }
                    break;
                case GameState.pauseMenu:
                    break;
                default:
                    break;
            }
        }
    }
}