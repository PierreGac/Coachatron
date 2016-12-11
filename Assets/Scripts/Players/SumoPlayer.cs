using System.IO;
using UnityEngine;

namespace CoachSimulator
{
    public class SumoPlayer : MonoBehaviour, ISportPlayer
    {
        public SumoStats stats;

        public Sprite playerSprite
        {
            get
            {
                return stats.sprite;
            }
        }

        public string GetDescription()
        {
            return string.Format("<b>Description:</b>\r\n{0}", stats.GetDescription());
        }

        private static Names _names;

        public static string GetRandomPlayerName()
        {
            if(_names == null)
            {
                _names = new Names();

                string path = string.Format("{0}/Names/names.json", Application.dataPath);
                if (File.Exists(path))
                {
                    _names.Unserialize(path);
                }
            }
            return string.Format("{0} {1}", _names[Random.Range(0, _names.Length)], _names[Random.Range(0, _names.Length)]);
        }
    }
}