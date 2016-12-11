using UnityEngine;

namespace CoachSimulator
{
    public interface ISportPlayer
    {
        string name { get; }
        Sprite playerSprite { get; }

        string GetDescription();
    }
}