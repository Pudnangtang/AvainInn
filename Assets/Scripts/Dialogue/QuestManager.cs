using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestManager : MonoBehaviour
{
    [Header("Quest Data")]
    public string title;
    public string description;

    [Header("Quest State")]
    public bool isActive;
    public bool isCompleted;

    [Header("Quest Log UI")]
    [SerializeField] private GameObject questLogPanel;
    [SerializeField] private TextMeshProUGUI questTitleText;
    [SerializeField] private TextMeshProUGUI questDescriptionText;

    // Event delegates for quest start and completion
    public delegate void QuestAction(QuestManager quest);
    public event QuestAction OnQuestStarted;
    public event QuestAction OnQuestCompleted;

    void Start()
    {
        questLogPanel.SetActive(false);
    }

    // Call this method to start the quest
    public void ActivateQuest()
    {
        Debug.Log($"Activating quest: {title}");

        isActive = true;
        isCompleted = false;

        if (questTitleText != null && questDescriptionText != null)
        {
            questTitleText.text = title;
            questDescriptionText.text = description;
            questLogPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("Quest UI components are not assigned!");
        }

        OnQuestStarted?.Invoke(this);
    }

    // Call this method when the player completes the quest
    public void CompleteQuest()
    {
        isActive = false;
        isCompleted = true;

        questLogPanel.SetActive(false);

        OnQuestCompleted?.Invoke(this);
    }

    // Additional quest-related methods can be added here
}
