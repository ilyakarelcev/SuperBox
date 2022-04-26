using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cephei
{
    public static class CollisionExtantion
    {
        public static Vector3 MidleContactPoint(this Collision collision)
        {
            Vector3 midPoint = Vector3.zero;
            foreach (var contact in collision.contacts)
            {
                midPoint += contact.point;
            }

            return midPoint / collision.contacts.Length;
        }
    }
}