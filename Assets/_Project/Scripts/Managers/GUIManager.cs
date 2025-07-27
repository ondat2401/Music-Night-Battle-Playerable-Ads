using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIManager : SingletonModule<GUIManager>
{
    protected override bool DontDestroy => true;
    public GUI_Gameplay GUI_Gameplay;
    private void Start()
    {
        GUI_Gameplay.Init();
    }
}
