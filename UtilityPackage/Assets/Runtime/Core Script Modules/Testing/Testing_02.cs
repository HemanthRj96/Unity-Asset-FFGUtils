using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class Testing_02 : MonoBehaviour
{
    public string path;

    private void Start()
    {
        Debug.Log(File.Exists(path));
    }
}