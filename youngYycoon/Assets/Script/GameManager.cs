using CooKing;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Start()
    {
        ControllerManager.Instance.SendMessage<MainController>(new InitMainParm());
    }
}
