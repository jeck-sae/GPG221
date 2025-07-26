
using UnityEngine;

public class ChaseUnitEffect : Effect
{
    public override void DoEffect()
    {
        Debug.Log("Chase");
    }

    public override void SetupNode(ActionNode node)
    {
        node.AddTextField("Chase", x=>Debug.Log("Chase"));
    }
}


public class GetMoneyEffect : Effect
{
    public int amount;

    public override void DoEffect()
    {
        Debug.Log("GetMoney " + amount);
    }

    public override void SetupNode(ActionNode node)
    {
        node.AddTextField("Amount", x=> {
            if (int.TryParse(x.newValue, out int newAmount))
                amount = newAmount;
        });
    }
}




public class MoneyCondition : Condition
{
    public int minMoney;
    public override bool CheckCondition()
    {
        return false;
    }

    public override void SetupNode(PrerequisiteNode node)
    {
        node.AddTextField("Min money", x=> {
            if (int.TryParse(x.newValue, out int newAmount))
                minMoney = newAmount;
        });
    }
}