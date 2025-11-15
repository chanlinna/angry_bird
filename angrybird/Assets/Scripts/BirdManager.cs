using UnityEngine;
using System.Collections;

public class BirdManager : MonoBehaviour
{
    public static BirdManager Instance;

    [Header("Birds Queue")]
    public GameObject[] birds;
    private int currentBirdIndex = 0;

    [Header("Flight Settings")]
    public float flySpeed = 5f;
    public float rotationSpeed = 360f;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        SetupBirds();
    }

    private void SetupBirds()
    {
        for (int i = 0; i < birds.Length; i++)
        {
            if (birds[i] != null)
            {
                birds[i].SetActive(true); // all birds visible on ground
                birds[i].GetComponent<Bird>().isCurrentBird = (i == currentBirdIndex);
            }
        }

        // Start flight animation for the first bird
        if (birds.Length > 0)
            StartCoroutine(FlyToSlingshot(birds[0]));
    }

    private IEnumerator FlyToSlingshot(GameObject bird)
    {
        Bird birdScript = bird.GetComponent<Bird>();
        Vector3 target = birdScript.slingshotAnchor.position;
        float angle = 0f;

        while (Vector3.Distance(bird.transform.position, target) > 0.05f)
        {
            bird.transform.position = Vector3.MoveTowards(bird.transform.position, target, flySpeed * Time.deltaTime);
            angle += rotationSpeed * Time.deltaTime;
            bird.transform.rotation = Quaternion.Euler(0, 0, angle);
            yield return null;
        }

        bird.transform.position = target;
        bird.transform.rotation = Quaternion.identity;
    }

    public void BirdFinished()
    {
        birds[currentBirdIndex].GetComponent<Bird>().isCurrentBird = false;
        currentBirdIndex++;

        if (currentBirdIndex < birds.Length)
        {
            birds[currentBirdIndex].GetComponent<Bird>().isCurrentBird = true;
            StartCoroutine(FlyToSlingshot(birds[currentBirdIndex]));
        }
        else
        {
            Debug.Log("All birds used! Level over.");
        }
    }
}
