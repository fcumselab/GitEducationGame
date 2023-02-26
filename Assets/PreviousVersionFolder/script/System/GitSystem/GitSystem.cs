using Console;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GitSystem : MonoBehaviour , Panel
{
    [SerializeField]
    GameObject mainFlag;
    [SerializeField]
    GameObject headFlag;

    List <GameObject> flagObjects = new List<GameObject>();
    List<GameObject> commitObjects = new List<GameObject>();
    [SerializeField]
    GameObject startCommit;
    [SerializeField]
    GameObject remoteObjects;
    [SerializeField]
    public GameObject localObjects;
    [SerializeField]
    GameObject exampleTagObject;
    public GameObject nowCommit { private set; get; }
    public Repository localRepository;

    [SerializeField]
    FileSystem fileSystem;
    [SerializeField]
    ConflictSystem conflictSystem;

    [SerializeField]
    GameObject fileInputField;
    [SerializeField]
    GameObject gitInputField;
    [SerializeField]
    GameObject gitFileObject;
    List<KeyValuePair<string, string>> modifiedFiles;
    List<string> confilctFiles;

    [SerializeField]
    List<string> remotes;

    public Repository serverRepository;

    public bool cloned { private set; get; } = false;
    public bool hasPush { private set; get; } = false;
    public bool sync { private set; get; } = false;
    public bool conflicted { private set; get; } = false;
    public bool isCalledGitVersion { private set; get; } = false;

    public bool hasStash { private set; get; } = false;
    List<KeyValuePair<string, string>> stashFiles;
    public int tagCounts;


    public void buildRepository()
    {
        if ( modifiedFiles == null )
        {
            modifiedFiles = new List<KeyValuePair<string, string>>();
        }
        if (stashFiles == null)
        {
            stashFiles = new List<KeyValuePair<string, string>>();
        }
        localRepository = new Repository();
        mainFlag.SetActive(true);
        mainFlag.GetComponent<Image>().color = Color.red;
        headFlag = mainFlag;
        flagObjects.Add(mainFlag);
        nowCommit = null;
    }

    public bool hasRepository()
    {
        if (localRepository != null)
        {
            return true;
        }
        return false;
    }
    public bool hasRemote()
    {
        if(remotes.Count != 0)
        {
            return true;
        }
        return false;
    }
    public void addRemote(string name)
    {
        remotes.Add(name);
    }

    public void Commit(string name)
    {
        // Debug.Log("Commit!! " + name);
        if (hasRepository() && !conflicted)
        {
            //Commit.cs
            Commit newCommit = new Commit(name, "");

            for (int i = 0; i < modifiedFiles.Count; i++)
            {
                newCommit.addModifiedFile(modifiedFiles[i]);
                fileSystem.untrackFile(modifiedFiles[i].Key);

            }
            if (nowCommit == null)
            {
                // Debug.Log("nowCommit null ");
                nowCommit = startCommit;
                startCommit.SetActive(true);
            }
            else
            {
                //bp
                // Debug.Log("nowCommit not null! ");
                //Debug.Log("commit nowCommit  " + nowCommit);
                // GameObject newCommitObject = Instantiate(nowCommit, nowCommit.transform.parent);
                GameObject newCommitObject = Instantiate(nowCommit, headFlag.transform.Find("title").parent);
                
                //commit child arrow setActive
                newCommitObject.transform.GetChild(1).gameObject.SetActive(true);
                newCommitObject.GetComponent<RectTransform>().localPosition = new Vector3(nowCommit.GetComponent<RectTransform>().localPosition.x - 150, nowCommit.GetComponent<RectTransform>().localPosition.y, nowCommit.GetComponent<RectTransform>().localPosition.z);
                nowCommit = newCommitObject;
            }
            // nowCommit.GetComponentInChildren<Text>().text = newCommit.name + "\ncommitId:" + newCommit.id;
            nowCommit.GetComponentInChildren<Text>().text = newCommit.name + "\n    " + newCommit.id;
            nowCommit.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector3(100 - newCommit.name.Length * 5, -49, 0);
            // normal flag set
            headFlag.GetComponent<RectTransform>().localPosition = new Vector3(nowCommit.GetComponent<RectTransform>().localPosition.x - 160, nowCommit.GetComponent<RectTransform>().localPosition.y + 5, headFlag.GetComponent<RectTransform>().localPosition.z);
            // new branch and start commit
            if (localRepository.nowBranch.branchStart)
            {
                int size = localRepository.nowBranch.nowCommit.branchUsed;
                nowCommit.GetComponent<RectTransform>().localPosition = new Vector3(nowCommit.GetComponent<RectTransform>().localPosition.x, nowCommit.GetComponent<RectTransform>().localPosition.y - 145 * size, nowCommit.GetComponent<RectTransform>().localPosition.z);
                nowCommit.transform.GetChild(1).GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 45);
                nowCommit.transform.GetChild(1).GetComponent<RectTransform>().localPosition = new Vector3(67, 45, 0);
                headFlag.GetComponent<RectTransform>().localPosition = new Vector3(headFlag.GetComponent<RectTransform>().localPosition.x, headFlag.GetComponent<RectTransform>().localPosition.y - 140 * size, headFlag.GetComponent<RectTransform>().localPosition.z);
                nowCommit.GetComponent<Image>().color = UnityEngine.Random.ColorHSV();
            }
            else
            {
                nowCommit.transform.GetChild(1).GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 0);
                nowCommit.transform.GetChild(1).GetComponent<RectTransform>().localPosition = new Vector3(67, 0, 0);
            }
            headFlag.transform.GetChild(1).gameObject.SetActive(true);
            localRepository.Commit(newCommit);
            modifiedFiles = new List<KeyValuePair<string, string>>();
            nowCommit.name = newCommit.name;
            commitObjects.Add(nowCommit);
            //nowCommit.GetComponent<Image>().color = new Color(Random.Range(0, 255), Random.Range(0, 255), Random.Range(0, 255));
            sync = false;
            hasPush = false;
        }
    }

    public void Commit(Commit commit,string branch)
    {
        
        if (hasRepository() && !conflicted)
        {

            Commit newCommit = new Commit(commit.name, "");
            for (int i = 0; i < modifiedFiles.Count; i++)
            {
                newCommit.addModifiedFile(modifiedFiles[i]);
                fileSystem.untrackFile(modifiedFiles[i].Key);

            }
            if (nowCommit == null)
            {
                nowCommit = startCommit;
                startCommit.SetActive(true);
            }
            else
            {
                // Debug.Log("nowCommit" + nowCommit);
                // Debug.Log(branch + "_" + commit.name);
                // Debug.Log("Find :" + GameObject.Find(branch + "_" + commit.name).name);
                GameObject newCommitObject = Instantiate(GameObject.Find(branch + "_" + commit.name), nowCommit.transform.parent);
                newCommitObject.transform.GetChild(1).gameObject.SetActive(true);
                newCommitObject.GetComponent<RectTransform>().localPosition = new Vector3(nowCommit.GetComponent<RectTransform>().localPosition.x - 150, nowCommit.GetComponent<RectTransform>().localPosition.y, nowCommit.GetComponent<RectTransform>().localPosition.z);
                nowCommit = newCommitObject;
            }
            nowCommit.GetComponentInChildren<Text>().text = newCommit.name + "\ncommitId:" + newCommit.id;

            //commit text
            nowCommit.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector3(100 - newCommit.name.Length * 5, -49, 0);
            
            // normal flag set
            headFlag.GetComponent<RectTransform>().localPosition = new Vector3(nowCommit.GetComponent<RectTransform>().localPosition.x - 160, nowCommit.GetComponent<RectTransform>().localPosition.y + 5, headFlag.GetComponent<RectTransform>().localPosition.z);
            
            // new branch and start commit
            if (localRepository.nowBranch.branchStart)
            {
                int size = localRepository.branches.Count;
                nowCommit.GetComponent<RectTransform>().localPosition = new Vector3(nowCommit.GetComponent<RectTransform>().localPosition.x, nowCommit.GetComponent<RectTransform>().localPosition.y - 145, nowCommit.GetComponent<RectTransform>().localPosition.z);
                nowCommit.transform.GetChild(1).GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 45);
                nowCommit.transform.GetChild(1).GetComponent<RectTransform>().localPosition = new Vector3(67, 45, 0);
                headFlag.GetComponent<RectTransform>().localPosition = new Vector3(headFlag.GetComponent<RectTransform>().localPosition.x, headFlag.GetComponent<RectTransform>().localPosition.y - 140, headFlag.GetComponent<RectTransform>().localPosition.z);
                
            }
            else
            {
                nowCommit.transform.GetChild(1).GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, 0);
                nowCommit.transform.GetChild(1).GetComponent<RectTransform>().localPosition = new Vector3(67, 0, 0);
            }
            headFlag.transform.GetChild(1).gameObject.SetActive(true);
            localRepository.Commit(commit);
            modifiedFiles = new List<KeyValuePair<string, string>>();
            nowCommit.name = localRepository.nowBranch.branchName + "_" + newCommit.name;
            commitObjects.Add(nowCommit);
            //nowCommit.GetComponent<Image>().color = new Color(Random.Range(0, 255), Random.Range(0, 255), Random.Range(0, 255));
            sync = false;
            hasPush = false;
        }
    }

    public void Commit()
    {
        if (hasRepository() && !conflicted)
        {
            gitFileObject.SetActive(true);
            GameSystemManager.GetSystem<PanelManager>().AddSubPanel(this);
        }
    }

    public bool Merge(string branchName)
    {
        if (!localRepository.hasBranch(branchName))
            return false;

        // if conflict
        confilctFiles = new List<string>();
        Branch branch = localRepository.branches.Find(x => x.branchName == branchName);
        for (int i = 0; i < branch.nowCommit.allFiles.Count; i++)
        {
            for (int j = 0; j < localRepository.nowBranch.nowCommit.allFiles.Count; j++)
            {
                //Debug.Log(serverRepository.nowBranch.nowCommit.allFiles[i].Key + " : " +  modifiedFiles[j].Key);
                if (branch.nowCommit.allFiles[i].Key == localRepository.nowBranch.nowCommit.allFiles[j].Key)
                {
                    if (branch.nowCommit.allFiles[i].Value != localRepository.nowBranch.nowCommit.allFiles[j].Value)
                    {
                        Debug.Log("Conflicted: " + localRepository.nowBranch.nowCommit.allFiles[j].Key);
                        Debug.Log(localRepository.nowBranch.nowCommit.allFiles[j].Value + " : " + branch.nowCommit.allFiles[i].Value);
                        confilctFiles.Add(localRepository.nowBranch.nowCommit.allFiles[j].Key);
                    }
                }
            }

        }
        if (confilctFiles.Count > 0)
        {
            conflicted = true;
            conflictSystem.OpenConflict(confilctFiles, localRepository.nowBranch.nowCommit.allFiles, branch.nowCommit.allFiles, true, branchName);
            GameSystemManager.GetSystem<PanelManager>().AddSubPanel(conflictSystem);
            conflictSystem.gameObject.SetActive(true);
        }
        else
        {
            Commit("Merge branch " + branchName);
            nowCommit.transform.GetChild(2).gameObject.SetActive(true);
        }
        return true;
    }

    public bool Pull(string remote, string branch)
    {
        if (remotes.Contains(remote))
        {
           
            if (serverRepository.hasBranch(branch))
            {
                
                confilctFiles = new List<string>();
                //Debug.Log(serverRepository.nowBranch.nowCommit.allFiles.Count + " : " + modifiedFiles.Count);
                for (int i = 0; i < serverRepository.nowBranch.nowCommit.allFiles.Count; i++)
                {
                    for(int j = 0; j< modifiedFiles.Count; j++)
                    {
                        //Debug.Log(serverRepository.nowBranch.nowCommit.allFiles[i].Key + " : " +  modifiedFiles[j].Key);
                        if (serverRepository.nowBranch.nowCommit.allFiles[i].Key == modifiedFiles[j].Key)
                        {
                            //Debug.Log("Conflicted: " + modifiedFiles[j].Key);
                            confilctFiles.Add(modifiedFiles[j].Key);
                        }
                    }

                }
                if( confilctFiles.Count > 0)
                {
                    conflicted = true;
                    GameSystemManager.GetSystem<PanelManager>().AddSubPanel(conflictSystem);
                    conflictSystem.OpenConflict(confilctFiles, modifiedFiles, serverRepository.nowBranch.nowCommit.allFiles,false,"");
                    conflictSystem.gameObject.SetActive(true);
                }
                return true;
            }
        }
        return false;
    }

    public void Push()
    {
        if (remoteObjects != null)
        {
            Destroy(remoteObjects);
        }
        remoteObjects = Instantiate(localObjects, localObjects.transform.parent);
        remoteObjects.name = "remote";
        remoteObjects.GetComponent<RectTransform>().localPosition = new Vector3(0, 174, 0);
        foreach (Transform remoteObject in remoteObjects.transform)
        {
            if(remoteObject.name == "masterFlag") {
                remoteObject.GetComponent<Image>().color = Color.white;
            }
        }
        
        hasPush = true;
        sync = true;
    }

    

    public void trackFile(string fileName, string Content)
    {
        //Debug.Log(fileName + " " + Content);
        fileSystem.trackFile(fileName);
        //Debug.Log(modifiedFiles);
        modifiedFiles.Add( new KeyValuePair<string,string>(fileName, Content));
    }
    public void untrackFile(string fileName)
    {
        fileSystem.untrackFile(fileName);
        modifiedFiles.Remove( modifiedFiles.Find( x=> x.Key == fileName  )  );
    }

    public void PanelInput()
    {
        if ((Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return)))
        {
            if( gitInputField.GetComponent<InputField>().text == "!wq" )
            {
                Commit(fileInputField.GetComponent<InputField>().text);
                gitFileObject.SetActive(false);
                GameSystemManager.GetSystem<PanelManager>().ReturnTopPanel();
            }
            //Commit(gitInputField.);
        }
    }

    public bool cloneRepository(string remote)
    {
        localObjects.SetActive(true);
        cloned = true;
        hasPush = false;
        sync = true;

        if (remotes.Contains(remote))
        {
            // GameObject cloneObjects = Instantiate(remoteObjects, localObjects.transform.parent);
            // cloneObjects.GetComponent<RectTransform>().localPosition = new Vector3(0, -140, 0);
            Destroy(localObjects);
            localObjects = Instantiate(remoteObjects, remoteObjects.transform.parent);
            localObjects.name = "local";
            int childCounts = localObjects.transform.childCount;
            
            foreach (Transform localObject in localObjects.transform)
            {
                int localObjectNumber = 0;
                if(localObject.name.Contains("gitCommit"))
                {
                    if(localObject.name != "gitCommit")
                    {
                        localObjectNumber = Int32.Parse(localObject.name.Split("gitCommit")[1]);
                        if (localObjectNumber == childCounts - 1)
                        {
                            nowCommit = localObject.gameObject;
                        }
                    }
                    else
                    {
                        startCommit = localObject.gameObject;
                    }
                    
                } else if(localObject.name == "masterFlag") {

                    mainFlag = localObject.gameObject;
                    headFlag = mainFlag;
                }
            }
            
            localObjects.GetComponent<RectTransform>().localPosition = new Vector3(0, -140, 0);
            cloned = true;
            //localRepository = serverRepository;
            //mainFlag = localObjects.transform.GetChild(0).gameObject;
            sync = true;
            hasPush = false;
            // fileSystem.NewFile("index","<h1>Hello World!</h1>");
            // fileSystem.NewFile("page1", "<h2>page1</h2>");

            if (modifiedFiles == null)
            {
                modifiedFiles = new List<KeyValuePair<string, string>>();
            }
            mainFlag.GetComponent<Image>().color = Color.red;
            //headFlag = mainFlag;
            //flagObjects.Add(mainFlag);
            //Debug.Log("true");
            return true;
        }
        else
        {
            //Debug.Log("false");
            return false;
        }
    }

    public bool checkout(string name)
    {
        // Debug.Log("checkout: " + name);

        string[] nameList = name.Split('~','^');
        
        GameObject switchFlag = flagObjects.Find(x => x.name == nameList[0] + "Flag");
        
        if(switchFlag != null){
                //bp
            if(nameList.Length > 1){
                //type: git checkout <branchName>~2^3 
                List<char> symbolList = new List<char>();

                int nowP = 0;
                while (true){
                    int findP1 = name.IndexOf('^',nowP);
                    int findP2 = name.IndexOf('~',nowP);
                    if(findP1 == -1 && findP2 == -1){
                        break;
                    }else if ((findP1 < findP2 && findP1 != -1) || (findP2 == -1)){
                        nowP = findP1 + 1;
                        symbolList.Add('^');
                    }else if((findP1 > findP2 && findP2 != -1) || (findP1 == -1)){
                        nowP = findP2 + 1;
                        symbolList.Add('~');
                    }
                }
                
                Branch branch = localRepository.branches.Find( branch => branch.branchName == nameList[0]);

                //get all commit in a branch
                string line = "";
                foreach (Commit commit in branch.commits)
                {
                    line += commit.id;
                    line += " , ";
                }
                //Debug.Log("0 " + branch.commits[0].id);
                //Debug.Log("branch: " + branch.branchName + "; Length: " + branch.commits.Count + "; All commits: " + line);
                //end

                int CommitLen = branch.commits.Count - 1;
                for(int i=0;i<symbolList.Count;i++){
                    if(symbolList[i] == '~' && nameList[i + 1] == ""){
                        CommitLen--;
                        // Debug.Log("now ~ " + CommitLen);
                    }else if(symbolList[i] == '~'){
                        CommitLen -= Int16.Parse(nameList[i + 1]);
                        // Debug.Log("now ~mul " + CommitLen);
                    }
                }
                if(CommitLen >= 0){
                    Debug.Log("Move to: " + branch.commits[CommitLen].id);
                    return checkoutByCommitId(branch, branch.commits[CommitLen]);
                }else{
                    Debug.Log("Can\'t find this commit.");
                    return false;
                }
            }else{

                //type: git checkout <branchName>
                
                if(switchFlag == headFlag){
                    GameSystemManager.GetSystem<DeveloperConsole>().AddMessageToConsole("Already on " + '\"' + name + '\"');
                    return true;
                }

                switchFlag.GetComponent<Image>().color = Color.red;
                localRepository.switchBranch(name);
                headFlag.GetComponent<Image>().color = Color.white;

                nowCommit = switchFlag.transform.GetChild(switchFlag.transform.childCount-1).gameObject;
                if(nowCommit.name == "arrow")
                {
                    nowCommit = headFlag.transform.GetChild(headFlag.transform.childCount-1).gameObject;
                }

                headFlag = switchFlag;
                return true;
            }
        }else{
            //type: git checkout b4z3weacx...(commit id)
            for(int i=0; i< localRepository.branches.Count; i++){
                Branch branch = localRepository.branches[i];
                Commit findC = branch.commits.Find( commit => commit.id == name );
                return checkoutByCommitId(branch,findC);
            }
        }

        Debug.Log("not found this branch");
        return false;
    }

    bool checkoutByCommitId(Branch branch, Commit findC){
        if(findC != null){
            //Debug.Log("branch: " + branch.branchName);
            
            localRepository.switchBranch(branch.branchName);

            Commit saveCommit = localRepository.nowBranch.nowCommit;
            localRepository.nowBranch.nowCommit = findC;
            // foreach(var a in localRepository.branches){
            //     Debug.Log("branch: " + a.branchName);
            // }
            createBranch("HEAD");
            localRepository.nowBranch.nowCommit = saveCommit;

            GameObject switchFlag = flagObjects.Find(x => x.name == "HEADFlag");
            switchFlag.GetComponent<Image>().color = Color.red;
            localRepository.switchBranch("HEAD");
            localRepository.nowBranch.nowCommit = findC;
            
            //Debug.Log(headFlag.transform.name);

            foreach(GameObject flagObj in flagObjects){
                Transform findObj = flagObj.transform.Find(findC.name);
                if(findObj != null && flagObj.name != "HEAD"){
                    headFlag.GetComponent<Image>().color = Color.white;
                    //Debug.Log("headFlag " + headFlag.name + "color " + headFlag.GetComponent<Image>().color);
                    nowCommit = findObj.gameObject;
                    break;
                }
            }
            //Debug.Log("color " + headFlag.GetComponent<Image>().color);
            headFlag = switchFlag;
            // Debug.Log("find! findC.name: " +findC.name + " -- nowCommit: " + localRepository.nowBranch.nowCommit.name);                
            return true;
        }else{
            // Debug.Log("not found: ");
            return false;
        }
    }
    public bool createBranch(string name)
    {
        //bp
        if (localRepository.hasBranch("HEAD")){
            GameObject saveHeadFlag = headFlag;
            deleteBranch("HEAD");
            headFlag = saveHeadFlag;
            headFlag.GetComponent<Image>().enabled = true;
            headFlag.transform.Find("title").GetComponent<Text>().enabled = true;
            headFlag.transform.Find("arrow").GetComponent<Image>().enabled = true;
            
        }else if(localRepository.hasBranch(name)) {
            return false;
        }
        

        localRepository.CreateBranch(name);
        // Debug.Log("find! now branch: " + localRepository.nowBranch.branchName + " -- nowCommit: " + localRepository.nowBranch.nowCommit.name);
        // Debug.Log("headFlag: " + headFlag.name);

        GameObject newFlag;
        newFlag = Instantiate(headFlag, headFlag.transform.parent);

        

        foreach (Transform child in newFlag.transform)
        {
            //Debug.Log("hi, " + child.name);
            if(child.name != "title" && child.name != "arrow")
            Destroy(child.gameObject);
            
            
        }
        newFlag.GetComponent<Image>().color = Color.white;
        newFlag.GetComponentInChildren<Text>().text = name;
        newFlag.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector3(80 - name.Length * 6, -70, 0);
        //Debug.Log("want find:  "+ localRepository.nowBranch.branchName + "_" + localRepository.nowBranch.nowCommit.name);

        Transform nowCommitGameObject = localObjects.transform.Find(localRepository.nowBranch.branchName + "Flag")?.Find(localRepository.nowBranch.nowCommit.name);
        if(nowCommitGameObject != null)
        {
            newFlag.GetComponent<RectTransform>().position = new Vector3(nowCommitGameObject.GetComponent<RectTransform>().position.x, nowCommitGameObject.GetComponent<RectTransform>().position.y - 170, nowCommitGameObject.GetComponent<RectTransform>().position.z);
            // Debug.Log("!null : " + nowCommitGameObject);
        }
        else
        {
            newFlag.GetComponent<RectTransform>().position = new Vector3(headFlag.GetComponent<RectTransform>().position.x, headFlag.GetComponent<RectTransform>().position.y - 170, headFlag.GetComponent<RectTransform>().position.z);
            // Debug.Log("nono");
        }
        newFlag.name = name + "Flag";
        flagObjects.Add(newFlag);
        //Debug.Log("branchUse len:  " + localRepository.nowBranch.nowCommit.branchUsed);
        localRepository.nowBranch.nowCommit.branchUsed++;
        return true;
    }

    public bool deleteBranch(string name)
    {
        if (!localRepository.hasBranch(name))
            return false;

        GameObject deleteFlag = flagObjects.Find(x => x.name == name + "Flag");
        flagObjects.Remove(deleteFlag);
        Destroy(deleteFlag);
        localRepository.deleteBranch(name);

        return true;
    }
    public void SolvedConflict(bool branch,string name)
    {
        conflicted = false;
        if (branch)
        {
            Commit("Merge branch " + name);
            nowCommit.transform.GetChild(2).gameObject.SetActive(true);
        }
        else
        {
            Commit(serverRepository.nowBranch.nowCommit.name);
            fileSystem.NewFile("page2", "");
            trackFile("page2", "init");
        }
    }

    public void stash()
    {
        hasStash = true;
        for(int i =0; i < modifiedFiles.Count; i++)
        {
            stashFiles.Add(modifiedFiles[i]);
        }
        for (int i = 0; i < stashFiles.Count; i++)
        {
            untrackFile(stashFiles[i].Key);
        }
    }
    
    public void pop()
    {
        for (int i = 0; i < stashFiles.Count; i++)
        {
            trackFile(stashFiles[i].Key, stashFiles[i].Value);
        }
        stashFiles.Clear();
        hasStash = false;
    }

    public void rebase(string branch)
    {
        if(headFlag.name.Split("Flag")[0] == branch){
            GameSystemManager.GetSystem<DeveloperConsole>().AddMessageToConsole("Current branch "+ '\"' + branch + '\"' + " is up to date.");
            return;
        }

        List<Commit> oldBranchCommits = localRepository.getBranch(branch).commits;
        for (int i= 0; i < oldBranchCommits.Count; i++)
        {
            Debug.Log(oldBranchCommits[i].name);
        }

        for (int i= 1; i < oldBranchCommits.Count; i++)
        {

            Commit(oldBranchCommits[i], branch);
        }
        for (int i =0; i< commitObjects.Count; i++)
        {
            if(commitObjects[i].name.Contains(branch + "_"))
            {
                GameObject delCommitObj = commitObjects[i];
                commitObjects.Remove(commitObjects[i]);
                Destroy(delCommitObj);
                i--;
            }
        }
        deleteBranch(branch);
    }

    public void tag(string version)
    {
        tagCounts++;
        GameObject newTagObject = Instantiate(exampleTagObject, nowCommit.transform);
        newTagObject.GetComponent<Text>().color = nowCommit.GetComponent<Image>().color;
        newTagObject.GetComponent<Text>().text = version;
        newTagObject.transform.localPosition = new Vector3(95 - newTagObject.GetComponent<Text>().text.Length * 5, -72, 0);
        newTagObject.SetActive(true);
        newTagObject.transform.SetParent(nowCommit.transform.parent);
    }

    public void reset(string mode, string commitId)
    {
        int resetCommitIndex = localRepository.nowBranch.resetCommit(commitId);
        nowCommit = commitObjects[resetCommitIndex];
        headFlag.GetComponent<RectTransform>().localPosition = new Vector3(nowCommit.GetComponent<RectTransform>().localPosition.x - 160, nowCommit.GetComponent<RectTransform>().localPosition.y + 5, headFlag.GetComponent<RectTransform>().localPosition.z);
        int removeCommitIndex = resetCommitIndex + 1;
        while( commitObjects.Count != resetCommitIndex+1)
        {
            GameObject destroyedCommitObject= commitObjects[removeCommitIndex];
            commitObjects.RemoveAt(resetCommitIndex+1);
            Destroy(destroyedCommitObject);
        }
    }

    public void showGitVersion(){
        isCalledGitVersion = true;
    }
}

