using UnityEngine;

public class EndGame : MonoBehaviour
{
    [SerializeField] private GameObject winObj;
    [SerializeField] private GameObject loseObj;

    public void SetEndGame(bool isWin)
    {
        winObj.gameObject.SetActive(isWin);
        loseObj.gameObject.SetActive(!isWin);
    }
}
