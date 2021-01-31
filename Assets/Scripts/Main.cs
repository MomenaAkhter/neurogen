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

        // void Update()
        // {
        // var rigidbody = cars[0].GetComponent<Rigidbody>();
        // rigidbody.velocity = Vector3.forward * Input.GetAxis("Vertical") * 100 * Time.deltaTime;
        // rigidbody.angularVelocity = Vector3.up * Input.GetAxis("Horizontal") * 10 * Time.deltaTime * rigidbody.velocity.magnitude;

        // cars[0].wheels[0].transform.rotation = Quaternion.Euler(0, 45 * Input.GetAxis("Horizontal"), 0);
        // cars[0].wheels[1].transform.rotation = Quaternion.Euler(0, 45 * Input.GetAxis("Horizontal"), 0);
        // }
    }
}