using System.Collections;
using UnityEngine;

public class ChaseUnitEffect : Effect
{
    public override IEnumerator DoEffect()
    {
        Debug.Log("Chase");
        yield break;
    }

    public override void SetupNode(GoapNode node)
    {
        node.title = "Chase";
        node.AddTextField("Chase", x=>Debug.Log("Chase"));
    }
}


public class GetMoneyEffect : Effect
{
    public int amount;

    public override IEnumerator DoEffect()
    {
        Debug.Log("GetMoney " + amount);
        yield break;
    }

    public override void SetupNode(GoapNode node)
    {
        node.title = "GetMoney";
        node.AddTextField("Amount", x=> {
            if (int.TryParse(x.newValue, out int newAmount))
                amount = newAmount;
        }).value = amount.ToString();
        
    }
}




public class MoneyCondition : Condition
{
    public int minMoney;
    public override bool CheckCondition()
    {
        return false;
    }

    public override void SetupNode(GoapNode node)
    {
        node.title = "Check Money";
        node.AddTextField("Min money", x=> {
            if (int.TryParse(x.newValue, out int newAmount))
                minMoney = newAmount;
        }).value = minMoney.ToString();
    }
}