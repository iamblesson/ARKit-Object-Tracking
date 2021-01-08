using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class TrackedObjectInfoManager : MonoBehaviour
{
    [SerializeField]
    private Text imageTrackedText;

    [SerializeField]
    private GameObject[] arObjectsToPlace;

    [SerializeField]
    private Vector3 scaleFactor = new Vector3(0.1f,0.1f,0.1f);

    private ARTrackedObjectManager m_TrackedObjectManager;

    private Dictionary<string, GameObject> arObjects = new Dictionary<string, GameObject>();

    void Awake()
    {
        m_TrackedObjectManager = GetComponent<ARTrackedObjectManager>();
        
        // setup all game objects in dictionary
        foreach(GameObject arObject in arObjectsToPlace)
        {
            GameObject newARObject = Instantiate(arObject, Vector3.zero, Quaternion.identity);
            newARObject.name = arObject.name;
            arObjects.Add(arObject.name, newARObject);
            newARObject.SetActive(false);
        }
    }

    void OnEnable()
    {
        m_TrackedObjectManager.trackedObjectsChanged += OnTrackedObjectsChanged;
    }

    void OnDisable()
    {
        m_TrackedObjectManager.trackedObjectsChanged -= OnTrackedObjectsChanged;
    }

    void OnTrackedObjectsChanged(ARTrackedObjectsChangedEventArgs eventArgs)
    {
        foreach (ARTrackedObject trackedObject in eventArgs.added)
        {
            imageTrackedText.text = trackedObject.referenceObject.name + " added";
            UpdateARImage(trackedObject);
        }

        foreach (ARTrackedObject trackedObject in eventArgs.updated)
        {
            imageTrackedText.text = trackedObject.referenceObject.name + " updated";
            UpdateARImage(trackedObject);
        }

        foreach (ARTrackedObject trackedObject in eventArgs.removed)
        {
            imageTrackedText.text = trackedObject.referenceObject.name + " removed";
            arObjects[trackedObject.name].SetActive(false);
        }
    }

    private void UpdateARImage(ARTrackedObject trackedObject)
    {
        // Display the name of the tracked image in the canvas
        //imageTrackedText.text = trackedImage.referenceImage.name;

        // Assign and Place Game Object
        AssignGameObject(trackedObject.referenceObject.name, trackedObject.transform.position);

        Debug.Log($"trackedImage.referenceImage.name: {trackedObject.referenceObject.name}");
    }

    void AssignGameObject(string name, Vector3 newPosition)
    {
        if(arObjectsToPlace != null)
        {
            GameObject goARObject = arObjects[name];
            goARObject.SetActive(true);
            goARObject.transform.position = newPosition;
            goARObject.transform.localScale = scaleFactor;
            foreach(GameObject go in arObjects.Values)
            {
                Debug.Log($"Go in arObjects.Values: {go.name}");
                if(go.name != name)
                {
                    go.SetActive(false);
                }
            } 
        }
    }
}
