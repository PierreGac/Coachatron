using UnityEngine;

namespace CoachSimulator
{
    public interface IGameType
    {
        string title { get; }
        string description { get; }
    }
}