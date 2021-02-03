using UnityEngine;
using SharpNeat.Phenomes;

namespace NeuroGen
{
    public class Car : UnitController
    {
        public GameObject leftSensor;
        public GameObject middleSensor;
        public GameObject rightSensor;
        private bool showSensors = true;
        private bool dead = false;
        private LineRenderer leftSensorLineRenderer;
        private LineRenderer middleSensorLineRenderer;
        private LineRenderer rightSensorLineRenderer;
        private Vector3 sensors = Vector3.zero;
        public WheelCollider[] wheelColliders;
        public Transform[] wheelTransforms;
        public float maxSteeringAngle = 30f;
        public float motorForce = 1100f;
        public float brakeForce = 3000f;
        public bool humanControlled = false;
        bool IsRunning;
        IBlackBox box;
        private Vector3 lastPosition;
        private float distanceTravelled;

        // Start is called before the first frame update
        void Start()
        {
            lastPosition = transform.position;
            distanceTravelled = 0;

            var extents = transform.GetChild(0).gameObject.GetComponent<Renderer>().bounds.extents;

            if (showSensors)
            {
                leftSensorLineRenderer = leftSensor.GetComponent<LineRenderer>();
                leftSensorLineRenderer.transform.position += new Vector3(0, 0, extents.z);
                leftSensorLineRenderer.useWorldSpace = false;
                leftSensorLineRenderer.startColor = Color.green;
                leftSensorLineRenderer.endColor = Color.green;
                leftSensorLineRenderer.transform.position += new Vector3(0, 0.5f, 0);
                leftSensorLineRenderer.SetPosition(0, Vector3.zero);
                leftSensorLineRenderer.SetPosition(1, new Vector3(-2.7f, 0, 3f));

                middleSensorLineRenderer = middleSensor.GetComponent<LineRenderer>();
                middleSensorLineRenderer.transform.position += new Vector3(0, 0, extents.z);
                middleSensorLineRenderer.useWorldSpace = false;
                middleSensorLineRenderer.startColor = Color.green;
                middleSensorLineRenderer.endColor = Color.green;
                middleSensorLineRenderer.transform.position += new Vector3(0, 0.5f, 0);
                middleSensorLineRenderer.SetPosition(0, Vector3.zero); middleSensorLineRenderer.SetPosition(1, new Vector3(0, 0, 3.5f));

                rightSensorLineRenderer = rightSensor.GetComponent<LineRenderer>();
                rightSensorLineRenderer.transform.position += new Vector3(0, 0, extents.z);
                rightSensorLineRenderer.useWorldSpace = false;
                rightSensorLineRenderer.startColor = Color.green;
                rightSensorLineRenderer.endColor = Color.green;
                rightSensorLineRenderer.SetPosition(0, Vector3.zero);
                rightSensorLineRenderer.SetPosition(1, new Vector3(2.7f, 0, 3f));
                rightSensorLineRenderer.transform.position += new Vector3(0, 0.5f, 0);
            }
        }

        void FixedUpdate()
        {
            if (showSensors)
            {
                var raySPosition = middleSensorLineRenderer.transform.TransformPoint(middleSensorLineRenderer.GetPosition(0));

                var leftSensorDirection = leftSensorLineRenderer.transform.TransformDirection(leftSensorLineRenderer.GetPosition(1));
                var leftSensorMagnitude = leftSensorDirection.magnitude;

                var middleSensorDirection = middleSensorLineRenderer.transform.TransformDirection(middleSensorLineRenderer.GetPosition(1));
                var middleSensorMagnitude = middleSensorDirection.magnitude;

                var rightSensorDirection = rightSensorLineRenderer.transform.TransformDirection(rightSensorLineRenderer.GetPosition(1));
                var rightSensorMagnitude = rightSensorDirection.magnitude;


                RaycastHit hit;
                if (Physics.Raycast(raySPosition, leftSensorDirection.normalized, out hit, leftSensorMagnitude, Physics.DefaultRaycastLayers))
                {
                    leftSensorLineRenderer.startColor = Color.red;
                    leftSensorLineRenderer.endColor = Color.red;
                    sensors[0] = leftSensorMagnitude - hit.distance;
                }
                else
                {
                    leftSensorLineRenderer.startColor = Color.green;
                    leftSensorLineRenderer.endColor = Color.green;
                    sensors[0] = 0;
                }

                if (Physics.Raycast(raySPosition, middleSensorDirection.normalized, out hit, middleSensorMagnitude, Physics.DefaultRaycastLayers))
                {
                    middleSensorLineRenderer.startColor = Color.red;
                    middleSensorLineRenderer.endColor = Color.red;
                    sensors[1] = middleSensorMagnitude - hit.distance;
                }
                else
                {
                    middleSensorLineRenderer.startColor = Color.green;
                    middleSensorLineRenderer.endColor = Color.green;
                    sensors[1] = 0;
                }

                if (Physics.Raycast(raySPosition, rightSensorDirection.normalized, out hit, rightSensorMagnitude, Physics.DefaultRaycastLayers))
                {
                    rightSensorLineRenderer.startColor = Color.red;
                    rightSensorLineRenderer.endColor = Color.red;
                    sensors[2] = rightSensorMagnitude - hit.distance;
                }
                else
                {
                    rightSensorLineRenderer.startColor = Color.green;
                    rightSensorLineRenderer.endColor = Color.green;
                    sensors[2] = 0;
                }
            }

            float horizontalControl = 0, verticalControl = 0;
            bool isBraking = false;

            if (humanControlled)
            {
                horizontalControl = Input.GetAxis("Horizontal");
                verticalControl = Input.GetAxis("Vertical");
                isBraking = Input.GetKey(KeyCode.Space);
            }
            else if (IsRunning)
            {
                ISignalArray input = box.InputSignalArray;

                for (int i = 0; i < 3; i++)
                    input[i] = sensors[i];

                box.Activate();

                ISignalArray output = box.OutputSignalArray;

                if (output[0] <= 0.33)
                    horizontalControl = 0;
                else if (output[0] > 0.33 && output[0] <= .66)
                    horizontalControl = 1;
                else
                    horizontalControl = -1;

                if (output[1] <= 0.33)
                    verticalControl = 0;
                else if (output[1] > 0.33 && output[0] <= .66)
                    verticalControl = 1;
                else
                    verticalControl = -1;

                isBraking = output[2] > 0.5 ? true : false;

                // Accumulate distance
                distanceTravelled += Vector3.Distance(transform.position, lastPosition);
                lastPosition = transform.position;
            }

            for (int i = 0; i < 2; i++)
            {
                // Motor
                wheelColliders[i].motorTorque = verticalControl * motorForce;

                // Steering
                wheelColliders[i].steerAngle = maxSteeringAngle * horizontalControl;
            }

            // Braking
            foreach (var wheelCollider in wheelColliders)
                wheelCollider.brakeTorque = isBraking ? brakeForce : 0f;

            // Wheel Poses
            for (int i = 0; i < 4; i++)
            {
                Vector3 position;
                Quaternion rotation;

                wheelColliders[i].GetWorldPose(out position, out rotation);
                wheelTransforms[i].rotation = rotation;
                wheelTransforms[i].position = position;
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.name == "Stopper" || collision.gameObject.name == "Track")
            {
                Stop();
                dead = true;
            }
        }

        void Reset()
        {
            transform.position = Main.startPosition;
            transform.rotation = Quaternion.identity;
            dead = false;
        }

        override public void Activate(IBlackBox box)
        {
            lastPosition = transform.position;
            distanceTravelled = 0;

            this.box = box;
            IsRunning = true;
        }

        override public void Stop()
        {
            IsRunning = false;
        }

        public override float GetFitness()
        {
            return distanceTravelled;
        }
    }
}