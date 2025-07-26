using Sirenix.OdinInspector;
using UnityEngine;

public class GOAP : SerializedMonoBehaviour
{
    public GoapAction rootAction;
    
    public void DoAction(GoapAction action)
    {
        action.TryAction();
    }
    
}