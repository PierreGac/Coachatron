using UnityEngine;
using System.Collections;

namespace CoachSimulator
{
    public class SockTarget : MonoBehaviour, ITargetSock
    {
        public enum Type
        {
            Bad,
            Good,
        }

        public enum State
        {
            Idle,
            Falling,
        }

        [SerializeField]
        private Type _type = Type.Bad;
        public Type type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }

        [SerializeField]
        private State _state = State.Idle;
        public State state
        {
            get
            {
                return _state;
            }
            set
            {
                if(_state == State.Idle && value == State.Falling)
                {
                    _fallingSpeed = Random.Range(minFallingSpeed, maxFallingSpeed);
                }
                _state = value;
            }
        }

        public int score = 10;

        private Vector3 _endStart;
        public Vector3 endStart
        {
            get
            {
                return _endStart;
            }
            set
            {
                _endStart = value;
            }
        }

        private Vector3 _endEnd;
        public Vector3 endEnd
        {
            get
            {
                return _endEnd;
            }
            set
            {
                _endEnd = value;
            }
        }

        public float minFallingSpeed = 7f, maxFallingSpeed = 20f;

        private float _fallingSpeed = 10f;

        private void Update()
        {
            if(_state == State.Idle)
            {
                return;
            }
            // Fall:
            transform.position -= Vector3.up * Time.deltaTime * _fallingSpeed;
        }

        public int Validate()
        {
            return score;
        } 
    }
}