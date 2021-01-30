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
        // transform.Translate(Vector3.forward * Time.deltaTime);

        leftSensorLineRenderer.SetPosition(0, new Vector3(0, 0.5f, 0));
        leftSensorLineRenderer.SetPosition(1, new Vector3(0, 0.5f, 3.5f));
        leftSensorLineRenderer.transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y - 45, 0);

        middleSensorLineRenderer.SetPosition(0, new Vector3(0, 0.5f, 0));
        middleSensorLineRenderer.SetPosition(1, new Vector3(0, 0.5f, 3.5f));

        rightSensorLineRenderer.SetPosition(0, new Vector3(0, 0.5f, 0));
        rightSensorLineRenderer.SetPosition(1, new Vector3(0, 0.5f, 3.5f));
        rightSensorLineRenderer.transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 45, 0);

        // Debug.Log(leftSensorLineRenderer.transform.TransformPoint( leftSensorLineRenderer.GetPosition(1)));

        // Debug.Log(leftSensorLineRenderer.GetPosition(0));
        // Debug.Log(new Vector3(0, 0.5f, 3.5f).);
        // Debug.Log(leftSensorLineRenderer.localToWorldMatrix);
    }

    void FixedUpdate()
    {
        var raySPosition = middleSensorLineRenderer.transform.TransformPoint(middleSensorLineRenderer.GetPosition(0));

        var leftSensorPointDirection = leftSensorLineRenderer.transform.TransformDirection(leftSensorLineRenderer.GetPosition(1) - new Vector3(0, 0.5f, 0)).normalized;

        var middleSensorPointDirection = middleSensorLineRenderer.transform.TransformDirection(middleSensorLineRenderer.GetPosition(1) - new Vector3(0, 0.5f, 0)).normalized;

        var rightSensorPointDirection = rightSensorLineRenderer.transform.TransformDirection(rightSensorLineRenderer.GetPosition(1) - new Vector3(0, 0.5f, 0)).normalized;

        {
            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(raySPosition, leftSensorPointDirection, out hit, 3.5f, Physics.DefaultRaycastLayers))
            {
                Debug.DrawRay(raySPosition, leftSensorPointDirection * hit.distance, Color.yellow);
            }
            else
            {
                Debug.DrawRay(raySPosition, leftSensorPointDirection * 3.5f, Color.white);
            }
        }
        {
            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(raySPosition, middleSensorPointDirection, out hit, 3.5f, Physics.DefaultRaycastLayers))
            {
                Debug.DrawRay(raySPosition, middleSensorPointDirection * hit.distance, Color.yellow);
            }
            else
            {
                Debug.DrawRay(raySPosition, middleSensorPointDirection * 3.5f, Color.white);
            }
        }
        {
            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(raySPosition, rightSensorPointDirection, out hit, 3.5f, Physics.DefaultRaycastLayers))
            {
                Debug.DrawRay(raySPosition, rightSensorPointDirection * hit.distance, Color.yellow);
            }
            else
            {
                Debug.DrawRay(raySPosition, rightSensorPointDirection * 3.5f, Color.white);
            }
        }
    }
}
