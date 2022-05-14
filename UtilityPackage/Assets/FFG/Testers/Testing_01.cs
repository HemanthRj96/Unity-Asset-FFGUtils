using FFG;
using UnityEngine;

public class Testing_01 : MonoBehaviour
{
    public GlobalEventComponent component_1;
    public GlobalEventComponent component_2;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            GlobalEvents.InvokeEvent($"Event-Test-{Random.Range(1, 3)}");
        if (Input.GetKeyDown(KeyCode.G))
            GlobalEvents.InvokeEvent($"Event-Test-{Random.Range(1, 3)}", 69);
        if (Input.GetKeyDown(KeyCode.H))
            GlobalEvents.InvokeEvent($"Event-Test-{Random.Range(1, 3)}", "Hello world!", gameObject);
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (component_1)
                Destroy(component_1.gameObject);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (component_2)
                Destroy(component_2.gameObject);
        }
    }

    public void TestNoParamEvent_1()
    {
        Debug.Log("#1 No parameter test pass!");
    }

    public void TestSingleParamEvent_1(object data)
    {
        Debug.Log($"#1 Single parameter test pass! Data : {(int)data}");
    }

    public void TestDoubleParamEvent_1(object data, GameObject instigator)
    {
        Debug.Log($"#1 Double parameter test pass! Data : {(string)data}, Instigator : {instigator.name}");
    }

    public void TestNoParamEvent_2()
    {
        Debug.Log("#2 No parameter test pass!");
    }

    public void TestSingleParamEvent_2(object data)
    {
        Debug.Log($"#2 Single parameter test pass! Data : {(int)data}");
    }

    public void TestDoubleParamEvent_2(object data, GameObject instigator)
    {
        Debug.Log($"#2 Double parameter test pass! Data : {(string)data}, Instigator : {instigator.name}");
    }
}
