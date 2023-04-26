using HutongGames.PlayMakerEditor;
using UnityEditor;
using UnityEngine;

namespace PixelCrushers.DialogueSystem.PlayMaker
{

    [CustomActionEditor(typeof(StartConversation))]
    public class CustomActionEditorStartConversation : CustomActionEditor
    {

        private ConversationPicker conversationPicker = null;
        private DialogueEntryPicker entryPicker = null;

        public override void OnEnable()
        {
            var action = target as StartConversation;
            if (action.conversation == null) action.conversation = new HutongGames.PlayMaker.FsmString();
            conversationPicker = new ConversationPicker(EditorTools.FindInitialDatabase(), action.conversation.Value, !action.conversation.UseVariable);
        }

        public override bool OnGUI()
        {
            var isDirty = false;

            var action = target as StartConversation;
            if (action == null) return DrawDefaultInspector();

            if (action.conversation == null) action.conversation = new HutongGames.PlayMaker.FsmString();
            action.conversation.UseVariable = EditorGUILayout.Toggle(new GUIContent("Use Variable", "Specify the conversation title with a PlayMaker variable"), action.conversation.UseVariable);

            if (action.conversation.UseVariable)
            {
                EditField("conversation");
            }
            else
            {
                conversationPicker.Draw(true);
                if (!string.Equals(action.conversation.Value, conversationPicker.currentConversation))
                {
                    action.conversation.Value = conversationPicker.currentConversation;
                    isDirty = true;
                    entryPicker = null;
                }
            }

            if (action.startingEntryID == null)
            {
                action.startingEntryID = new HutongGames.PlayMaker.FsmInt();
                action.startingEntryID.Value = -1;
            }
            var specifyEntryID = (action.startingEntryID.Value != -1);
            EditorGUI.BeginChangeCheck();
            specifyEntryID = EditorGUILayout.Toggle(new GUIContent("Specify Starting Entry", "Specify a dialogue entry at which to start the conversation"), specifyEntryID);
            if (EditorGUI.EndChangeCheck())
            {
                action.startingEntryID.Value = specifyEntryID ? 0 : -1;
            }
            if (specifyEntryID)
            {
                if (entryPicker == null)
                {
                    entryPicker = new DialogueEntryPicker(action.conversation.Value);
                }
                if (entryPicker.isValid)
                {
                    action.startingEntryID.Value = entryPicker.DoLayout("Starting Entry ID", action.startingEntryID.Value);
                }
                else
                {
                    EditField("startingEntryID");
                }
            }

            EditField("actor");
            EditField("conversant");
            EditField("exclusive");
            EditField("replace");

            return isDirty || GUI.changed;
        }
    }
}