using UnityEngine;
using System.Collections;

namespace CoachSimulator
{
    public interface ITargetSock
    { 
        SockTarget.Type type { get; set; }
        SockTarget.State state { get; set; }

        Transform transform { get; }

        int Validate();
    }
}