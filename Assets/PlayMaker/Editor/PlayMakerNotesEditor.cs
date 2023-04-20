using System.Collections.Generic;
using System.Linq;
using HutongGames.Editor;
using UnityEngine;
using HutongGames.PlayMaker;
using JetBrains.Annotations;
using UnityEditor;

namespace HutongGames.PlayMakerEditor
{
    [CustomEditor(typeof(PlayMakerNotes))]
    public class PlayMakerNotesEditor : UnityEditor.Editor
    {
        private PlayMakerNotes notes;
        private readonly Dictionary<HtmlNotes, HtmlNotesEditor> editors = new Dictionary<HtmlNotes, HtmlNotesEditor>();

        [UsedImplicitly]
        private void OnEnable()
        {
            notes = target as PlayMakerNotes;
            if (notes == null) return; // shouldn't happen, maybe on scene load?

            foreach (var note in notes.Notes)
            {
                note.rawText = note.rawText.Replace("{{", "");
                note.rawText = note.rawText.Replace("}}", "");
            }
        }

        public override void OnInspectorGUI()
        {
            if (notes == null) return;

            var evt = Event.current;

            if (evt.type != EventType.Layout)
            {
                if (evt.clickCount > 1 ||
                    evt.type == EventType.ContextClick)
                {
                    evt.Use();
                }
            }

            foreach (var section in notes.Notes)
            {
                var editor = GetEditor(section);
                editor.OnGUI();
            }

            GUILayout.Space(10);
        }

        private HtmlNotesEditor GetEditor(HtmlNotes section)
        {
            HtmlNotesEditor editor;
            if (!editors.TryGetValue(section, out editor))
            {
                editor = new HtmlNotesEditor(notes, section);
                editors.Add(section, editor);
            }

            return editor;
        }
    }
}