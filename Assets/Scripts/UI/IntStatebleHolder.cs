using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class IntStatebleHolder : MonoBehaviour {

    public List<IntStatebleItem> Items;

}

[System.Serializable]
public class UnityEventBool : UnityEvent<bool> { }
