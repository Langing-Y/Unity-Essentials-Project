using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI; // Required for Type handling

public class UpdateCollectibleCount : MonoBehaviour
{
    [Header("完成特效")]
    public GameObject completeEffect;//特效

    private TextMeshProUGUI collectibleText; // Reference to the TextMeshProUGUI component
    private string completeText = " Congratulation!";

    private bool isComplete = false;//防止重复触发

    void Start()
    {
        collectibleText = GetComponent<TextMeshProUGUI>();
        if (collectibleText == null)
        {
            Debug.LogError("UpdateCollectibleCount script requires a TextMeshProUGUI component on the same GameObject.");
            return;
        }
        UpdateCollectibleDisplay(); // Initial update on start
    }

    void Update()
    {
        if (!isComplete)
            UpdateCollectibleDisplay();
    }

    private void UpdateCollectibleDisplay()
    {
        int totalCollectibles = 0;

        // Check and count objects of type Collectible
        Type collectibleType = Type.GetType("Collectible");
        if (collectibleType != null)
        {
            totalCollectibles += UnityEngine.Object.FindObjectsOfType(collectibleType).Length;
        }

        // Optionally, check and count objects of type Collectible2D as well if needed
        Type collectible2DType = Type.GetType("Collectible2D");
        if (collectible2DType != null)
        {
            totalCollectibles += UnityEngine.Object.FindObjectsOfType(collectible2DType).Length;
        }

        //Debug.Log("Total collectibles: " + totalCollectibles);
        // Update the collectible count display
        Debug.Log("update num");
        collectibleText.text = $"Collectibles remaining: {totalCollectibles}";

        if (totalCollectibles == 0 && !isComplete)
        {
            Debug.Log("completed!");
            collectibleText.color = Color.green;
            collectibleText.text = completeText;
            isComplete = true;
            ShowCompleteEffect();
        }

    }

    private void ShowCompleteEffect()
    {
        GameObject player = GameObject.FindWithTag("Player");
        Debug.Log("Player found: " + (player != null));

        if (player != null && completeEffect != null)
        {
            Debug.Log("Spawning effect at: " + player.transform.position);
            Instantiate(completeEffect,player.transform.position, completeEffect.transform.rotation);
        }
        else
        {
            Debug.LogWarning("Player or CompleteEffect is null!");
        }
    }
}
