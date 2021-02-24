using UnityEngine;

namespace NeuroGen
{
    public class Main : MonoBehaviour
    {
        public GameObject[] extensions;
        public int selectedExtensionIndex;
        public float timeScale = 5;
        public CarController car;
        private CarController[] cars;
        public GameObject spawnPoint;
        void Start()
        {
            Time.timeScale = timeScale;

            if (selectedExtensionIndex >= 0 && selectedExtensionIndex < extensions.Length)
                extensions[selectedExtensionIndex].SetActive(true);

            // int count = 1;
            // cars = new CarController[count];
            // for (int i = 0; i < count; i++)
            // {
            //     cars[i] = Instantiate<CarController>(car);
            //     cars[i].transform.SetPositionAndRotation(spawnPoint.transform.position, spawnPoint.transform.rotation);
            // }

            // cars[0].humanControlled = true;
        }

        // private void FixedUpdate()
        // {
        //     cars[0].Step(null);
        //     var values = cars[0].SensorValues;
        //     string text = "";
        //     foreach (var value in values)
        //         text += value + " ";
        //     Debug.Log(text);
        // }

        void OnGUI()
        {
            timeScale = GUI.HorizontalSlider(new Rect(25, 5, 200, 30), timeScale, 1, 100);

            Time.timeScale = timeScale;

            // Debug.Log(Time.timeScale);
        }

        // void OnDrawGizmos()
        // {
        // var mesh = track.GetComponent<MeshFilter>().sharedMesh;

        // Gizmos.color = Color.red;
        // foreach (Vector3 vertex in mesh.vertices)
        //     Gizmos.DrawSphere(track.transform.TransformPoint(vertex), .1f);
        // }
    }
}