using System.IO;
using UnityEngine;

public interface ISaveable
{
    string SaveID { get; }

    public void Load(string path);
}