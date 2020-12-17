using UnityEngine;
using com.ootii.Messages;

namespace HutongGames.PlayMaker.Actions
{

    [ActionCategory("VRActions")]
    public class RecieveSystemWsMessages : FsmStateAction
    {
        public FsmGameObject MessageObject;
        public FsmString FsmEventName;
        // Code that runs on entering the state.
        public FsmString id_name;
        public FsmString name_name;
        public FsmString kind_name;
        public FsmString changenum_name;
        public FsmString a_name;
        public FsmString b_name;
        public FsmString c_name;
        public FsmString d_name;
        public FsmString e_name;

        public override void OnEnter()
        {
            MessageDispatcher.AddListener(WsMessageType.RecieveChangeObj.ToString(), RecieveChangeObj);
        }

        // Code that runs when exiting the state.
        public override void OnExit()
        {
            MessageDispatcher.RemoveListener(WsMessageType.RecieveChangeObj.ToString(), RecieveChangeObj);
        }
        void RecieveChangeObj(IMessage msg)
        {
            WsChangeInfo rinfo = msg.Data as WsChangeInfo;
            MessageObject.Value.GetComponent<PlayMakerFSM>().Fsm.Variables.FindFsmString(id_name.Value).Value = rinfo.id;
            MessageObject.Value.GetComponent<PlayMakerFSM>().Fsm.Variables.FindFsmString(name_name.Value).Value = rinfo.name;
            MessageObject.Value.GetComponent<PlayMakerFSM>().Fsm.Variables.FindFsmString(kind_name.Value).Value = rinfo.kind;
            MessageObject.Value.GetComponent<PlayMakerFSM>().Fsm.Variables.FindFsmString(changenum_name.Value).Value = rinfo.changenum;

            MessageObject.Value.GetComponent<PlayMakerFSM>().Fsm.Variables.FindFsmString(a_name.Value).Value = rinfo.a;
            MessageObject.Value.GetComponent<PlayMakerFSM>().Fsm.Variables.FindFsmString(b_name.Value).Value = rinfo.b;
            MessageObject.Value.GetComponent<PlayMakerFSM>().Fsm.Variables.FindFsmString(c_name.Value).Value = rinfo.c;
            MessageObject.Value.GetComponent<PlayMakerFSM>().Fsm.Variables.FindFsmString(d_name.Value).Value = rinfo.d;
            MessageObject.Value.GetComponent<PlayMakerFSM>().Fsm.Variables.FindFsmString(e_name.Value).Value = rinfo.e;
            
            MessageObject.Value.GetComponent<PlayMakerFSM>().SendEvent(FsmEventName.Value);
        }
    }
}
