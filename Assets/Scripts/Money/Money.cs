using UnityEngine;
using UnityEngine.UI; // Include this namespace to work with basic UI
using TMPro; // Uncomment this if you're using TextMeshPro

public class Money : MonoBehaviour
{
    public int money;
    public TMP_Text moneyText; // Reference to the UI Text component
    // public TextMeshProUGUI moneyText; // Uncomment this if you're using TextMeshPro

    void Start()
    {
        // Initialize money display
        UpdateMoneyDisplay();
    }

    public void AddScore(int amount)
    {
        money += amount;
        UpdateMoneyDisplay();
    }

    private void UpdateMoneyDisplay()
    {
        // Assuming moneyText is already set to the correct UI element
        if (moneyText != null)
        {
            moneyText.text = "$ " + money.ToString();
        }
        else
        {
            Debug.LogError("Money text UI component not set!");
        }
    }
}
