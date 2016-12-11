using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace CoachSimulator
{
    public class SocksGame : MonoBehaviour
    {
        public static SocksGame instance = null;

        public delegate void SocksEvent();


        public AudioClip ambientSound;

        public SocksEvent OnEndGame;

        public GameObject board;

        public Transform spawnPool;
        public SocksDeleteArea deleteArea;
        public Transform bucket;

        public Vector3 spawnStart, spawnEnd;

        public GameObject[] prefabs;

        public AudioClip good, bad;

        #region UI
        public GameObject ui;
        public Text score;
        public Button button;
        #endregion

        private float _magnitude;
        private Vector3 _dir;

        public float xBoundMin, xBoundMax;

        public float minTime = 10f, maxTime = 40f;

        public float spawnRate;

        private BoxCollider _collider;

        private Vector3 _initialPosition;

        public bool active = false;

        private int _score = 0;

        private List<ITargetSock> _pool;
        private List<ITargetSock> _inUse;
        private float _nextSpawnEvent = -1f;

        private float _endTime;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                DestroyImmediate(gameObject);
            }
        }

        private void Start()
        {
            _initialPosition = transform.position;
            _collider = bucket.GetComponent<BoxCollider>();
            deleteArea.DeleteEvent = OnSockDestroyEvent;
            Populate();
            Hide();
        }

        private void Populate()
        {
            // 20 socks stock
            int count = 20;
            Transform tr;

            _pool = new List<ITargetSock>();
            _inUse = new List<ITargetSock>();

            for (int i = 0; i < count; i++)
            {
                tr = Instantiate(prefabs[Random.Range(0, prefabs.Length)]).transform;
                tr.SetParent(spawnPool);
                tr.localPosition = Vector3.zero;
                tr.localRotation = Quaternion.identity;

                _pool.Add(tr.GetComponent<ITargetSock>());
            }
        }

        private void ResetPool()
        {
            for(int i = 0; i < _pool.Count; i++)
            {
                _pool[i].transform.localPosition = Vector3.zero;
                _pool[i].transform.localRotation = Quaternion.identity;
            }
        }

        public void StartGame()
        {
            AudioManager.instance.ambient.clip = ambientSound;
            AudioManager.instance.ambient.Play();
            Show();
            active = true;
            _dir = spawnEnd - spawnStart;
            _magnitude = _dir.magnitude;
            _dir.Normalize();
            ResetPool();
            _score = 0;
            _nextSpawnEvent = Time.time + spawnRate;
            for(int i = 0; i < _inUse.Count; i++)
            {
                _inUse[i].state = SockTarget.State.Idle;
                _inUse[i].transform.localPosition = Vector3.zero;
                _pool.Add(_inUse[i]);
            }
            _inUse.Clear();
            deleteArea.active = true;
            _endTime = Time.time + Random.Range(minTime, maxTime);
            button.gameObject.SetActive(false);
            score.text = "Score: 0";
        }

        public void FinishGame()
        {
            active = false;
            for (int i = 0; i < _inUse.Count; i++)
            {
                _inUse[i].state = SockTarget.State.Idle;
                _inUse[i].transform.localPosition = Vector3.zero;
                _pool.Add(_inUse[i]);
            }
            _inUse.Clear();

            button.gameObject.SetActive(true);
           
        }

        public void EndGame()
        {
            OnEndGame();
            active = false;
            Hide();
        }

        public Vector3 GetSpawnPoint()
        {
            return transform.TransformPoint((spawnStart + _dir * Random.Range(0, _magnitude)));
        }

        private void SpawnItem()
        {
            ITargetSock sock = _pool[Random.Range(0, _pool.Count)];
            _inUse.Add(sock);
            _pool.Remove(sock);

            sock.transform.position = GetSpawnPoint();
            _nextSpawnEvent = Time.time + spawnRate;
            sock.state = SockTarget.State.Falling;
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                StartGame();
            }
            if (!active)
            {
                return;
            }

            if(Time.time >= _endTime)
            {
                FinishGame();
            }

            if(Time.time >= _nextSpawnEvent)
            {
                SpawnItem();
            }
            float x = Input.GetAxis("Horizontal");

            Vector3 targetPos = bucket.position + Vector3.right * x;

            // Check bounds
            if (targetPos.x > xBoundMax || targetPos.x < xBoundMin)
            {
                return;
            }
            bucket.position = Vector3.Lerp(bucket.position, targetPos, Time.deltaTime * 25f);


            // Raycast Update

            Collider[] colliders = Physics.OverlapBox(bucket.position, _collider.size / 2f);
            ITargetSock sock = null;

            for(int i = 0; i < colliders.Length; i++)
            {
                sock = colliders[i].GetComponent<ITargetSock>();
                if (sock != null)
                {
                    if(sock.type == SockTarget.Type.Bad)
                    {
                        AudioManager.Play(bad);
                    }
                    else
                    {
                        AudioManager.Play(good);
                    }
                    // Process and delete
                    int temp = sock.Validate();
                    Coach.instance.score += temp;
                    _score += temp;
                    Coach.instance.experience += temp;
                    score.text = string.Format("Score: {0}", _score);

                    OnSockDestroyEvent(sock);
                }
            }
        }

        private void OnSockDestroyEvent(ITargetSock sock)
        {
            sock.transform.localPosition = Vector3.zero;
            sock.transform.localRotation = Quaternion.identity;

            sock.state = SockTarget.State.Idle;
            _pool.Add(sock);
            _inUse.Remove(sock);
        }

        public void Show()
        {
            board.SetActive(true);
            ui.SetActive(true);
        }

        public void Hide()
        {
            ui.SetActive(false);
            board.SetActive(false);
        }
    }
}