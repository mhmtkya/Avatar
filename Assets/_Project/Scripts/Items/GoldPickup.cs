using Unity.VisualScripting;
using UnityEngine;

public class GoldPickup : MonoBehaviour
{
    [HideInInspector]
    public int goldAmount;

    private bool isFollowing = false;
    private Transform playerTransform;
    private float moveSpeed = 5f;

    public void StartFollowing(Transform player)
    {
        isFollowing = true;
        playerTransform = player;
    }

    private void Update()
    {
        if (isFollowing && playerTransform != null)
        {
            moveSpeed += Time.deltaTime * 15f;
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerGold playerGold = collision.GetComponent<PlayerGold>();
            if (playerGold != null)
            {
                playerGold.AddGold(goldAmount);
                Destroy(gameObject);
            }
        }
    }
}
