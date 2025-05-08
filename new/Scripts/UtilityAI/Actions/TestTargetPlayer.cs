using NoOpArmy.WiseFeline;
using NoOpArmy.WiseFeline.BlackBoards;
using UnityEngine;

namespace Insolence 
{
    public class TestTargetPlayer : ActionBase
    {
        BlackBoard bb;
        GameObject target;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            target = GameObject.FindWithTag("Player");
            bb = Brain.GetComponent<BlackBoard>();
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
            bb.SetGameObject("target", target);
        }

        protected override void UpdateTargets()
        {
            AddTarget(target.transform);
        }
    }
}


