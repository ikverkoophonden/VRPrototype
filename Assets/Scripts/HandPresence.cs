using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandPresence : MonoBehaviour
{
    public bool showController;
    public List<GameObject> controllerPrefabs;
    public GameObject handModelPrefab;
    public InputDeviceCharacteristics inputDeviceCharacteristics;

    private InputDevice targetDevice;
    private GameObject spawnedController;
    private GameObject spawnedModel;
    private Animator handAnimator;

    // Start is called before the first frame update
    void Start()
    {
        List<InputDevice> devices = new List<InputDevice>();
        
        InputDevices.GetDevicesWithCharacteristics(inputDeviceCharacteristics, devices);

        if (devices.Count > 0)
        {
            targetDevice = devices[0];

            GameObject prefab = controllerPrefabs.Find(controller => controller.name == targetDevice.name);
            Debug.Log(targetDevice.name);
            if (prefab)
            {
                spawnedController = Instantiate(prefab, transform);
            } else
            {
                spawnedController = Instantiate(controllerPrefabs[0], transform);
            }

            spawnedModel = Instantiate(handModelPrefab, transform);
            handAnimator = spawnedModel.GetComponent<Animator>();
        }
    }

    void UpdateHandAnimation()
    {
        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handAnimator.SetFloat("Trigger", triggerValue);
        } else
        {
            handAnimator.SetFloat("Trigger", 0);
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnimator.SetFloat("Grip", gripValue);
        }
        else
        {
            handAnimator.SetFloat("Grip", 0);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (showController)
        {
            spawnedModel.SetActive(false);
            spawnedController.SetActive(true);
        } else
        {
            spawnedModel.SetActive(true);
            spawnedController.SetActive(false);
            UpdateHandAnimation();
        }
    }
}
