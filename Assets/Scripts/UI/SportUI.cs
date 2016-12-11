using UnityEngine;

namespace CoachSimulator
{
    public class SportUI : MonoBehaviour
    {
        public static SportUI instance = null;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }


        private void Start()
        {
            EnableSport("");
        }

        public void EnableSport(string name)
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                if(transform.GetChild(i).name == name)
                {
                    transform.GetChild(i).GetComponent<CanvasGroup>().alpha = 1;
                    transform.GetChild(i).GetComponent<CanvasGroup>().blocksRaycasts = true;
                }
                else
                {
                    transform.GetChild(i).GetComponent<CanvasGroup>().alpha = 0;
                    transform.GetChild(i).GetComponent<CanvasGroup>().blocksRaycasts = false;
                }
            }
        }

    }
}