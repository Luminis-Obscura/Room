using System.Collections.Generic;
using UnityEngine;

public class ReplyController : ChatController
{
    [SerializeField] private List<GameObject> replyObjects;
    private Animator animator;

    public override void Start()
    {
        DeactivateAllReplyObjects();
        base.Start();
    }

    public int ActivateRandomReplyObject()
    {
        int randomIndex = Random.Range(0, replyObjects.Count);
        ActivateReplyObject(randomIndex);
        return randomIndex;
    }
    
    public void DeactivateAllReplyObjects()
    {
        foreach (GameObject replyObject in replyObjects)
        {
            replyObject.SetActive(false);
        }
    }


    public void DeactivateReplyObject(int index)
    {
        if (index >= 0 && index < replyObjects.Count)
        {
            replyObjects[index].SetActive(false);
        }
        else
        {
            Debug.LogError("Index out of range: " + index);
        }
    }

    public void ActivateReplyObject(int index)
    {
        if (index >= 0 && index < replyObjects.Count)
        {
            replyObjects[index].SetActive(true);
        }
        else
        {
            Debug.LogError("Index out of range: " + index);
        }
    }
    
}
