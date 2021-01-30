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

        middleSensorLineRenderer = middleSensor.GetComponent<LineRenderer>();
        middleSensorLineRenderer.transform.position += new Vector3(0, 0, extents.z);
        middleSensorLineRenderer.useWorldSpace = false;
        middleSensorLineRenderer.startColor = Color.green;
        middleSensorLineRenderer.endColor = Color.green;

        rightSensorLineRenderer = rightSensor.GetComponent<LineRenderer>();
        rightSensorLineRenderer.transform.position += new Vector3(0, 0, extents.z);
        rightSensorLineRenderer.useWorldSpace = false;
        rightSensorLineRenderer.startColor = Color.green;
        rightSensorLineRenderer.endColor = Color.green;
    }

    // Update is called once per frame
    void Update()
    {
        leftSensorLineRenderer.SetPosition(0, new Vector3(0, 0.5f, 0));
        leftSensorLineRenderer.SetPosition(1, new Vector3(0, 0.5f, 3.5f));
        leftSensorLineRenderer.transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 45, 0);

        middleSensorLineRenderer.SetPosition(0, new Vector3(0, 0.5f, 0));
        middleSensorLineRenderer.SetPosition(1, new Vector3(0, 0.5f, 3.5f));

        rightSensorLineRenderer.SetPosition(0, new Vector3(0, 0.5f, 0));
        rightSensorLineRenderer.SetPosition(1, new Vector3(0, 0.5f, 3.5f));
        rightSensorLineRenderer.transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 45, 0);
    }

    void FixedUpdate()
    {
        var raySPosition = middleSensorLineRenderer.transform.TransformPoint(middleSensorLineRenderer.GetPosition(0));

        var leftSensorPointDirection = leftSensorLineRenderer.transform.TransformDirection(leftSensorLineRenderer.GetPosition(1) - new Vector3(0, 0.5f, 0)).normalized;

        var middleSensorPointDirection = middleSensorLineRenderer.transform.TransformDirection(middleSensorLineRenderer.GetPosition(1) - new Vector3(0, 0.5f, 0)).normalized;

        var rightSensorPointDirection = rightSensorLineRenderer.transform.TransformDirection(rightSensorLineRenderer.GetPosition(1) - new Vector3(0, 0.5f, 0)).normalized;

        {
            RaycastHit hit;
            if (Physics.Raycast(raySPosition, leftSensorPointDirection, out hit, 3.5f, Physics.DefaultRaycastLayers))
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
            if (Physics.Raycast(raySPosition, middleSensorPointDirection, out hit, 3.5f, Physics.DefaultRaycastLayers))
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
            if (Physics.Raycast(raySPosition, rightSensorPointDirection, out hit, 3.5f, Physics.DefaultRaycastLayers))
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
