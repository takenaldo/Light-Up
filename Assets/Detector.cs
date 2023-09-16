using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    // Start is called before the first frame update

    public List<GameObject> connects;
    public string type = "cable";

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateConnects()
    {
        GameObject[] allDetectors = GameObject.FindGameObjectsWithTag("detector");
        connects.Clear();
        foreach (GameObject item in allDetectors)
        {
            Detector d = item.GetComponent<Detector>();
            d.connects.Remove(gameObject);
        }

        GameObject []detectors = GameObject.FindGameObjectsWithTag("detector");
        foreach (GameObject otherDetector in detectors)
        {
            if (transform.parent.gameObject == otherDetector.transform.parent.gameObject)
                continue;

            if (!gameObject.activeInHierarchy || !otherDetector.activeInHierarchy)
                continue;

            float distance = Vector2.Distance(transform.position, otherDetector.transform.position);

            if (distance < GameManager.instance.margin)
            {
                Debug.Log(gameObject.name + " ---AND--- " +otherDetector.name);
                connects.Add(otherDetector);
                otherDetector.GetComponent<Detector>().connects.Add(gameObject);
            }
        }
        Debug.Log("------------------------------------------------------------");

    }
}
