using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Team
{
    public string name;
    public Color colour;
    public Transform[] spawnPoints;
    public GameObject botPrefab;
    public string[] tagOfEnemy;
    public Transform[] botsPath;
}
