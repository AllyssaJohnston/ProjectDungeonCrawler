using System.Collections.Generic;
using UnityEngine;

public class ChestBehavior : MonoBehaviour
{

    public float ChestResetTime = 1.5f;
    
    public List<Item> LootContents = new List<Item>(32);
    public ChestUI chestUI;
    private Animator animator;
    private bool isOpen = false;
    private bool playerInRange = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !isOpen)
        {
            animator.SetTrigger("Open");
            isOpen = true;
            chestUI.OpenChest(LootContents);
            StartCoroutine(ResetChest());
        }
    }

    System.Collections.IEnumerator ResetChest()
    {
        yield return new WaitForSeconds(ChestResetTime);
        isOpen = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        playerInRange = false;
        chestUI.CloseChest();
    }
}
