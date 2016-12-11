using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace CoachSimulator
{
    public class PingPongResult : ResultScreen
    {

        public SocksGame socksGame;

        public override void DisplayResultMenu(ResultData data)
        {
            base.DisplayResultMenu(data);
        }

        public override void OnClickNext()
        {
            if (Random.Range(0, 3) == 0 || true)
            {
                // Play mini game
                socksGame.OnEndGame = OnMiniGameEnd;
                socksGame.StartGame();
                MessageBox.instance.Show("Corvée de chaussettes !!");
            }
            else
            {
                SkillsUI.instance.Show();
            }
            AudioManager.PlayPloup();
            _canvas.alpha = 0;
            _canvas.blocksRaycasts = false;
        }

        private void OnMiniGameEnd()
        {
            SkillsUI.instance.Show();
        }
    }
}