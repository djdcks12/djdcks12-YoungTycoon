using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using System;

public class Customer : MonoBehaviour
{
    [SerializeField] TMP_Text _txtDish;
    [SerializeField] Image _custmoerImage;
    (GameManager.IngredientType, GameManager.CookType) _dishType;
    UnityAction _leaveCallback;
    int _leftSec = 0;
    int _maxSec = 0;
    IDisposable _timer;
    public void SetCustomer((GameManager.IngredientType, GameManager.CookType) inDishType, UnityAction inLeaveCallback)
    {
        _dishType = inDishType;
        _leaveCallback = inLeaveCallback;
        _leftSec = 0;
        _maxSec = 100;
        _txtDish.text = GameManager.dishName(inDishType);

        _timer = Observable.Interval(System.TimeSpan.FromSeconds(0.3)).Subscribe(_ => Wait()).AddTo(this.gameObject);
    }
    void Wait()
    {
        _leftSec++;
        _custmoerImage.color = new Color(1, 1 - ((float)_leftSec/_maxSec), 1 - ((float)_leftSec/_maxSec));
        if(_leftSec >= _maxSec)
        {
            _leaveCallback?.Invoke();
        }
    }
    public void BuyDish(UnityAction inLeaveCallback)
    {
        inLeaveCallback?.Invoke();
    }
    public (GameManager.IngredientType, GameManager.CookType) GetDishType()
    {
        return _dishType;
    }
}
