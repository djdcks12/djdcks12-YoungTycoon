using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using System;
using CooKing;
using System.Threading;
using Cysharp.Threading.Tasks;

public class Customer : MonoBehaviour
{
    [SerializeField] TMP_Text _txtDish;
    [SerializeField] Image _customerImage;
    (IngredientType, CookType) _dishType;
    UnityAction _leaveCallback;
    int _leftSec = 0;
    int _maxSec = 0;
    CancellationTokenSource _cancellationTokenSource;

    public void SetCustomer((IngredientType, CookType) inDishType, UnityAction inLeaveCallback)
    {
        _dishType = inDishType;
        _leaveCallback = inLeaveCallback;
        _leftSec = 0;
        _maxSec = 100;
        _txtDish.text = MainController.DishName(inDishType);

        _cancellationTokenSource = new CancellationTokenSource();
        WaitAsync(_cancellationTokenSource.Token).Forget();
    }
    private async UniTaskVoid WaitAsync(CancellationToken token)
    {
        while (_leftSec < _maxSec)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.3), cancellationToken: token);
            _leftSec++;
            _customerImage.color = new Color(1, 1 - ((float)_leftSec / _maxSec), 1 - ((float)_leftSec / _maxSec));
        }

        _leaveCallback?.Invoke();
    }

    public void ReturnPool()
    {
        _cancellationTokenSource?.Cancel();
    }

    public void BuyDish(UnityAction inLeaveCallback)
    {
        inLeaveCallback?.Invoke();
    }
    public (IngredientType, CookType) GetDishType()
    {
        return _dishType;
    }
}
