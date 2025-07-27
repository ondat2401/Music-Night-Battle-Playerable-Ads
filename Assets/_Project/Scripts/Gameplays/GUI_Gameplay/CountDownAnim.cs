using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDownAnim : MonoBehaviour
{
    [SerializeField] Animator anim;

    public void TriggerCountDown(int count)
    {
        anim.SetTrigger($"CountDown_{count}");
    }
}
