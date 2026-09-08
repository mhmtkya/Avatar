using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChestPickuo : MonoBehaviour
{
    private bool isOpened = false;
    private bool isPlayerInRange = false;
    
    
    private PlayerStats cachedPlayerStats;
    private PlayerGold cachedPlayerGold;

    [Header("Chest Settings")] 
    public int chestCost = 50;
    public Sprite openedChestSprite;
    
    [Header("UI & Visuals")]
    public TextMeshPro costText;
    public SpriteRenderer spriteRenderer;

    [Header("Input Settings")]
    public InputActionReference interactAction;


    private void Start()
    {
        if (costText != null)
            costText.text = chestCost.ToString() + "G";
    }

    private void OnEnable()
    {
        if (interactAction != null)
        {
            interactAction.action.Enable();
            interactAction.action.performed += OnInteractPressed;
        }
    }

    private void OnDisable()
    {
        if (interactAction != null)
        {
            interactAction.action.performed -= OnInteractPressed;
        }
    }

    private void OnInteractPressed(InputAction.CallbackContext context)
    {
        if (!isOpened && isPlayerInRange)
        {
            if (cachedPlayerGold != null && cachedPlayerGold.HasEnoughGold(chestCost))
            {
                cachedPlayerGold.SpendGold(chestCost);
                
                OpenChest();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange =  true;
            cachedPlayerStats = other.GetComponent<PlayerStats>();
            cachedPlayerGold = other.GetComponent<PlayerGold>();
            costText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange =  false;
            cachedPlayerStats = null;
            cachedPlayerGold = null;
            costText.gameObject.SetActive(false);
        }
    }

    private void OpenChest()
    {
        isOpened = true;

        if (spriteRenderer != null && openedChestSprite != null)
        {
            spriteRenderer.sprite = openedChestSprite;
        }

        if (costText != null)
        {
            costText.gameObject.SetActive(false);        
        }
        
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;
        
    }
}
