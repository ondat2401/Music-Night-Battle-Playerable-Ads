using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "MapConfig",menuName = "Config/MapConfig",order = 0)]
public class MapConfig : ScriptableObject
{
    public List<MapData> mapDatas = new List<MapData>();
    public NotePrefab arrowPrefab;
    public ComboPrefab comboPrefab;

    public MapData FindMapById(int id)
    {
        var map = mapDatas.Find(m => m.mapId == id);
        return map != null ? map : mapDatas.Last();
    }
    public Vector2 GetArrowPositionByType()
    {
        return new Vector2(0, 830);
    }
    public void ParseMap()
    {
        foreach (var mapData in mapDatas)
        {
            if (mapData.jsonData == null) continue;
            mapData.noteList.Clear();
            string jsonContent = mapData.jsonData.text;
            NoteListWrapper wrapper = JsonUtility.FromJson<NoteListWrapper>(jsonContent);
            if (wrapper == null)
            {
                return;
            }
            mapData.noteList = wrapper.list;
            foreach (var data in mapData.noteList)
            {
                data.isLongNote = data.duration > 0.1f;
                switch (data.noteID)
                {
                    case 72: 
                        data.arrowType = NoteType.Left; 
                        data.isPlayer = true;
                        break;
                    case 73:
                        data.arrowType = NoteType.Down;
                        data.isPlayer = true;
                        break;
                    case 74:
                        data.arrowType = NoteType.Up;
                        data.isPlayer = true;
                        break;
                    case 75:
                        data.arrowType = NoteType.Right;
                        data.isPlayer = true;
                        break;
                    case 76:
                        data.arrowType = NoteType.Left;
                        data.isPlayer = false;
                        break;
                    case 77:
                        data.arrowType = NoteType.Down;
                        data.isPlayer = false;
                        break;
                    case 78:
                        data.arrowType = NoteType.Up;
                        data.isPlayer = false;
                        break;
                    case 79:
                        data.arrowType = NoteType.Right;
                        data.isPlayer = false;
                        break;
                }
            }
        }
    }
    public void InitArrow(NoteBase arrow, NoteType type, bool isPlayer)
    {
        switch (type)
        {
            case NoteType.Left: arrow.img.sprite = isPlayer ? arrowPrefab.arrowSprites[0] : arrowPrefab.arrowSprites[4]; break;
            case NoteType.Down: arrow.img.sprite = isPlayer ? arrowPrefab.arrowSprites[1] : arrowPrefab.arrowSprites[5]; break;
            case NoteType.Up: arrow.img.sprite = isPlayer ? arrowPrefab.arrowSprites[2] : arrowPrefab.arrowSprites[6]; break;
            case NoteType.Right: arrow.img.sprite = isPlayer ? arrowPrefab.arrowSprites[3] : arrowPrefab.arrowSprites[7]; break;
        }
        arrow.type = type;
    }
    public void InitArrowLong(NoteBase arrow, NoteType type, bool isPlayer)
    {
        switch (type)
        {
            case NoteType.Left: arrow.img.sprite = isPlayer ? arrowPrefab.arrowSprites[0] : arrowPrefab.arrowSprites[4];
                arrow.longImg.sprite = isPlayer ? arrowPrefab.arrowLongSprites[0] : arrowPrefab.arrowLongSprites[4];
                break;
            case NoteType.Down: arrow.img.sprite = isPlayer ? arrowPrefab.arrowSprites[1] : arrowPrefab.arrowSprites[5]; 
                arrow.longImg.sprite = isPlayer ? arrowPrefab.arrowLongSprites[1] : arrowPrefab.arrowLongSprites[4];                
                break;
            case NoteType.Up: arrow.img.sprite = isPlayer ? arrowPrefab.arrowSprites[2] : arrowPrefab.arrowSprites[6]; 
                arrow.longImg.sprite = isPlayer ? arrowPrefab.arrowLongSprites[2] : arrowPrefab.arrowLongSprites[4];                                
                break;
            case NoteType.Right: arrow.img.sprite = isPlayer ? arrowPrefab.arrowSprites[3] : arrowPrefab.arrowSprites[7]; 
                arrow.longImg.sprite = isPlayer ? arrowPrefab.arrowLongSprites[3] : arrowPrefab.arrowLongSprites[4];      
                break;
        }
        arrow.type = type;
    }
    public void InitCombo(Combo combo, ComboType type)
    {
        switch (type)
        {
            case ComboType.Bad: combo.img.sprite = comboPrefab.comboSprites[0]; break;
            case ComboType.Good: combo.img.sprite = comboPrefab.comboSprites[1]; break;
            case ComboType.Sick: combo.img.sprite = comboPrefab.comboSprites[2]; break;

        }
    }
}
[System.Serializable]
public class MapData
{
    public TextAsset jsonData;
    public string mapName;
    public int mapId;
    public float noteDelay;
    public float mapSample;
    public AudioClip mapAudio;

    [Header("Data")]
    public List<NoteData> noteList;

  
}

[System.Serializable]
public class NotePrefab
{
    public NoteBase prefab;
    public List<Sprite> arrowSprites = new List<Sprite>();
    public List<Sprite> arrowLongSprites = new List<Sprite>();

}
[System.Serializable]
public class ComboPrefab
{
    public Combo prefab;
    public List<Sprite> comboSprites = new List<Sprite>();

}
public enum NoteType
{
    Left, Down, Up, Right
}
public enum ComboType
{
    Bad, Good, Sick
}
[System.Serializable]
public class NoteData
{
    public NoteType arrowType;
    public float timeAppear;
    public int noteID;
    public float duration;
    public bool isPlayer;
    public bool isLongNote;
}
[System.Serializable]
public class NoteListWrapper
{
    public List<NoteData> list;
}