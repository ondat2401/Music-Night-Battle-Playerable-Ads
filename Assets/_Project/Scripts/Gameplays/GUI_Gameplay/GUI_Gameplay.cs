using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GUI_Gameplay : MonoBehaviour
{
    [SerializeField] CountDownAnim countDownAnim;
    [SerializeField] GameObject scoreBar;
    [SerializeField] Image fillScoreBar;
    [SerializeField] Image scoreIcon;
    [SerializeField] GameObject PlayBtn;
    [SerializeField] GameObject RestartBtn;
    [SerializeField] GameObject ArrowButtons;
    [SerializeField] GameObject Logo;
    [SerializeField] List<GameObject> stars = new List<GameObject>();

    public float score;
    public float maxScore;
    public void Init()
    {
        score = 0;
        scoreIcon.transform.localPosition = new Vector3(-445, -90, 0);
        scoreBar.SetActive(false);
        fillScoreBar.fillAmount = 0;
        PlayBtn.SetActive(true);
        RestartBtn.SetActive(false);
        ArrowButtons.SetActive(false);
        Logo.SetActive(true);

        foreach (var star in stars) star.gameObject.SetActive(true);

        maxScore = GameConfigManager.Instance.mapConfig.FindMapById(GameplayManager.Instance.currentMap).noteList.Where(note => note.isPlayer).ToList().Count;
    }
    public void StartGame()
    {
        scoreBar.SetActive(true);
        PlayBtn.SetActive(false);
        Logo.SetActive(false);

        ArrowButtons.SetActive(true);
    }
    public void EndGame()
    {
        ArrowButtons.SetActive(false);
        RestartBtn.SetActive(true);
    }
    public void CountDown(int count)
    {
        countDownAnim.TriggerCountDown(count);
    }
    public void Button_Start()
    {
        Init();
        StartGame();
        GameplayManager.Instance.StartGame();
    }
    public void Button_Replay()
    {
        Button_Start();
        PoolManager.Instance.arrowTransform.gameObject.SetActive(true);
        RestartBtn.SetActive(false );
    }
    public void AddScore()
    {
        score++;
        fillScoreBar.fillAmount = score / maxScore;

        float minX = -445f;
        float maxX = 445f;
        float percent = Mathf.Clamp01(score / maxScore);
        float newX = Mathf.Lerp(minX, maxX, percent);
        scoreIcon.transform.localPosition = new Vector3(newX, scoreIcon.transform.localPosition.y, scoreIcon.transform.localPosition.z);

        if(score / maxScore >= 0.99f) stars[2].SetActive(false);
        else if(score / maxScore >= 0.66f) stars[1].SetActive(false);
        else if(score / maxScore >= 0.33f) stars[0].SetActive(false);
    }
}

