using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameClock : MonoBehaviour
{
    public TMP_Text timeText; // Assign this in the inspector
    public TMP_Text dayText;  // Assign this in the inspector for displaying days


    private float timer = 0f;
    private int hoursPassed = 0;
    private int daysPassed = 1; // Starting from Day 1

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 60f) // Every real-time minute, increment in-game hour
        {
            hoursPassed++;
            timer = 0f;

            if (hoursPassed >= 24) // A new day
            {
                hoursPassed = 0; // Reset hours to 0
                daysPassed++;    // Increment day count
                UpdateDayDisplay(); // Update the day display
            }

            UpdateTimeDisplay(); // Update the time display
        }
    }

    private void UpdateTimeDisplay()
    {
        int displayHours = hoursPassed % 12;
        displayHours = displayHours == 0 ? 12 : displayHours; // Convert 0 to 12 for 12-hour format
        string amPm = hoursPassed >= 12 ? "PM" : "AM";

        // Update Time Text
        if (timeText != null)
        {
            timeText.text = string.Format("{0:00} {1}", displayHours, amPm);
        }
    }

    private void UpdateDayDisplay()
    {
        // Update Day Text
        if (dayText != null)
        {
            dayText.text = "Day " + daysPassed;
        }
    }
}

