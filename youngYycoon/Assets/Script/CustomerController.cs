using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CooKing;
using TimeExtension;

public class ISetCustmoerParm:IControllerParam
{

}
public class ISellSellCustomerParm : IControllerParam
{
    public (IngredientType, CookType) DishType { get; set; }
    public SellType SellType { get; set; }
    public Dish Dish { get; set; }
}

public class CustomerController : Controller
{
    readonly int maxCustomer = 4;
    List<Customer> _customerList = new List<Customer>();
    Queue<Customer> _customerQueue = new Queue<Customer>();

    protected override void OnRecv_SendMessage(IControllerParam param)
    {
        if(param is ISetCustmoerParm)
        {
            SetCustmoerController();
        }
        else if(param is ISellSellCustomerParm)
        {
            var sellSellCustomerParm = param as ISellSellCustomerParm;
            ControllerManager.Instance.SendMessage<MainController>(new IResponseSellCustomerParm()
            {
                isSell = SellCustomer(sellSellCustomerParm.DishType),
                Type = sellSellCustomerParm.DishType.Item1,
                Mode = sellSellCustomerParm.DishType.Item2,
                SellType = sellSellCustomerParm.SellType,
                Dish = sellSellCustomerParm.Dish
            });
        }
    }
    void SetCustmoerController()
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
            var customer = GameObject.Instantiate(GetViewer<CustomerViwer>().Customer, new Vector3(0,0,0), Quaternion.identity, GetViewer<CustomerViwer>().transform);
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

    bool SellCustomer((IngredientType, CookType) inDishType)
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
