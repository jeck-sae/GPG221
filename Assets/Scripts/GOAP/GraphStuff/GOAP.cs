using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class GOAP : SerializedMonoBehaviour
{
    public GoapAction rootAction;
    
    public GoapAction currentRootAction;
    Coroutine behaviourLoop;
    
    public void SetBehaviour(GoapDataScriptableObject behaviourData)
    {
        if(behaviourLoop != null)
            StopCoroutine(behaviourLoop);
        
        currentRootAction = behaviourData.GetRootAction();
    }

    public IEnumerator PlayTurn()
    {
        yield return currentRootAction.GetEffect().nextEffect;
    }

    /*IEnumerator BehaviourLoop()
    {
        while (true)
        {
            var result = currentRootAction.GetEffect();
            if (result.success == true)
            {
                yield return result.nextEffect.DoEffect();
                yield break;
            }

            while (result.nextEffect == null)
            {
                yield return null;
                continue;
            }
            
            yield return result.nextEffect.DoEffect();
        }
    }*/
    
}