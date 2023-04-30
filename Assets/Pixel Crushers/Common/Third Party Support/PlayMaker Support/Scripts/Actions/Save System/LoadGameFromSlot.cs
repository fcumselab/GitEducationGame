﻿// Copyright (c) Pixel Crushers. All rights reserved.

using UnityEngine;
using HutongGames.PlayMaker;

namespace PixelCrushers.PlayMakerSupport
{

    [ActionCategory("Pixel Crushers Common")]
    [HutongGames.PlayMaker.Tooltip("Loads the game that was saved in a slot.")]
    public class LoadGameFromSlot : FsmStateAction
    {

        [HutongGames.PlayMaker.Tooltip("Saved game slot.")]
        public FsmInt slot = new FsmInt();

        public override void Reset()
        {
            slot.Value = 0;
        }

        public override void OnEnter()
        {
            SaveSystem.LoadFromSlot(slot.Value);
            Finish();
        }

    }

}