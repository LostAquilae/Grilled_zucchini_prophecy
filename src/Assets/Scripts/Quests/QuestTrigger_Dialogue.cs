﻿public class QuestTrigger_Dialogue : QuestTrigger
{
    public Dialogue dialogue = new Dialogue();

    override public void Trigger()
    {
        if (GameManager.dialogueManager.ShowDialogue(dialogue))
        {
            base.Trigger();
        }
    }

    public override void EndOfInteraction()
    {
        base.EndOfInteraction();

        GameManager.dialogueManager.EndDialogue();
    }
}
