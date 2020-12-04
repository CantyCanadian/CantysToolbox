using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LayerUtil
{
    public static bool MaskContainsLayer(LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }
}
