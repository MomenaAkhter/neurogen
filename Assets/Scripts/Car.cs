using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public GameObject leftSensor;
    public GameObject middleSensor;
    public GameObject rightSensor;

    private LineRenderer leftSensorLineRenderer;
    private LineRenderer middleSensorLineRenderer;
    private LineRenderer rightSensorLineRenderer;
    private Vector3 extents;

    // Start is called before the first frame update
    void Start()
    {
        extents = transform.GetChild(0).gameObject.GetComponent<Renderer>().bounds.extents;

        leftSensorLineRenderer = leftSensor.GetComponent<LineRenderer>();
        leftSensorLineRenderer.transform.position += new Vector3(0, 0, extents.z);
        leftSensorLineRenderer.useWorldSpace = false;
        leftSensorLineRenderer.startColor = Color.green;
        leftSensorLineRenderer.endColor = Color.green;
        leftSensorLineRenderer.SetPosition(0, Vector3.zero);
        leftSensorLineRenderer.transform.position += new Vector3(0, 0.5f, 0);

        middleSensorLineRenderer = middleSensor.GetComponent<LineRenderer>();
        middleSensorLineRenderer.transform.position += new Vector3(0, 0, extents.z);
        middleSensorLineRenderer.useWorldSpace = false;
        middleSensorLineRenderer.startColor = Color.green;
        middleSensorLineRenderer.endColor = Color.green;
        middleSensorLineRenderer.SetPosition(0, Vector3.zero);
        middleSensorLineRenderer.transform.position += new Vector3(0, 0.5f, 0);

        rightSensorLineRenderer = rightSensor.GetComponent<LineRenderer>();
        rightSensorLineRenderer.transform.position += new Vector3(0, 0, extents.z);
        rightSensorLineRenderer.useWorldSpace = false;
        rightSensorLineRenderer.startColor = Color.green;
        rightSensorLineRenderer.endColor = Color.green;
        rightSensorLineRenderer.SetPosition(0, Vector3.zero);
        rightSensorLineRenderer.transform.position += new Vector3(0, 0.5f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        leftSensorLineRenderer.SetPosition(1, new Vector3(-2.7f, 0, 3f));
        middleSensorLineRenderer.SetPosition(1, new Vector3(0, 0, 3.5f));
        rightSensorLineRenderer.SetPosition(1, new Vector3(2.7f, 0, 3f));
    }

    void FixedUpdate()
    {
        var raySPosition = middleSensorLineRenderer.transform.TransformPoint(middleSensorLineRenderer.GetPosition(0));

        var leftSensorDirection = leftSensorLineRenderer.transform.TransformDirection(leftSensorLineRenderer.GetPosition(1));
        var leftSensorMagnitude = leftSensorDirection.magnitude;

        var middleSensorDirection = middleSensorLineRenderer.transform.TransformDirection(middleSensorLineRenderer.GetPosition(1));
        var middleSensorMagnitude = middleSensorDirection.magnitude;

        var rightSensorDirection = rightSensorLineRenderer.transform.TransformDirection(rightSensorLineRenderer.GetPosition(1));
        var rightSensorMagnitude = rightSensorDirection.magnitude;

        {
            RaycastHit hit;
            if (Physics.Raycast(raySPosition, leftSensorDirection.normalized, out hit, leftSensorMagnitude, Physics.DefaultRaycastLayers))
            {
                leftSensorLineRenderer.startColor = Color.red;
                leftSensorLineRenderer.endColor = Color.red;
            }
            else
            {
                leftSensorLineRenderer.startColor = Color.green;
                leftSensorLineRenderer.endColor = Color.green;
            }
        }
        {
            RaycastHit hit;
            if (Physics.Raycast(raySPosition, middleSensorDirection.normalized, out hit, middleSensorMagnitude, Physics.DefaultRaycastLayers))
            {
                middleSensorLineRenderer.startColor = Color.red;
                middleSensorLineRenderer.endColor = Color.red;
            }
            else
            {
                middleSensorLineRenderer.startColor = Color.green;
                middleSensorLineRenderer.endColor = Color.green;
            }
        }
        {
            RaycastHit hit;
            if (Physics.Raycast(raySPosition, rightSensorDirection.normalized, out hit, rightSensorMagnitude, Physics.DefaultRaycastLayers))
            {
                rightSensorLineRenderer.startColor = Color.red;
                rightSensorLineRenderer.endColor = Color.red;
            }
            else
            {
                rightSensorLineRenderer.startColor = Color.green;
                rightSensorLineRenderer.endColor = Color.green;
            }
        }
    }
}
