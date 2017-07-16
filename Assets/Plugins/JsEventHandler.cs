//using UnityEngine;
using System.Collections;
using System;

public class JsEventHandler<T, U> : object
{
	public delegate void JsEvent(T publisher, U eventArgs);
	private event JsEvent Subscribers;
	private T publisher;
	
	public JsEventHandler(T publisher)
	{
		this.publisher = publisher;
	}
	
	public void Subscribe(JsEvent subscriberFunction)
	{
		UnityEngine.Debug.Log("got subscribed to ");
		Subscribers += subscriberFunction;
	}
	
	public void Publish(U e)
	{
		if(Subscribers != null)
		{
			Subscribers(publisher, e);
		}
	}
}
