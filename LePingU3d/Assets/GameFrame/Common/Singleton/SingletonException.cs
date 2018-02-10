using UnityEngine;
using System.Collections;

public class SingletonException : System.Exception
{

    public SingletonException(string msg) : base(msg)
    {
    }
}
