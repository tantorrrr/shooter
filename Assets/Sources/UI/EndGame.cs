using UnityEngine;

public class EndGame : MonoBehaviour
{
    [SerializeField] private GameObject winObj;
    [SerializeField] private GameObject loseObj;
    [SerializeField] private GameObject replayBtn;
    [SerializeField] private GameObject nextLevelBtn;

    public void SetEndGame(bool isWin)
    {
        winObj.gameObject.SetActive(isWin);
        nextLevelBtn.gameObject.SetActive(isWin);

        loseObj.gameObject.SetActive(!isWin);
        replayBtn.gameObject.SetActive(!isWin);
    }
}
