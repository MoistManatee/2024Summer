using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class XPCrystal : MonoBehaviour, ISubject, IPoolable
{
    [SerializeField] Player PlayerRef;
    float lifetime = 60f;

    public void Reset()
    {
        lifetime = 60f;
    }
    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime < 0)
        {
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            Notify();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //Attach(PlayerRef);
    }

    // For the sake of simplicity, the Subject's state, essential to all
    // subscribers, is stored in this variable.
    public int State { get; set; } = -0;

    public int XP_Value = 50;

    // List of subscribers. In real life, the list of subscribers can be
    // stored more comprehensively (categorized by event type, etc.).
    [SerializeField] private List<IObserver> _observers = new List<IObserver>();

    // The subscription management methods.
    public void Attach(IObserver observer)
    {
        Console.WriteLine("Subject: Attached an observer.");
        this._observers.Add(observer);
    }

    public void Detach(IObserver observer)
    {
        this._observers.Remove(observer);
        Console.WriteLine("Subject: Detached an observer.");
    }

    // Trigger an update in each subscriber.
    public void Notify()
    {
        Console.WriteLine("Subject: Notifying observers...");

        foreach (var observer in _observers)
        {
            observer.UpdateObserver(this, XP_Value);
        }
    }
}

public interface ISubject
{
    // Attach an observer to the subject.
    void Attach(IObserver observer);

    // Detach an observer from the subject.
    void Detach(IObserver observer);

    // Notify all observers about an event.
    void Notify();
}
public interface IObserver
{
    // Receive update from subject
    void UpdateObserver(ISubject subject, int XP);
}
