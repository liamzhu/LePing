using System.Collections;
using UnityEngine;

public interface INotifier
{
    void OnReceiveData(uint cmdId, object param1, object param2);
}
