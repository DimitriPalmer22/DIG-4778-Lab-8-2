using System.IO;
using UnityEngine;

public interface ISaveable
{
    string SaveID { get; }

    public void Load(ISaveDataToken token);
}