using UnityEngine;
using SharpNeat.Phenomes;

namespace NeuroGen
{
    public class Car : UnitController
    {
        public GameObject leftSensor;
        public GameObject middleSensor;
        public GameObject rightSensor;
        public GameObject middleLeftSensor;
        public GameObject middleRightSensor;
        private bool showSensors = true;
        private LineRenderer leftSensorLineRenderer;
        private LineRenderer middleSensorLineRenderer;
        private LineRenderer rightSensorLineRenderer;
        private LineRenderer middleLeftSensorLineRenderer;
        private LineRenderer middleRightSensorLineRenderer;
        private float[] sensors = { 0, 0, 0, 0, 0 };
        public WheelCollider[] wheelColliders;
        public Transform[] wheelTransforms;
        public float maxSteeringAngle = 30f;
        public float motorForce = 1100f;
        public float brakeForce = 3000f;
        public bool humanControlled = false;
        IBlackBox box;
        private Vector3 lastPosition;
        private float distanceTravelled;
        private float idleTime = 0;
        public SpriteRenderer speciesColor;
        public int Id { get; protected set; }

        public override float Fitness { get { return distanceTravelled; } }

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
                leftSensorLineRenderer.SetPosition(1, new Vector3(-5f, 0, 3f));

                middleSensorLineRenderer = middleSensor.GetComponent<LineRenderer>();
                middleSensorLineRenderer.transform.position += new Vector3(0, 0, extents.z);
                middleSensorLineRenderer.useWorldSpace = false;
                middleSensorLineRenderer.startColor = Color.green;
                middleSensorLineRenderer.endColor = Color.green;
                middleSensorLineRenderer.transform.position += new Vector3(0, 0.5f, 0);
                middleSensorLineRenderer.SetPosition(0, Vector3.zero); middleSensorLineRenderer.SetPosition(1, new Vector3(0, 0, 5.4f));

                rightSensorLineRenderer = rightSensor.GetComponent<LineRenderer>();
                rightSensorLineRenderer.transform.position += new Vector3(0, 0, extents.z);
                rightSensorLineRenderer.useWorldSpace = false;
                rightSensorLineRenderer.startColor = Color.green;
                rightSensorLineRenderer.endColor = Color.green;
                rightSensorLineRenderer.SetPosition(0, Vector3.zero);
                rightSensorLineRenderer.SetPosition(1, new Vector3(5f, 0, 3f));
                rightSensorLineRenderer.transform.position += new Vector3(0, 0.5f, 0);

                middleLeftSensorLineRenderer = middleLeftSensor.GetComponent<LineRenderer>();
                middleLeftSensorLineRenderer.transform.position += new Vector3(-extents.x, 0, 0);
                middleLeftSensorLineRenderer.useWorldSpace = false;
                middleLeftSensorLineRenderer.startColor = Color.green;
                middleLeftSensorLineRenderer.endColor = Color.green;
                middleLeftSensorLineRenderer.SetPosition(0, Vector3.zero);
                middleLeftSensorLineRenderer.SetPosition(1, new Vector3(-5f, 0, 0));
                middleLeftSensorLineRenderer.transform.position += new Vector3(0, 0.5f, 0);

                middleRightSensorLineRenderer = middleRightSensor.GetComponent<LineRenderer>();
                middleRightSensorLineRenderer.transform.position += new Vector3(extents.x, 0, 0);
                middleRightSensorLineRenderer.useWorldSpace = false;
                middleRightSensorLineRenderer.startColor = Color.green;
                middleRightSensorLineRenderer.endColor = Color.green;
                middleRightSensorLineRenderer.SetPosition(0, Vector3.zero);
                middleRightSensorLineRenderer.SetPosition(1, new Vector3(5f, 0, 0));
                middleRightSensorLineRenderer.transform.position += new Vector3(0, 0.5f, 0);
            }
        }

        void FixedUpdate()
        {
            if (humanControlled || isRunning)
            {
                // Sensors
                for (int i = 0; i < 5; i++)
                    sensors[i] = 0;

                var frontRaySPosition = middleSensorLineRenderer.transform.TransformPoint(middleSensorLineRenderer.GetPosition(0));
                var middleLeftRaySPosition = middleLeftSensorLineRenderer.transform.TransformPoint(middleLeftSensorLineRenderer.GetPosition(0));
                var middleRightRaySPosition = middleRightSensorLineRenderer.transform.TransformPoint(middleRightSensorLineRenderer.GetPosition(0));

                var leftSensorDirection = leftSensorLineRenderer.transform.TransformDirection(leftSensorLineRenderer.GetPosition(1));
                var leftSensorMagnitude = leftSensorDirection.magnitude;

                var middleSensorDirection = middleSensorLineRenderer.transform.TransformDirection(middleSensorLineRenderer.GetPosition(1));
                var middleSensorMagnitude = middleSensorDirection.magnitude;

                var rightSensorDirection = rightSensorLineRenderer.transform.TransformDirection(rightSensorLineRenderer.GetPosition(1));
                var rightSensorMagnitude = rightSensorDirection.magnitude;

                var middleLeftSensorDirection = middleLeftSensorLineRenderer.transform.TransformDirection(middleLeftSensorLineRenderer.GetPosition(1));
                var middleLeftSensorMagnitude = middleLeftSensorDirection.magnitude;

                var middleRightSensorDirection = middleRightSensorLineRenderer.transform.TransformDirection(middleRightSensorLineRenderer.GetPosition(1));
                var middleRightSensorMagnitude = middleRightSensorDirection.magnitude;

                RaycastHit hit;
                if (Physics.Raycast(frontRaySPosition, leftSensorDirection.normalized, out hit, leftSensorMagnitude, Physics.DefaultRaycastLayers))
                {
                    leftSensorLineRenderer.startColor = Color.red;
                    leftSensorLineRenderer.endColor = Color.red;
                    sensors[0] = (leftSensorMagnitude - hit.distance) / leftSensorMagnitude;
                }
                else
                {
                    leftSensorLineRenderer.startColor = Color.green;
                    leftSensorLineRenderer.endColor = Color.green;
                }

                if (Physics.Raycast(frontRaySPosition, middleSensorDirection.normalized, out hit, middleSensorMagnitude, Physics.DefaultRaycastLayers))
                {
                    middleSensorLineRenderer.startColor = Color.red;
                    middleSensorLineRenderer.endColor = Color.red;
                    sensors[1] = (middleSensorMagnitude - hit.distance) / middleSensorMagnitude;
                }
                else
                {
                    middleSensorLineRenderer.startColor = Color.green;
                    middleSensorLineRenderer.endColor = Color.green;
                }

                if (Physics.Raycast(frontRaySPosition, rightSensorDirection.normalized, out hit, rightSensorMagnitude, Physics.DefaultRaycastLayers))
                {
                    rightSensorLineRenderer.startColor = Color.red;
                    rightSensorLineRenderer.endColor = Color.red;
                    sensors[2] = (rightSensorMagnitude - hit.distance) / rightSensorMagnitude;
                }
                else
                {
                    rightSensorLineRenderer.startColor = Color.green;
                    rightSensorLineRenderer.endColor = Color.green;
                }

                if (Physics.Raycast(middleLeftRaySPosition, middleLeftSensorDirection.normalized, out hit, middleLeftSensorMagnitude, Physics.DefaultRaycastLayers))
                {
                    middleLeftSensorLineRenderer.startColor = Color.red;
                    middleLeftSensorLineRenderer.endColor = Color.red;
                    sensors[3] = (middleLeftSensorMagnitude - hit.distance) / middleLeftSensorMagnitude;
                }
                else
                {
                    middleLeftSensorLineRenderer.startColor = Color.green;
                    middleLeftSensorLineRenderer.endColor = Color.green;
                }

                if (Physics.Raycast(middleRightRaySPosition, middleRightSensorDirection.normalized, out hit, middleRightSensorMagnitude, Physics.DefaultRaycastLayers))
                {
                    middleRightSensorLineRenderer.startColor = Color.red;
                    middleRightSensorLineRenderer.endColor = Color.red;
                    sensors[4] = (middleRightSensorMagnitude - hit.distance) / middleRightSensorMagnitude;
                }
                else
                {
                    middleRightSensorLineRenderer.startColor = Color.green;
                    middleRightSensorLineRenderer.endColor = Color.green;
                }

                // Controls
                float horizontalControl = 0, verticalControl = 0;
                bool isBraking = false;
                var velocity = GetComponent<Rigidbody>().velocity.sqrMagnitude;

                if (humanControlled)
                {
                    horizontalControl = Input.GetAxis("Horizontal");
                    verticalControl = Input.GetAxis("Vertical");
                    isBraking = Input.GetKey(KeyCode.Space);
                }
                else if (isRunning)
                {
                    ISignalArray input = box.InputSignalArray;

                    for (int i = 0; i < 5; i++)
                        input[i] = sensors[i];
                    // input[3] = velocity;

                    box.Activate();

                    ISignalArray output = box.OutputSignalArray;

                    // Split the values into 3 categories
                    if (output[0] <= 0.33)
                        horizontalControl = 0;
                    else if (output[0] > 0.33 && output[0] <= .66)
                        horizontalControl = 1;
                    else
                        horizontalControl = -1;

                    // Split it up into 2 categories
                    verticalControl = output[1] > 0.5 ? 1 : 0;

                    // isBraking = output[2] > 0.5 ? true : false;

                    // Accumulate distance
                    distanceTravelled += Vector3.Distance(transform.position, lastPosition);
                    lastPosition = transform.position;

                    // Idle timer
                    if (velocity <= .5)
                    {
                        idleTime += Time.fixedDeltaTime;
                        if (idleTime > 5)
                            Stop();
                    }
                    else
                    {
                        idleTime = 0;
                    }
                }

                // Physics
                for (int i = 0; i < 2; i++)
                {
                    // Motor
                    wheelColliders[i].motorTorque = velocity <= 20 ? verticalControl * motorForce : 0;

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
        }

        void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.name == "Stopper" || collider.gameObject.name == "Track")
                Stop();
        }

        override public void Activate(IBlackBox box)
        {
            lastPosition = transform.position;
            distanceTravelled = 0;

            this.box = box;
            isRunning = true;
            gameObject.SetActive(true);
        }

        public void Reinit(Transform transform)
        {
            this.transform.position = transform.position;
            this.transform.rotation = transform.rotation;
        }

        override public void Stop()
        {
            isRunning = false;
            gameObject.SetActive(false);
        }
    }
}