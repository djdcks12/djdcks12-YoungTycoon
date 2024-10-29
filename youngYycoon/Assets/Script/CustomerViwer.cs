using UnityEngine;

public class CustomerViwer : Viwer
{
    [SerializeField] Customer m_customer;

    public Customer Customer => m_customer;

    void Awake()
    {
        ISetViwerParam setViwerParam = new ISetViwerParam()
        {
            Viwer = this,
        };
        ControllerManager.Instance.SendMessage<CustomerController>(setViwerParam);
    }
}
