using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoolManager : SingletonModule<PoolManager>
{
    protected override bool DontDestroy => true;

    private Queue<NoteBase> arrowQueue = new Queue<NoteBase>();
    private Queue<Combo> comboQueue = new Queue<Combo>();
    public NoteTransform arrowTransform;
    [SerializeField] Transform comboTransform;
    #region Arrow
    public void AddArrowToPool(NoteBase arrow)
    {
        GameplayManager.Instance.RemoveArrowFromList(arrow);
        arrow.gameObject.SetActive(false);
        DOVirtual.DelayedCall(2, () =>
        {
            arrowQueue.Enqueue(arrow);
        });
    }
    public NoteBase GetArrowFromPool(NoteType type, bool isPlayer, bool isLongNote)
    {
        var mapConfig = GameConfigManager.Instance.mapConfig;
        NoteBase arrow = null;

        if (arrowQueue.Count > 0 && !isLongNote)
        {
            arrow = arrowQueue.Dequeue();
            arrow.gameObject.SetActive(true);
        }
        else
        {
            arrow = Instantiate(mapConfig.arrowPrefab.prefab);
        }
        if (isLongNote)
        {
            arrow.longImg.gameObject.SetActive(true);
            mapConfig.InitArrowLong(arrow, type, isPlayer);
        }
        else
        {
            arrow.longImg.gameObject.SetActive(false);
            mapConfig.InitArrow(arrow, type, isPlayer);
        }
        arrow.img.gameObject.SetActive(true);
        arrow.isLong = isLongNote;
        arrow.transform.SetParent(GetArrowParent(type),true);
        var arrowPos = mapConfig.GetArrowPositionByType();
        arrow.transform.localPosition = arrowPos;
        arrow.transform.localScale = Vector3.one;


        return arrow;
    }
    private Transform GetArrowParent(NoteType type)
    {
        switch (type)
        {
            case NoteType.Left:
                return arrowTransform.masks[0].transform;
            case NoteType.Down:
                return arrowTransform.masks[1].transform;
            case NoteType.Up:
                return arrowTransform.masks[2].transform;
            case NoteType.Right:
                return arrowTransform.masks[3].transform;
            default:
                return arrowTransform.masks[0].transform;
        }
    }
    #endregion
    #region Combo
    public void AddComboToPool(Combo combo)
    {
        combo.gameObject.SetActive(false);
        DOVirtual.DelayedCall(2, () =>
        {
            comboQueue.Enqueue(combo);
        });
    }
    public Combo GetComboFromPool(ComboType type)
    {
        var mapConfig = GameConfigManager.Instance.mapConfig;
        Combo combo = null;

        if (comboQueue.Count > 0)
        {
            combo = comboQueue.Dequeue();
            combo.gameObject.SetActive(true);
        }
        else
        {
            combo = Instantiate(mapConfig.comboPrefab.prefab, comboTransform);
        }
        mapConfig.InitCombo(combo, type);
        combo.transform.localPosition = Vector3.zero;
        return combo;
    }
    #endregion
}
