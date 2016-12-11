using UnityEngine;
using System.Collections;


namespace CoachSimulator
{
    public class SportManager : MonoBehaviour
    {
        public Sport[] sports;

        [HideInInspector]
        public Sport selectedSport;

        public static Sport staticSelectedSport;


    }
}