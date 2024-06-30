using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Infrastructure.GameManegment
{
    public interface ICoroutineRunner
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
    }
}
