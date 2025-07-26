
using UnityEngine;

public class ChaseUnitEffect : Effect
{
    public override void DoEffect()
    {
        Debug.Log("Chase");
    }
    
    
}


public class GetMoney : Effect
{
    public int amount;

    public override void DoEffect()
    {
        Debug.Log("GetMoney " + amount);
    }
}