using UnityEngine;

public class ActionParameters : IActionParameters
{
    public ActionParameters(object data = null, GameObject source = null)
    {
        this.data = data;
        this.source = source;
    }

    public object data { get; set; }
    public GameObject source { get; set; }
}