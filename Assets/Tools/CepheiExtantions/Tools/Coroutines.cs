using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cephei
{
    public static class Coroutines
    {
        public static IEnumerator WaitToAction(Action action, float time)
        {
            yield return new WaitForSeconds(time);
            action.Invoke();
        }

        public static IEnumerator ActionInUpdat(Action action)
        {
            while (true)
            {
                yield return null;
                action.Invoke();
            }
        }
    }
}