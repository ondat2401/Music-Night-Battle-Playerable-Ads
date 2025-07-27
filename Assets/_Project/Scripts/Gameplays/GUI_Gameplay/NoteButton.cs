using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NoteButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public RectTransform rectTransform;
    public GameObject effect;
    [SerializeField] NoteType type;
    [SerializeField] Animator anim;
    public bool isHolding;


    public void OnPointerDown(PointerEventData eventData)
    {
        OnClick();
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        OnKeyUp();
    }
    public void OnClick()
    {
        isHolding = true;
        effect.SetActive(true);
        anim.SetTrigger("Hold");

        StartCoroutine(GameplayManager.Instance.CheckArrowWithinTime(type, this, 0.5f));
    }
    public void OnKeyUp()
    {

        isHolding = false;
        anim.SetTrigger("Up");

    }


    private void Update()
    {
        switch (type)
        {
            case NoteType.Left:
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    OnClick();
                }
                if (Input.GetKeyUp(KeyCode.LeftArrow))
                {
                    OnKeyUp();
                }
                break;
            case NoteType.Down:
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    OnClick();
                }
                if (Input.GetKeyUp(KeyCode.DownArrow))
                {
                    OnKeyUp();
                }
                break;
            case NoteType.Up:
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    OnClick();
                }
                if (Input.GetKeyUp(KeyCode.UpArrow))
                {
                    OnKeyUp();
                }
                break;
            case NoteType.Right:
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    OnClick();
                }
                if (Input.GetKeyUp(KeyCode.RightArrow))
                {
                    OnKeyUp();
                }
                break;
        }
    }


}
