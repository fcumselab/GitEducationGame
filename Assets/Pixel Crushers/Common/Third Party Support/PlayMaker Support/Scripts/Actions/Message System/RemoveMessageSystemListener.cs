﻿// Copyright (c) Pixel Crushers. All rights reserved.

using UnityEngine;
using HutongGames.PlayMaker;

namespace PixelCrushers.PlayMakerSupport
{

    [ActionCategory("Pixel Crushers Common")]
    [HutongGames.PlayMaker.Tooltip("Removes a Message System listener.")]
    public class RemoveMessageSystemListener : FsmStateAction
    {

        [HutongGames.PlayMaker.TooltipAttribute("GameObject that will no longer listen for messages.")]
        public FsmOwnerDefault listener;

        [HutongGames.PlayMaker.TooltipAttribute("Message to listen for.")]
        public FsmString message = new FsmString();

        [HutongGames.PlayMaker.TooltipAttribute("Optional message parameter.")]
        public FsmString parameter = new FsmString();

        public override void Reset()
        {
            listener = null;
            message.Value = string.Empty;
            parameter.Value = string.Empty;
        }

        public override void OnEnter()
        {
            var target = Fsm.GetOwnerDefaultTarget(listener);
            IMessageHandler handler = (target == null) ? null : target.GetComponent(typeof(IMessageHandler)) as IMessageHandler;
            if (handler == null)
            {
                LogError("Listener '" + target + "' doesn't have a component that can handle messages.");
            }
            else
            {
                MessageSystem.RemoveListener(handler, message.Value, parameter.Value);
            }
            Finish();
        }

    }

}