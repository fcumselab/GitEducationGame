using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class NewCommit : SerializedMonoBehaviour
{

    [SerializeField] CommitDatas commitDatas;

    [Header("References")]
    [SerializeField] Image icon;
    [SerializeField] Text textBoxId;
    [SerializeField] Text textBoxMessage;

    public void SetCommitDatas(CommitDatas data)
    {
        commitDatas = data;
    }

    public CommitDatas GetCommitDatas()
    {
        return commitDatas;
    }

    public void UpdateCommitUI(bool isNowCommit)
    {
        textBoxMessage.text = commitDatas.GetMessage();
        textBoxId.text = commitDatas.GetId();

        UpdateSprite(isNowCommit);
    }

    public void UpdateSprite(bool isNowCommit)
    {
        if (isNowCommit) icon.sprite = ImageManager.Instance.GetIconImage("CommitIconFocus");
        else icon.sprite = ImageManager.Instance.GetIconImage("CommitIconNotFocus");
    }
}
