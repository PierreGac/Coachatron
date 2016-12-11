using UnityEngine;
using System.Collections;

namespace CoachSimulator
{
    [RequireComponent(typeof(BoxCollider))]
    public class SocksDeleteArea : MonoBehaviour
    {
        public delegate void SocksEvent(ITargetSock sock);

        public SocksEvent DeleteEvent;

        private BoxCollider _collider;
        public bool active = false;

        private void Start()
        {
            _collider = GetComponent<BoxCollider>();
        }

        private void Update()
        {
            if(!active)
            {
                return;
            }


            Collider[] colliders = Physics.OverlapBox(transform.position, _collider.size / 2f);

            for(int i = 0; i < colliders.Length; i++)
            {
                if(colliders[i].GetComponent<ITargetSock>() != null)
                {
                    DeleteEvent(colliders[i].GetComponent<ITargetSock>());
                }
            }
        }
    }
}