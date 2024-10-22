using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UniRx;

public class Cook : MonoBehaviour
{
    [SerializeField] TMP_Text _txtIngredient;
    [SerializeField] TMP_Text _leftSeconds;
    [SerializeField] Button _btnCompletedCook;
    UnityAction _afterCookAction;
    private IDisposable timer;

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

    public void StartCooking(GameManager.IngredientType inIngredinetType, UnityAction inAfterCookAction)
    {
        _status = Status.Cooking;
        _afterCookAction = inAfterCookAction;
        _txtIngredient.text = GameManager.GetIngredientName(inIngredinetType);

        _leftSec = 15;
        _leftSeconds.text = String.Format("{0}s", _leftSec);

        timer = Observable.Interval(TimeSpan.FromSeconds(1)).Subscribe(_ => OnTimer()).AddTo(this.gameObject);
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
            timer.Dispose();
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
