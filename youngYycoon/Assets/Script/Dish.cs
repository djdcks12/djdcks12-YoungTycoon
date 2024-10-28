using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using CooKing;

public class Dish : MonoBehaviour
{
    [SerializeField] TMP_Text _txtDish;
    [SerializeField] Button _btnSell;
    private (IngredientType, CookType) _dishType;
    public void SetDish((IngredientType, CookType) inDishType, UnityAction inSellAction)
    {
        _dishType = inDishType;
        _txtDish.text = GameManager.DishName(inDishType);
        _btnSell.onClick.AddListener(inSellAction);
    }
    public (IngredientType, CookType) GetDishType()
    {
        return _dishType;
    }
}
