using UnityEngine;

public class Action100 : GameAction
{
    private string content;
    private ActionResult result = new ActionResult();

    public Action100()
        : base((int)100)
    {
    }

    protected override void SendParameter(NetWriter writer, ActionParam actionParam)
    {

    }

    protected override void DecodePackage(NetReader reader)
    {

    }

    public override ActionResult GetResponseData()
    {
        return result;
    }
}
