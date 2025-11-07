using UnityEngine;
using DialogueEditor;

public class ConservationStarter : MonoBehaviour
{
    [SerializeField] private NPCConversation myConversation;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ConversationManager.Instance.StartConversation(myConversation);
        }
    }
}
