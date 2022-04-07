using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPersonComponent
{
    public IPerson Person { get; }

    void Init(IPerson person);
}