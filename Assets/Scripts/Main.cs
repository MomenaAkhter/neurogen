using UnityEngine;

namespace NeuroGen
{
    public class Main : MonoBehaviour
    {
        public GameObject[] extensions;
        public int selectedExtensionIndex;
        private float timeScale = 5;
        void Start()
        {
            Time.timeScale = 5;

            if (selectedExtensionIndex >= 0 && selectedExtensionIndex < extensions.Length)
                extensions[selectedExtensionIndex].SetActive(true);
            // int count = 1;
            // cars = new Car[count];
            // for (int i = 0; i < count; i++)
            //     cars[i] = Instantiate<Car>(car);

            // cars[0].humanControlled = true;
        }

        void OnGUI()
        {
            timeScale = GUI.HorizontalSlider(new Rect(25, 5, 100, 30), timeScale, 0.0F, 100.0F);

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