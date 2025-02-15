using System.Collections;
using UnityEngine;

namespace Infrastructure.GameManegment
{
    public interface ICoroutineRunner
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
    }
}
