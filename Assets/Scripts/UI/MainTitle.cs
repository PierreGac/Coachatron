using UnityEngine;
using System.Collections;

namespace CoachSimulator
{
    public class MainTitle : MonoBehaviour
    {

        public Transform image;

        private Vector3 _targetScale;

        public AudioClip mainTitle;

        private float _endEvent;

        // Use this for initialization
        void Start()
        {
            _endEvent = Time.time + 5f;
            _targetScale = new Vector3(0.2f, 1, 1);
        }

        // Update is called once per frame
        void Update()
        {
            image.localScale = Vector3.Lerp(image.localScale, _targetScale, Time.deltaTime * 5);

            if(Vector3.Distance(image.localScale, _targetScale) <= 0.1f)
            {
                if (Mathf.Approximately(_targetScale.x, 0.2f))
                {
                    _targetScale = new Vector3(1, 1, 1);
                }
                else
                {
                    _targetScale = new Vector3(0.2f, 1, 1);
                }
            }

            if(Time.time >= _endEvent)
            {
                AudioManager.instance.music.clip = mainTitle;
                AudioManager.instance.music.Play();
                Destroy(gameObject);
            }
        }
    }
}