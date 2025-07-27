using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class NoteBase : MonoBehaviour
{
    private Tween moveTween;
    public RectTransform rectTransform;
    public Image img;
    public Image longImg;
    public NoteType type;
    public bool isLong = false;
    public void Move(float timeAppear, bool isPlayer)
    {
        moveTween?.Kill();
        string anim = "";
        switch (type)
        {
            case NoteType.Up:
                anim = "Up";break;
            case NoteType.Down:
                anim = "Down"; break;
            case NoteType.Right:
                anim = "Right"; break;
            case NoteType.Left:
                anim = "Left"; break;
        }
        float targetY = -820;
        float distance = Mathf.Abs(transform.localPosition.y - targetY);
        float time = distance / timeAppear;
        moveTween = transform.DOLocalMoveY(-820, time).SetEase(Ease.Linear).OnComplete(() =>
        {
            if (isPlayer)
            {
                targetY = -1700;
                distance = Mathf.Abs(transform.localPosition.y - targetY);
                time = distance / timeAppear;
                moveTween = transform.DOLocalMoveY(-1700, time).SetEase(Ease.Linear).OnComplete(() =>
                {
                    PoolManager.Instance.AddArrowToPool(this);
                    AudioManager.Instance.PlaySFX(AudioName.SFX_Failed);
                    GameplayManager.Instance.missedNote++;
                });
            }
            else
            {
                
                GameplayManager.Instance.enemy_AC.SetTrigger(anim);
                PoolManager.Instance.AddArrowToPool(this);
            }
        });
    }
    public void KillTween()
    {
        moveTween?.Kill();
        PoolManager.Instance.AddArrowToPool(this);
    }
}
