using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using CooKing;
using TimeExtension;

public class Cook : MonoBehaviour
{
    [SerializeField] TMP_Text _txtIngredient;
    [SerializeField] TMP_Text _leftSeconds;
    [SerializeField] Button _btnCompletedCook;
    UnityAction _afterCookAction;

    int _leftSec = 0;
    public enum Status
    {
        Empty,
        Cooking,
        Completed
    }
    Status _status;
    public Status GetStatus()
    {
        return _status;
    }

    public void StartCooking(IngredientType inIngredinetType, UnityAction inAfterCookAction)
    {
        _status = Status.Cooking;
        _afterCookAction = inAfterCookAction;
        _txtIngredient.text = MainController.GetIngredientName(inIngredinetType);

        _leftSec = 15;
        _leftSeconds.text = String.Format("{0}s", _leftSec);

        TimeManager.Instance.RegisterAction(1, OnTimer);
        _btnCompletedCook.onClick.AddListener(OnTouchCompletedCook);
    }
    void OnTimer()
    {
        _leftSec--;
        _leftSeconds.text = String.Format("{0}s", _leftSec);
        if (_leftSec <= 0)
        {
            _status = Status.Completed;
            _leftSeconds.text = "완료";
            TimeManager.Instance.UnregisterAction(1,OnTimer);
        }
    }
    public void OnTouchCompletedCook()
    {
        if (_status == Status.Completed)
        {
            _afterCookAction();
            _status = Status.Empty;
            _txtIngredient.text = "";
            _leftSeconds.text = "";
            gameObject.SetActive(false);
        }
    }
}
