using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteTransform : MonoBehaviour
{
    public List<Mask> masks = new List<Mask>();
    public List<Image> imgs = new List<Image>();

    public void EnableMask(NoteType type, bool enable)
    {
        switch (type)
        {
            case NoteType.Left:
                masks[0].enabled = enable;
                imgs[0].enabled = enable; 
                break;
            case NoteType.Down:
                masks[1].enabled = enable;
                imgs[1].enabled = enable;
                break;
            case NoteType.Up:
                masks[2].enabled = enable;
                imgs[2].enabled = enable;
                break;
            case NoteType.Right:
                masks[3].enabled = enable;
                imgs[3].enabled = enable;
                break;
        }
    }
}
