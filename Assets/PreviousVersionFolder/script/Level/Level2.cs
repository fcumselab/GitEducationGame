﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2 : Level
{

    private void Start()
    {
        setUp();
        gitSystem.buildRepository();
    }
    // Update is called once per frame
    void Update()
    {
        if ( !targetSystem.targetStatus[0] && gitSystem.isCalledGitVersion )
        {
            targetSystem.targetStatus[0] = true;
            targetSystem.AccomplishTarget(0);
        }

        if ( !targetSystem.targetStatus[1] && gitSystem.hasRepository()  )
        {
            targetSystem.targetStatus[1] = true;
            targetSystem.AccomplishTarget(1);
        }
        else if ( !gitSystem.hasRepository() )
        {
            targetSystem.targetStatus[1] = false;
            targetSystem.UndoTarget(1);
        }
        updateTarget();
        levelCostCount();
    }
}
