using System.Collections;
using System.Collections.Generic;

public interface IPoolComponent
{
    void Initialize();
    void OnTrigger();
    void OnDiscard();
}
