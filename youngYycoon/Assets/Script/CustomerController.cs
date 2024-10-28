using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CooKing;
using TimeExtension;

public class CustomerController : MonoBehaviour
{
    [SerializeField] Customer m_customer;
    readonly int maxCustomer = 4;
    List<Customer> _customerList = new List<Customer>();
    Queue<Customer> _customerQueue = new Queue<Customer>();

    public void SetCustmoerController()
    {
        TimeManager.Instance.RegisterAction(3, GenerateCustomer);
    }
    void GenerateCustomer()
    {
        bool isGenerate = UnityEngine.Random.Range(0, 2) == 0;
        if(_customerList.Count < maxCustomer && isGenerate)
        {
            IngredientType randomIngredientType = (IngredientType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(IngredientType)).Length);
            CookType randomCookType = (CookType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(CookType)).Length);
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
        _customerQueue.Enqueue(inCustomer);
        _customerList.Remove(inCustomer);
        inCustomer.ReturnPool();
        inCustomer.gameObject.SetActive(false);
    }
    public bool SellCustomer((IngredientType, CookType) inDishType)
    {
        for(int i = 0; i < _customerList.Count; i++)
        {
            if(_customerList[i].GetDishType() == inDishType)
            {
                _customerList[i].BuyDish(()=>
                {
                    LeaveCustomer(_customerList[i]);
                });
                return true;
            }
        }
        return false;
    }
    public void OnDestroy()
    {
        TimeManager.Instance.UnregisterAction(3, GenerateCustomer);
    }
}
