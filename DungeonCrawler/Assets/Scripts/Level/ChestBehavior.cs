using System.Collections;
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
	private static bool rememberControls;
	private static bool memoryIsSet = false;

	void Start()
    {
        animator = GetComponent<Animator>();

        if (chestUI == null) chestUI = FindFirstObjectByType<ChestUI>();
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !isOpen)
        {
			if (GameManagerBehavior.modernControls) {
				if (!memoryIsSet) {
					rememberControls = GameManagerBehavior.modernControls;
					memoryIsSet = true;
				}
				GameManagerBehavior.modernControls = false;
			}

			animator.SetTrigger("Open");
            isOpen = true;
            chestUI.OpenChest(LootContents);

			StartCoroutine(ResetChest());
        }
    }

    IEnumerator ResetChest()
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
        StartCoroutine(closeChest());
        playerInRange = false;
        chestUI.CloseChest();
		if (memoryIsSet) {
			GameManagerBehavior.modernControls = rememberControls;
		}

	}

    IEnumerator closeChest()
    {
		if (memoryIsSet) {
			GameManagerBehavior.modernControls = rememberControls;
			memoryIsSet = false;
		}
		yield return new WaitForSeconds(.75f);
        animator.SetTrigger("Close");
    }
}
