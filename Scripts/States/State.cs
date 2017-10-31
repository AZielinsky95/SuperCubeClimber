using UnityEngine;
using System.Collections;

public abstract class State : MonoBehaviour
{
    public abstract void Begin();
    public abstract void End();
    public abstract void OnUpdate();

    public override string ToString()
    {
        return this.GetType().ToString();
    }
}
