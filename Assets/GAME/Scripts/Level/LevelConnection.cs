using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LevelConnection : ScriptableObject
{
    public static LevelConnection ActiveConnection { get; set; }
}
