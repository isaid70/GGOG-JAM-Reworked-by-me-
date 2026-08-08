using UnityEngine;

public class DenemeDialogue : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public Dialogue skillDialogue;

    private void Start()
    {
        UseSkill();
    }
    public void UseSkill()
    {
        dialogueManager.StartDialogue(skillDialogue);
    }
}
