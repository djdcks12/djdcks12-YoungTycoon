using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Dish : MonoBehaviour
{
    [SerializeField] TMP_Text _txtDish;
    [SerializeField] Button _btnSell;
    private (GameManager.IngredientType, GameManager.CookType) _dishType;
    public void SetDish((GameManager.IngredientType, GameManager.CookType) inDishType, UnityAction inSellAction)
    {
        _dishType = inDishType;
        _txtDish.text = GameManager.dishName(inDishType);
        _btnSell.onClick.AddListener(inSellAction);
    }
    public (GameManager.IngredientType, GameManager.CookType) GetDishType()
    {
        return _dishType;
    }
}
