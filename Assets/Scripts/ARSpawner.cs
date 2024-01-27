using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
public class ARSpawner : MonoBehaviour
{
    public GameObject[] spawningObjects;
    [SerializeField]
    ARRaycastManager arRaycastManager;
    List<ARRaycastHit> arHits = new List<ARRaycastHit>();
    Camera arCam;
    GameObject spawnedObject, spawnToObject;

    // Start is called before the first frame update
    void Start()
    {
        spawnedObject = null;
        arCam = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 0)
            return;
        Touch touch = Input.GetTouch(0);
        if (arRaycastManager.Raycast(touch.position, arHits))
        {
            if (touch.phase == TouchPhase.Began)// && spawnedObject == null)
            {
                Ray ray = arCam.ScreenPointToRay(touch.position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.gameObject.tag == "Spawnable")
                    {
                        spawnedObject = hit.collider.gameObject;
                    }
                    else
                    {
                        SpawnObject(arHits[0].pose.position);
                    }
                }
            }
            else if (touch.phase == TouchPhase.Moved && spawnedObject != null)
            {
                spawnedObject.transform.position = arHits[0].pose.position;
            }
            if (touch.phase == TouchPhase.Ended)
                spawnedObject = null;

        }
    }

    private void SpawnObject(Vector3 spawnPosition)
    {
        spawnedObject =Instantiate(spawnToObject, spawnPosition, Quaternion.identity);
    }

    public void OnClickButton(int num)
    {
        switch (num)
        {
            case 0:
                spawnToObject = spawningObjects[0];
                break;
            case 1:
                spawnToObject = spawningObjects[1];
                break;
            case 2:
                spawnToObject = spawningObjects[2];
                break;
        }
    }
}