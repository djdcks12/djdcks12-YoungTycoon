using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

public class CustomerController : MonoBehaviour
{
    [SerializeField] Customer m_customer;
    readonly int maxCustomer = 4;
    List<Customer> _customerList = new List<Customer>();
    Queue<Customer> _customerQueue = new Queue<Customer>();

    public void SetCustmoerController()
    {
        Observable.Interval(TimeSpan.FromSeconds(3)).Subscribe(_ => GenerateCustomer()).AddTo(this.gameObject);
    }
    void GenerateCustomer()
    {
        bool isGenerate = UnityEngine.Random.Range(0, 2) == 0;
        if(_customerList.Count < maxCustomer && isGenerate)
        {
            GameManager.IngredientType randomIngredientType = (GameManager.IngredientType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(GameManager.IngredientType)).Length);
            GameManager.CookType randomCookType = (GameManager.CookType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(GameManager.CookType)).Length);
            var customer = GetCustomer();
            customer.SetCustomer((randomIngredientType, randomCookType), () => LeaveCustomer(customer));
            customer.gameObject.SetActive(true);
            _customerList.Add(customer);
        }
    }
    Customer GetCustomer()
    {
        if(_customerQueue.Count == 0)
        {
            var customer = Instantiate(m_customer, new Vector3(0,0,0), Quaternion.identity, this.transform);
            return customer;
        }
        else
        {
            return _customerQueue.Dequeue();
        }
    }

    void LeaveCustomer(Customer inCustomer)
    {
        _customerList.Remove(inCustomer);
        inCustomer.gameObject.SetActive(false);
    }
    public bool SellCustomer((GameManager.IngredientType, GameManager.CookType) inDishType)
    {
        for(int i = 0; i < _customerList.Count; i++)
        {
            if(_customerList[i].GetDishType() == inDishType)
            {
                _customerList[i].BuyDish(()=>
                {
                    Destroy(_customerList[i].gameObject);
                    _customerList.RemoveAt(i);
                });
                return true;
            }
        }
        return false;
    }
}
