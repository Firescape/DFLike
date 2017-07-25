//using UnityEngine;

using System.Collections;
using System;
using UnityEngine;

public class JsEventHandler<T, U> : object
{
    public delegate void JsEvent(T publisher, U eventArgs);

    private T publisher;

    public JsEventHandler(T publisher)
    {
        this.publisher = publisher;
    }

    private event JsEvent Subscribers;

    public void Subscribe(JsEvent subscriberFunction)
    {
        Debug.Log("got subscribed to ");
        Subscribers += subscriberFunction;
    }

    public void Publish(U e)
    {
        if (Subscribers != null)
            Subscribers(publisher, e);
    }
}