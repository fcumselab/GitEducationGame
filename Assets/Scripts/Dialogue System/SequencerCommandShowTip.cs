using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

namespace PixelCrushers.DialogueSystem.SequencerCommands
{

    public class SequencerCommandShowTip : SequencerCommand
    {
        string content = "";
        public void Start()
        {
            content = GetParameter(0);
            //Debug.Log("con: " + content);
            WindowManager.Instance.ShowTipTextBox(content);
            
            Stop();
        }
    }

}


/**/
