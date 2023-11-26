using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

public class SkipButton : MonoBehaviour
{

    public bool skip;
    public float waitTime;
    [SerializeField] AbstractDialogueUI dialogueUI;

    private void Awake()
    {
        //dialogueUI = GetComponentInChildren<AbstractDialogueUI>();
    }

    public void SkipToResponseMenu()
    {
        if (!skip)
        {
            skip = true;
            dialogueUI.OnContinue();
        }
        else
        {
            skip = false;
        }
    }

    public void StopSkip()
    {
        skip = false;
    }

    void OnConversationLine(Subtitle subtitle)
    {
        if (skip) StartCoroutine(ContinueAtEndOfFrame());
    }

    IEnumerator ContinueAtEndOfFrame()
    {
        yield return new WaitForSeconds(waitTime);
        dialogueUI.OnContinue();
    }

    void OnConversationResponseMenu(Response[] responses)
    {
        skip = false;
    }

    void OnConversationEnd(Transform actor)
    {
        skip = false;
    }

}