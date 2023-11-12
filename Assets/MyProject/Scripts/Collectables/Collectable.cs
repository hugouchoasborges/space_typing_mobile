using UnityEngine;

namespace collectable
{
    public class Collectable : MonoBehaviour
    {
        [SerializeField][Range(0, 100)] private int _points = 1;
        public int Points => _points;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
