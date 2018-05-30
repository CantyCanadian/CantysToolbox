using System.Collections;
using System.Collections.Generic;

public interface PoolLifetime
{
    public void Initialize();
    public void OnTrigger();
    public void OnDiscard();
}
