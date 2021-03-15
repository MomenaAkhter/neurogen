#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.

using UnityEngine;
using NeuroGen;
using System.Collections.Generic;

namespace NEAT
{
    /// <summary>
    /// A 'connection' class between the Popultion logic
    /// and the actual problem.
    /// </summary>
    public abstract class PopulationProxy : MonoBehaviour
    {
        #region Fields
        [SerializeField] private NEATConfig config = null;
        [SerializeField] private Transform genomeProxyStorage = null;
        #endregion

        #region Properties
        public NEATConfig Config { get { return config; } }
        public Population Popl { get; protected set; }
        public GenomeProxy[] GenomeProxies { get; protected set; }
        #endregion

        #region Monobehaviour methods
        protected virtual void Awake()
        {
            config.genomeCount = Main.Instance.genomeCount;
            config.inputCount = Main.Instance.inputCount;
            config.outputCount = Main.Instance.outputCount;

            InitPopl();
        }

        protected virtual void Start()
        {
            InstantiateAllGenomes();
        }
        #endregion

        #region Public methods
        public virtual void InitPopl()
        {
            // Get genomes of best fit models
            List<Model> models = Database.GetBestModels(5, Main.Instance.selectedExtensionId);
            List<Genome> genomes = new List<Genome>();
            foreach (var model in models)
            {
                // Deserialize JSON to get PackedGenome
                var packedGenome = JsonUtility.FromJson<PackedGenome>(model.content);
                genomes.Add(new Genome(config, packedGenome));
            }

            // Fill in the empty space in the genomes list
            int emptySlots = Config.genomeCount - genomes.Count;
            for (int i = 0; i < emptySlots; i++)
                genomes.Add(new Genome(Config.inputCount, Config.outputCount, Config.weightInitRandomValue));

            Popl = new Population(
                genomeCount: Config.genomeCount,
                inCount: Config.inputCount,
                outCount: Config.outputCount,
                config: config,
                genomes
            );
        }

        public virtual void Evolve()
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
            Popl.Evolve();
        }
        #endregion

        #region Protected methods
        // Instantiates all genomes from the current population.
        protected void InstantiateAllGenomes()
        {
            GenomeProxies = new GenomeProxy[Popl.Genomes.Count];

            for (int i = 0; i < Popl.Genomes.Count; i++)
            {
                GameObject obj;

                if (genomeProxyStorage == null)
                    obj = Instantiate(NeuroGen.Main.Instance.carPrefab).gameObject;
                else
                    obj = Instantiate(NeuroGen.Main.Instance.carPrefab, genomeProxyStorage).gameObject;

                var proxyComponent = obj.AddComponent<GenomeCar>();
                proxyComponent.Init(i, Popl);
                GenomeProxies[i] = proxyComponent;

                InitGenomeProxyObj(obj);
            }
        }

        // Initialize the 'physical' atributes of the instantiated genomeProxy.
        protected virtual void InitGenomeProxyObj(GameObject genomeProxy)
        {
            genomeProxy.transform.position = Vector3.zero;
        }
        #endregion

        #region Private methods
        #endregion
    }
}
