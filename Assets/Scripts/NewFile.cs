using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewFile : MonoBehaviour
{
    string fileName;
    string location;
    string content;

    public NewFile(string name, string location, string content)
    {
        fileName = name;
        this.location = location;
        this.content = content;
    }

    public string GetName()
    {
        return fileName;
    }

    public string GetLocation()
    {
        return location;
    }

    public string GetContent()
    {
        return content;
    }
}
