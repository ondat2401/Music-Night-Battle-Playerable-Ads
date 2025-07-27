using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;
using DG.Tweening;
public class GameplayManager : SingletonModule<GameplayManager>
{
    protected override bool DontDestroy => true;
    //private bool gameStart = false;
    public int currentMap = 0;
    public float timer = 0;
    public int missedNote;
    private MapData mapData = null;
    public Animator enemy_AC;
    public Animator player_AC;
    //[HideInInspector]
    public List<NoteBase> arrowList = new List<NoteBase>();
    public List<NoteButton> buttons = new List<NoteButton>(); // left - bot - top - right

    [Header("FlowGame")]
    Coroutine initGameCoroutine = null;
    Tween endGameTween = null;
    private void Start()
    {
        timer = 0;

    }
    private void Update()
    {
        if(missedNote == 4)
        {
            missedNote = 0;
            EndGame(true);
        }
    }
    public void StartGame()
    {
        missedNote = 0;
        initGameCoroutine = StartCoroutine(InitGame());
        endGameTween = DOVirtual.DelayedCall(30, () =>
        {
            EndGame();
        });
    }
    private void EndGame(bool stopBGM = false)
    {
        GUIManager.Instance.GUI_Gameplay.EndGame();
        if (stopBGM)
        {
            AudioManager.Instance.StopBGM();
        }
        endGameTween.Kill();
        StopCoroutine(initGameCoroutine);
        for (int i = arrowList.Count - 1; i >= 0; i--)
        {
            arrowList[i].KillTween();
        }
        arrowList.Clear();
    }
    private IEnumerator InitGame()
    {

        yield return StartCoroutine(LoadMapRoutine(currentMap));

        yield return CountDown();
        yield return SpawnArrowsCoroutine();
        //gameStart = true;
    }
    private IEnumerator LoadMapRoutine(int currentMap)
    {
        var mapConfig = GameConfigManager.Instance.mapConfig.FindMapById(currentMap);
        mapData = mapConfig;
        yield return null;
    }
    private IEnumerator CountDown()
    {
        WaitForSeconds oneSec = new WaitForSeconds(.45f);
        for (int i = 3; i >= 0; i--)
        {
            GUIManager.Instance.GUI_Gameplay.CountDown(i);
            AudioManager.Instance.PlaySFX(i);
            if(i > 0)
            {
                yield return oneSec;
            }
        }
    }
    private IEnumerator SpawnArrowsCoroutine()
    {

        var mapConfig = GameConfigManager.Instance.mapConfig;
        var currentMap = mapConfig.FindMapById(this.currentMap);
        var arrowDataList = currentMap.noteList;

        float currentTime = 0f;
        int index = 0;
        DOVirtual.DelayedCall(currentMap.noteDelay, () =>
        {
            AudioManager.Instance.PlayBGM(mapData.mapAudio);
        });
        while (index < arrowDataList.Count)
        {
            currentTime += Time.deltaTime;
            float spawnTime = arrowDataList[index].timeAppear - currentMap.noteDelay;
            if (currentTime >= spawnTime)
            {
                var arrow = SpawnArrow(arrowDataList[index].arrowType, arrowDataList[index].isPlayer, arrowDataList[index].isLongNote);
                arrow.Move(currentMap.mapSample, arrowDataList[index].isPlayer);
                index++;
            }

            yield return null;
        }
    }

    private NoteBase SpawnArrow(NoteType type, bool isPlayer, bool isLongNote)
    {
        var arrow = PoolManager.Instance.GetArrowFromPool(type, isPlayer, isLongNote);
        if(isPlayer)
            arrowList.Add(arrow);
        return arrow;
    }
    public void RemoveArrowFromList(NoteBase arrow)
    {
        if (arrowList.Any(a => a == arrow)) 
            arrowList.Remove(arrow);
    }


    public IEnumerator CheckArrowWithinTime(NoteType btnArrowType, NoteButton arrowBtn, float duration)
    {
        float timer = 0f;
        float checkInterval = 0.02f;
        NoteBase currentArrow = null;
        bool isNoteKilled = false;

        while (timer < duration)
        {
            var arrows = arrowList
                .Where(a => a.type == btnArrowType && GetCenterDistance(a.rectTransform, arrowBtn.rectTransform) < 1)
                .ToList();

            if (arrows.Count > 0)
            {
                var validArrow = arrows[0];

                if (validArrow.isLong && arrowBtn.isHolding)
                {
                    PoolManager.Instance.arrowTransform.EnableMask(validArrow.type, true);
                }
                currentArrow = validArrow;


            }
            if (!arrowBtn.isHolding && !isNoteKilled)
            {
                KillNote(btnArrowType, arrowBtn, currentArrow);
                isNoteKilled = true;
            }

            timer += checkInterval;
            yield return new WaitForSeconds(checkInterval);
        }
        if (!isNoteKilled && currentArrow != null)
        {
            if (currentArrow.isLong)
            {
                KillNote(btnArrowType, arrowBtn, currentArrow,ComboType.Sick);
            }
            else
            {
                KillNote(btnArrowType, arrowBtn, currentArrow);
            }
   
        }
    }

    private void KillNote(NoteType btnArrowType, NoteButton arrowBtn, NoteBase currentArrow, ComboType forceType = ComboType.Bad)
    {
        if (currentArrow != null)
        {
            currentArrow.KillTween();
            string anim = GetCharacterAnim(btnArrowType);
            player_AC.SetTrigger(anim);

            GUIManager.Instance.GUI_Gameplay.AddScore();
            SpawnCombo(currentArrow.rectTransform, arrowBtn.rectTransform, forceType);
            PoolManager.Instance.arrowTransform.EnableMask(currentArrow.type, false);

        }
    }
    private void SpawnCombo(RectTransform a, RectTransform b, ComboType forceType = ComboType.Bad)
    {
        ComboType type = ComboType.Bad;
        if (forceType != ComboType.Bad)
        {
            type = forceType;
        }
        var distance = GetCenterDistance(a, b);
        if (distance <= 0.3f)
        {
            type = ComboType.Sick;
        }
        else if (distance <= 0.75f)
        {
            type = ComboType.Good;
        }

        PoolManager.Instance.GetComboFromPool(type);

    }
    public string GetCharacterAnim(NoteType type)
    {
        switch (type)
        {
            case NoteType.Up:
                return "Up";
            case NoteType.Down:
                return "Down"; 
            case NoteType.Right:
                return "Right";
            case NoteType.Left:
                return "Left"; 
            default:
                return "";
        }
    }
    private float GetCenterDistance(RectTransform a, RectTransform b)
    {
        if (a == null || b == null) return float.MaxValue;

        Vector3 centerA = GetWorldCenter(a);
        Vector3 centerB = GetWorldCenter(b);

        return Vector3.Distance(centerA, centerB);
    }
    private Vector3 GetWorldCenter(RectTransform rt)
    {
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);
        return (corners[0] + corners[2]) / 2f;
    }

}
