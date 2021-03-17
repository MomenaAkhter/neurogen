using UnityEngine;
using UnityEngine.UI;
using CaseExtensions;

namespace NeuroGen
{
    public enum StartMode
    {
        RandomModels,
        BestModels
    }
    public enum Status
    {
        Running,
        Paused,
        Stopped
    }
    public class Main : MonoBehaviour
    {
        public int genomeCount;
        public int inputCount;
        public int outputCount;
        public int bestModelsCount;
        public StartMode startMode = StartMode.RandomModels;
        public Status status = Status.Stopped;
        public GameObject carPrefab;
        public GameObject[] extensions;
        public GameObject defaultCamera;
        public GameObject startMenu;
        public HUD hud;
        public TrackSystemInfo defaultTrackSystemInfo;
        public int selectedExtensionIndex;
        public int selectedExtensionId;
        public float timeScale = 5;
        private CarController[] carControllers;
        public Text speedText;
        private static Main instance = null;
        public static Main Instance { get { return instance; } }
        public int survivors;
        public float highestFitness;
        void Start()
        {
            instance = this;
            hud.speedSlider.value = timeScale;
            defaultCamera.SetActive(true);
            defaultTrackSystemInfo.gameObject.SetActive(true);

            Database.ConnectAndSetup(Application.persistentDataPath + "/db.sqlite");

            // int count = 1;
            // cars = new CarController[count];
            // for (int i = 0; i < count; i++)
            // {
            //     cars[i] = Instantiate<CarController>(car);
            //     cars[i].transform.SetPositionAndRotation(spawnPoint.transform.position, spawnPoint.transform.rotation);
            // }

            // cars[0].humanControlled = true;
        }

        void ManageSelectedExtension(bool activate = true)
        {
            if (selectedExtensionIndex >= 0 && selectedExtensionIndex < extensions.Length)
            {
                var extensionMainObject = extensions[selectedExtensionIndex];
                selectedExtensionId = Database.GetExtensionId(extensionMainObject.name.ToKebabCase());

                survivors = 0;
                highestFitness = 0;

                extensionMainObject.SetActive(activate);
                status = activate ? Status.Running : Status.Stopped;
            }
        }
        void ActivateExtension()
        {
            ManageSelectedExtension(true);
        }
        void DeactivateExtension()
        {
            ManageSelectedExtension(false);
        }

        void FixedUpdate()
        {
            Time.timeScale = hud.speedSlider.value;
            hud.speedSliderValue.text = Time.timeScale.ToString("0");
            hud.survivorsLabel.text = "Survivors: " + survivors + "/" + genomeCount;
            hud.highestFitnessLabel.text = "Highest Fitness: " + highestFitness.ToString("0");
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

        public void StartWithRandomModels()
        {
            StartSimulation(StartMode.RandomModels);
        }
        public void StartWithBestModels()
        {
            StartSimulation(StartMode.BestModels);
        }

        public void StartSimulation(StartMode startMode)
        {
            this.startMode = startMode;
            startMenu.SetActive(false);

            ActivateExtension();

            hud.gameObject.SetActive(true);
        }

        public void StopSimulation()
        {
            DeactivateExtension();

            hud.gameObject.SetActive(false);
            startMenu.SetActive(true);
        }
        public void TogglePauseResume()
        {
            if (Time.timeScale > 0)
            {
                Time.timeScale = 0;
                hud.pauseResumeToggle.GetComponentInChildren<Text>().text = "Resume";
            }
            else
            {
                Time.timeScale = hud.speedSlider.value;
                hud.pauseResumeToggle.GetComponentInChildren<Text>().text = "Pause";
            }
        }
        public void Quit()
        {
            Application.Quit();
        }
    }
}