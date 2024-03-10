using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class CurrentFolderPanel : SerializedMonoBehaviour
{
    [SerializeField] GameObject GitCommandValider;

    public void MoveGitCoreLocation()
    {
        GitCommandValider = GameObject.Find("GitCommandValider");
        PlayMakerFSM gitFsm = MyPlayMakerScriptHelper.GetFsmByName(GitCommandValider, "Has .git Folder");
        GameObject gitCoreObj = gitFsm.FsmVariables.GetFsmGameObject("gitCoreObj").Value;
        
        Transform TargetLocationObj = gitCoreObj.transform.parent;
        PlayMakerFSM fileFsm = MyPlayMakerScriptHelper.GetFsmByName(TargetLocationObj.Find("file").gameObject, "file");
        fileFsm.SendEvent("Current Folder Panel/Go Next Location");
    }
}
