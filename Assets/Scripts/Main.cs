using UnityEngine;

namespace NeuroGen
{
    public class Main : MonoBehaviour
    {
        public Car car;
        static public Car[] cars;
        public GameObject track;
        static public Vector3 startPosition;
        void Start()
        {
            startPosition = new Vector3(-7.75f, 1.39f, -19.96f);

            int count = 1;
            cars = new Car[count];
            for (int i = 0; i < count; i++)
                cars[i] = Instantiate<Car>(car, startPosition, Quaternion.identity);

            cars[0].humanControlled = true;
        }

        void OnDrawGizmos()
        {
            var mesh = track.GetComponent<MeshFilter>().sharedMesh;

            Gizmos.color = Color.red;
            foreach (Vector3 vertex in mesh.vertices)
            {
                Gizmos.DrawSphere(track.transform.TransformPoint(vertex), .1f);
            }
        }
    }
}