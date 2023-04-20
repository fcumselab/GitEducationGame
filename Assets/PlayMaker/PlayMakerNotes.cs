using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace HutongGames.PlayMaker
{
    /// <summary>
    /// Container for HtmlNotes.Useful for documentation.
    /// Note: Tag GameObject as EditorOnly to not include in the build.
    /// </summary>
    [AddComponentMenu("PlayMaker Notes")]
    public class PlayMakerNotes : MonoBehaviour
    {

#if UNITY_EDITOR

        [ContextMenu("Locked")]
        public void ToggleLocked()
        {
            locked = !locked;
        }

        [ContextMenu("Expand hierarchy to show all FSMs")]
        public void ExpandHierarchy()
        {
            var fsmComponents = FindObjectsOfType<PlayMakerFSM>();
            foreach (var fsm in fsmComponents)
                EditorGUIUtility.PingObject(fsm);
        }

#endif

        [SerializeField] private List<HtmlNotes> notes;
        [SerializeField] private bool locked;
        [SerializeField] private bool autoOpen;

        public List<HtmlNotes> Notes
        {
            get { return notes; }
        }

        public bool Locked
        {
            get { return locked; }
            set { locked = value; }
        }

        public bool AutoOpen
        {
            get { return autoOpen; }
            set { autoOpen = value; }
        }

        private void Reset()
        {
            notes = new List<HtmlNotes>();
            locked = false;
        }
    }
}

