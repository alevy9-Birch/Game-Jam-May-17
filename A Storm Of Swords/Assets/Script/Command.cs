using UnityEngine;

public abstract class Command : MonoBehaviour
{
    public string abilityName;

    public void Start()
    {
        Selectable selectable = gameObject.GetComponent<Selectable>();
        selectable.AddCommand(abilityName, this);

        CommandInitialize();
    }

    protected abstract void CommandInitialize();
    public abstract bool Execute();
    public abstract Command Duplicate();
}
