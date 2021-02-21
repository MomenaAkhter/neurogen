using System.Collections.Generic;
using UnityEngine;
using NEAT;
using UnityEngine.UI;

public class GenomeCar : GenomeProxy
{
    #region Fields
    private NeuroGen.CarController carController = null;

    [SerializeField] private Text fitnessText = null;
    public SpriteRenderer speciesColor;

    [Header("Raycast stuff")]
    [SerializeField] private LayerMask obstacleLayer = 0;
    [SerializeField] private Transform raycastOrigin = null;
    [SerializeField] private Transform[] raycastEndPoints = null;

    private int finishCross = 0;
    private Vector3 lastPositionMark;
    private PopulationCar populationCar;

    private float currentMaxFitness = 0;
    private float lastMaxFitnessUpdate = 0;

    private List<NeuroGen.Checkpoint> checkpointPassed = new List<NeuroGen.Checkpoint>();

    public override bool IsDone
    {
        get
        {
            return !carController.isRunning;
        }

        set
        {
            carController.isRunning = !value;
        }
    }
    #endregion

    #region Monobehaviour
    private void Awake()
    {
        carController = gameObject.GetComponent<NeuroGen.CarController>();
        populationCar = FindObjectOfType<PopulationCar>();
    }

    private void FixedUpdate()
    {
        ActivateNeuralNet(carController.SensorValues);
        GenomeProperty.Fitness = carController.Fitness;
    }
    #endregion

    #region Override
    public override void Init(int id, Population popl)
    {
        base.Init(id, popl);
    }

    public override void ProcessNetworkOutput(float[] netOutputs)
    {
        base.ProcessNetworkOutput(netOutputs);
        carController.Step(netOutputs);
    }
    #endregion

    #region Public Methods
    public void Reinit(Transform targetPositionRotation)
    {
        carController.Reinit();
        transform.SetPositionAndRotation(
            targetPositionRotation.position,
            targetPositionRotation.rotation
        );

        GenomeProperty.Fitness = 0;

        finishCross = 0;
        checkpointPassed.Clear();

        currentMaxFitness = 0;
        lastMaxFitnessUpdate = Time.time;
    }

    #endregion

    #region Private methods
    /// <summary>
    /// Define the car's inputs.
    /// A for loop can't be used here, since lambda functions are defined.
    /// The last input is the speed.
    /// </summary>
    // private void AssignGenomeInputFunctions()
    // {
    //     InputFunctions = new GenomeInputFunction[Popl.Config.inputCount];

    //     for (int i = 0; i < carController.SensorValues.Length; i++)
    //     {
    //         // foreach (var value in carController.SensorValues)
    //         //     Debug.Log(value);
    //         Debug.Log(carController.SensorValues[0] + ", " + carController.SensorValues[1] + ", " + carController.SensorValues[2] + ", " + carController.SensorValues[3] + ", " + carController.SensorValues[4]);

    //         // InputFunctions[i] = () => carController.SensorValues[i];
    //         InputFunctions[i] = () => 0;
    //     }
    // }

    public void Die()
    {
        carController.Stop();
    }

    // private void ProcessFinihCross()
    // {
    //     finishCross++;
    //     if (finishCross >= CarSettings.Instance.targetFinishCrossTimes)
    //     {
    //         var fitnessMult = CarSettings.Instance.finishFitnessMultiplier;

    //         fitnessMult *= Mathf.Lerp(
    //             1,
    //             CarSettings.Instance.fitnessMultiplierForBeingFirst,
    //             1f - (float)(populationCar.theRealFinish.crossedBy.Count) / populationCar.Config.genomeCount
    //         );
    //         populationCar.theRealFinish.crossedBy.Add(this);

    //         AddFitness(GenomeProperty.Fitness * fitnessMult);
    //         Die();
    //         SaveGenome();
    //     }
    // }

    // private void ProcessFitnessPointCross(NeuroGen.Checkpoint checkpoint)
    // {
    //     checkpointPassed.Add(checkpoint);
    //     if (IsDrivingForward())
    //         AddFitness(checkpoint.fitnessWhenTouched);
    // }

    // private void ProcessTeleportCross(NeuroGen.Checkpoint checkpoint)
    // {
    //     if (!IsDrivingForward())
    //     {
    //         Die();
    //         return;
    //     }

    //     checkpoint.Teleport(transform);
    // }

    private void SaveGenome()
    {
        var dir = GenomeSaver.DefaultSaveDir + "SavedGenomes\\";
        if (!System.IO.Directory.Exists(dir))
            System.IO.Directory.CreateDirectory(dir);

        var filePath = GenomeSaver.GenerateSaveFilePath(
            dir, GenomeProperty.Fitness, populationCar.Popl.Generation
        );

        GenomeSaver.SaveGenome(
            GenomeProperty,
            filePath
        );
    }
    #endregion
}
