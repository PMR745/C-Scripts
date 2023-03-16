using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;


public class placeObject : MonoBehaviour
{
    [SerializeField]
    ARRaycastManager raycastManager;
    List<ARRaycastHit> hits = new List<ARRaycastHit>();
    [SerializeField]
    GameObject spawnObj;

    Camera Arcam;
    GameObject SpawnedObj;
    // Start is called before the first frame update
    void Start()
    {
        SpawnedObj = null;
        Arcam = GameObject.Find("AR Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount == 0)
            return;
        RaycastHit hit;
        Ray ray = Arcam.ScreenPointToRay(Input.GetTouch(0).position);
        if(raycastManager.Raycast(Input.GetTouch(0).position, hits))
        {
            if(Input.GetTouch(0).phase == TouchPhase.Began && SpawnedObj == null)
            {
                if(Physics.Raycast(ray, out hit))
                {
                    if(hit.collider.gameObject.tag == "Spawnable")
                    {
                        spawnObj = hit.collider.gameObject;
                    }
                    else 
                    {
                        SpawnPrefab(hits[0].pose.position);
                    }
                }
            }
            else if(Input.GetTouch(0).phase == TouchPhase.Moved && SpawnedObj != null)
            {
                SpawnedObj.transform.position = hits[0].pose.position;
            }
            if(Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                SpawnedObj = null;
            }
        }
    }

    void SpawnPrefab(Vector3 position)
    {
        SpawnedObj = Instantiate(spawnObj,position,Quaternion.identity);
    }
}
