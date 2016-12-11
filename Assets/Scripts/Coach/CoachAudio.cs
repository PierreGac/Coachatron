using UnityEngine;
using System.Collections;

namespace CoachSimulator
{
    public class CoachAudio : MonoBehaviour
    {
        public AudioClip[] reactionClips;

        public AudioClip GetRandomReactionClip()
        {
            return reactionClips[Random.Range(0, reactionClips.Length)];
        }
    }
}