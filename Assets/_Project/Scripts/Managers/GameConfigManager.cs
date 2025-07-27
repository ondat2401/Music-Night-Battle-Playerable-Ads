using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfigManager : SingletonModule<GameConfigManager>
{
    protected override bool DontDestroy => true;
    public MapConfig mapConfig;
}
