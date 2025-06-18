using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelConnection", menuName = "Custom/Level Connection")]
public class LevelConnection : ScriptableObject
{
    public static LevelConnection ActiveConnection { get; set; }

    // You can add fields here for your level connection data
    // public string connectionName;
    // public int levelID;
}
