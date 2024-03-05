using UnityEngine;

public class HappinessDisplay : MonoBehaviour
{
    public Sprite redSprite; // Assign in the Inspector
    public Sprite yellowSprite; // Assign in the Inspector
    public Sprite greenSprite; // Assign in the Inspector

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void UpdateHappiness(int happinessLevel)
    {
        if (happinessLevel >= 66)
        {
            Debug.Log("green");
            spriteRenderer.sprite = greenSprite;
        }
        else if (happinessLevel >= 40)
        {
            Debug.Log("yellow.");
            spriteRenderer.sprite = yellowSprite;
        }
        else
        {
            Debug.Log("red");
            spriteRenderer.sprite = redSprite;
        }
    }
}
