using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatManager : MonoBehaviour
{
    public ReplyController sheepController;
    public ChatController govermentController;
    public float sheepChatDuration = 3f;
    public float govermentChatDuration = 5f;

    public float cooldownTime = 0.5f;
    public float govermentReplayDelayMax = 2f;
    public float govermentReplayDelayMin = 0.5f;
    public bool isChatting = false;

    public void Chat()
    {
        if (isChatting) return;
        StartCoroutine(SheepChatCoroutine(sheepChatDuration));
        float randomDelay = Random.Range(govermentReplayDelayMin, govermentReplayDelayMax);
        StartCoroutine(GovernmentChatCoroutine(randomDelay, govermentChatDuration));
        StartCoroutine(WaitForSeconds(cooldownTime));
        isChatting = true;
    }

    private IEnumerator WaitForSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        isChatting = false;
    }

    private IEnumerator SheepChatCoroutine(float duration)
    {
        sheepController.DeactivateAllReplyObjects();
        sheepController.ActivateRandomReplyObject();
        ChatEnterAnimation(sheepController);
        yield return new WaitForSeconds(duration);
        ChatExitAnimation(sheepController);
    }

    private IEnumerator GovernmentChatCoroutine(float delay, float duration)
    {
        yield return new WaitForSeconds(delay);
        ChatEnterAnimation(govermentController);
        yield return new WaitForSeconds(duration);
        ChatExitAnimation(govermentController);
    }
    
    void ChatEnterAnimation(ChatController chatController)
    {
        chatController.PlayAnimation("Enter");
    }
    
    void ChatExitAnimation(ChatController chatController)
    {
        chatController.PlayAnimation("Exit");
    }
}
