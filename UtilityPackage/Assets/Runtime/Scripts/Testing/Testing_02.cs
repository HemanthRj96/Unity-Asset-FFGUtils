using FickleFrames.ActionSystem;
using UnityEngine;


public class Testing_02 : MonoBehaviour
{
    private void Awake()
    {
        ActionManager.RegisterAction(action, "Appu");
    }

    private void action(IActionParameters parameters)
    {
        string data = (string)parameters.data;
        Debug.Log(data + $" in {gameObject.name}");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            ActionManager.ExecuteAction("Hemanth", $"Data from {gameObject.name}");
    }
}
