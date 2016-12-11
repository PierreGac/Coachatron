using UnityEngine;

namespace CoachSimulator
{
    public interface ISportUI
    {
        Sprite sprite { get; set; }
        string sportName { get; }
        string description { get; set; }
    }
}