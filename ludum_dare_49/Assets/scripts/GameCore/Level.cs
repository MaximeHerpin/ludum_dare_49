using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level : MonoBehaviour
{
    private class LevelElement
    {
        public Transform transform;
        public Vector3 initialLPosition;
        public Quaternion initialRotation;
        public Rigidbody rb;

        public LevelElement(GameObject element)
        {
            transform = element.transform;
            initialLPosition = transform.position;
            initialRotation = transform.rotation;
            rb = element.GetComponent<Rigidbody>();
        }
    }

    public Transform levelStart;
    public GameObject elementsContainer;
    public Transform levelEnd;

    public Collectible starPrefab;
    public Transform[] stars;

    [HideInInspector] public Bounds levelEndBounds;
    [HideInInspector] public Bounds levelBounds;

    public bool isFinished = false;
    public bool isStarted = false;
    public int bestScore = 0;
    public float cameraDistance = 30;
    private List<LevelElement> elements;
    private List<GameObject> starsInstances = new List<GameObject>();
    private int curentScore;

    private void SpawnStars()
    {
        DeleteStarInstances();
        if (stars.Length != 3)
        {
            Debug.Log("invalid amount of stars", this);
        }

        foreach (Transform star in stars)
        {
            var instance = Instantiate<Collectible>(starPrefab, star.position, star.rotation, star);
            instance.onCollected = (x) => { curentScore++;};
            starsInstances.Add(instance.gameObject);
        }
    }

    private void DeleteStarInstances()
    {
        foreach (var instance in starsInstances)
        {
            Destroy(instance);
        }
        starsInstances.Clear(); 
    }

    private void OnDrawGizmosSelected()
    {
        DrawGizmo();
    }

    public void DrawGizmo()
    {
        Gizmos.color = Color.green;
        if (levelStart != null)
            Gizmos.DrawSphere(levelStart.position, 1f);
        Gizmos.DrawWireCube(levelEndBounds.center, levelEndBounds.size);
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(levelBounds.center, levelBounds.size);
        Gizmos.color = Color.cyan;
        foreach (var star in stars)
        {
            Gizmos.DrawSphere(star.position, .2f);
        }
    }

    public void InitilizeElements()
    {
        elements = new List<LevelElement>();
        Transform[] allChildren = elementsContainer.GetComponentsInChildren<Transform>();
        foreach (var child in allChildren)
        {
            elements.Add(new LevelElement(child.gameObject));
        }
    }

    public bool IsWithinLevel(Vector3 position)
    {
        return levelBounds.Contains(position);
    }

    public bool IsAtTheEnd(Vector3 position)
    {
        return levelEndBounds.Contains(position);
    }

    public void OnEndLevel()
    {
        isFinished = true;
        if (curentScore > bestScore)
        {
            bestScore = curentScore;
        }
        DeleteStarInstances();
    }

    public void Cleanup()
    {
        ResetElements();
        DeleteStarInstances();
    }

    public void ResetLevel()
    {
        PlayerManager.instance.PlacePlayer(levelStart);
        curentScore = 0;
        SpawnStars();
        ResetElements();
    }

    public void StartLevel()
    {
        curentScore = 0;
        isStarted = true;
        SpawnStars();
        CameraManager.instance.targetDistance = cameraDistance;
        CameraManager.instance.SetFocusBounds(levelBounds);
        ResetElements();
    }

    private void ResetElements()
    {
        foreach (var element in elements)
        {
            element.transform.rotation = element.initialRotation;
            element.transform.position = element.initialLPosition;
            if (element.rb != null)
            {
                element.rb.velocity = Vector3.zero;
                element.rb.angularVelocity = Vector3.zero;
            }
        }
    }
}
