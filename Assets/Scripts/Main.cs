using UnityEngine;
using UnityEngine.UI;
using CaseExtensions;

namespace NeuroGen
{
    public class Main : MonoBehaviour
    {
        public int genomeCount;
        public int inputCount;
        public int outputCount;
        public int bestModelsCount;
        public GameObject carPrefab;
        public GameObject[] extensions;
        public GameObject defaultCamera;
        public TrackSystemInfo defaultTrackSystemInfo;
        public int selectedExtensionIndex;
        public int selectedExtensionId;
        public float timeScale = 5;
        private CarController[] carControllers;
        public Text speedText;
        private static Main instance = null;
        public static Main Instance { get { return instance; } }
        void Start()
        {
            instance = this;
            Time.timeScale = timeScale;
            defaultCamera.SetActive(true);
            defaultTrackSystemInfo.gameObject.SetActive(true);

            Database.ConnectAndSetup(Application.persistentDataPath + "/db.sqlite");

            if (selectedExtensionIndex >= 0 && selectedExtensionIndex < extensions.Length)
            {
                var extensionMainObject = extensions[selectedExtensionIndex];
                selectedExtensionId = Database.GetExtensionId(extensionMainObject.name.ToKebabCase());
                extensionMainObject.SetActive(true);
            }

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
            timeScale = GUI.HorizontalSlider(new Rect(10, 10, 350, 30), timeScale, 1, 100);

            Time.timeScale = timeScale;
            speedText.text = timeScale.ToString("Speed: 0");
        }
    }
}