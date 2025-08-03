using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Sets a random design for an enemy
public class TumbleDesignSetter : MonoBehaviour
{
    // Gets list of tumble designs
    private TumbleDesignSelectionButtons tdMenu;
    [HideInInspector]
    public SpriteRenderer spriteRenderer;
    // YT sprites played if user has enabled the yt design op
    [SerializeField]
    private Sprite[] ytSprites;
    Coroutine GanonCensorLoop;

    void Awake()
    {
        // Gets components
        spriteRenderer = GetComponent<SpriteRenderer>();
        // Gets list of tumble designs
        tdMenu = GameObject.Find("Tumble Design Menu").GetComponent<TumbleDesignSelectionButtons>();
    }

    void Start()
    {
        // Sets a random design to the enemy
        RandomTumbleDesign();
    }

    // Sets a random design to the enemy
    public void RandomTumbleDesign()
    {
        if (GanonCensorLoop != null)
            StopCoroutine(GanonCensorLoop);

        int randomNum;
        Sprite randomSprite;

        // If Youtube tumble mode is enabled
        if (tdMenu.isYtTumble || tdMenu.tumbleList.Count == 0)
        {
            // Instead of picking a random num from the selected list total it instead picks a random num from the total list
            randomNum = Random.Range(0, tdMenu.tumbleTotal);
            // Debug.Log(randomNum);

            // If the random num is above the list count it instead picks a Youtube sprite
            if (randomNum >= tdMenu.tumbleList.Count || tdMenu.tumbleList.Count == 0)
            {
                randomNum = Random.Range(0, ytSprites.Length - 1);
                randomSprite = ytSprites[randomNum];
                if (randomNum == 2)
                    // Loops the Ganon Censor sprites
                    GanonCensorLoop = StartCoroutine(GanonCensor());
            }
            // If below it uses a regular clip
            else
                randomSprite = tdMenu.tumbleList[randomNum];
        }
        // Picks a sprite from the selected tumble list
        else
        {
            randomNum = Random.Range(0, tdMenu.tumbleList.Count);
            randomSprite = tdMenu.tumbleList[randomNum];
        }

        spriteRenderer.sprite = randomSprite;
    }

    // Loops the Ganon Censor sprites
    IEnumerator GanonCensor()
    {
        while (true)
        {
            spriteRenderer.sprite = ytSprites[2];
            yield return new WaitForSeconds(1f);
            spriteRenderer.sprite = ytSprites[3];
            yield return new WaitForSeconds(1f);
        }
    }
    
}
