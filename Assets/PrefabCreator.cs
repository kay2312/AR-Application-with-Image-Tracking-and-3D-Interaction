using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PrefabCreator : MonoBehaviour
{
    [SerializeField] private GameObject tigerPrefab;
    [SerializeField] private GameObject horsePrefab;
    [SerializeField] private GameObject dogPrefab;

    [SerializeField] private Vector3 tigerOffset;
    [SerializeField] private Vector3 horseOffset;
    [SerializeField] private Vector3 dogOffset;

    private ARTrackedImageManager arTrackedImageManager;
    private Dictionary<TrackableId, List<GameObject>> spawnedObjects = new();

    private void Awake()
    {
        arTrackedImageManager = GetComponent<ARTrackedImageManager>();
    }

    private void OnEnable()
    {
        if (arTrackedImageManager != null)
        {
            arTrackedImageManager.trackedImagesChanged += OnTrackablesChanged;
        }
    }

    private void OnDisable()
    {
        if (arTrackedImageManager != null)
        {
            arTrackedImageManager.trackedImagesChanged -= OnTrackablesChanged;
        }
    }

    private void OnTrackablesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage image in eventArgs.added)
        {
            CreatePrefabs(image);
        }

        foreach (ARTrackedImage image in eventArgs.updated)
        {
            UpdatePrefabsPosition(image);
        }
    }

    private void CreatePrefabs(ARTrackedImage image)
    {
        if (!spawnedObjects.ContainsKey(image.trackableId))
        {
            spawnedObjects[image.trackableId] = new List<GameObject>();
        }

        GameObject tiger = Instantiate(tigerPrefab, image.transform.position + tigerOffset, image.transform.rotation);
        tiger.transform.SetParent(image.transform);
        spawnedObjects[image.trackableId].Add(tiger);

        GameObject horse = Instantiate(horsePrefab, image.transform.position + horseOffset, image.transform.rotation);
        horse.transform.SetParent(image.transform);
        spawnedObjects[image.trackableId].Add(horse);

        GameObject dog = Instantiate(dogPrefab, image.transform.position + dogOffset, image.transform.rotation);
        dog.transform.SetParent(image.transform);
        spawnedObjects[image.trackableId].Add(dog);
    }

    private void UpdatePrefabsPosition(ARTrackedImage image)
    {
        if (!spawnedObjects.ContainsKey(image.trackableId))
            return;

        var objects = spawnedObjects[image.trackableId];

        for (int i = 0; i < objects.Count; i++)
        {
            if (objects[i] == null) continue;

            // ОНОВЛЮЄМО позицію тільки один раз після появи, потім від'єднуємо
            if (objects[i].transform.parent == image.transform)
            {
                objects[i].transform.SetParent(null);
            }
        }
    }
}
