using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VFXController : MonoBehaviour
{

    public void DestroyOnAnimationEnd()
    {
        Destroy(gameObject);
    }

}
