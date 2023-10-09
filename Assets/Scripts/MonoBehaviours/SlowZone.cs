using System.Collections.Generic;
using UnityEngine;

namespace Game.MonoBehaviours
{
    [RequireComponent(typeof(Collider2D))]
    public class SlowZone : MonoBehaviour
    {
        public float TimeFactor = 0.1f;

        private Dictionary<Rigidbody2D, ObjectInfo> _bodyInfos = new Dictionary<Rigidbody2D, ObjectInfo>();

        private void FixedUpdate()
        {
            foreach (var pair in _bodyInfos)
            {
                var rigidbody = pair.Key;
                var info = pair.Value;

                if (info.PrevVelocity != null)
                {
                    var acceleration = rigidbody.velocity - info.PrevVelocity.Value;

                    var angularAcceleration = rigidbody.angularVelocity - info.PrevAngularVelocity.Value;

                    info.PrevVelocity = rigidbody.velocity = info.UnscaledVelocity * TimeFactor;
                    info.PrevAngularVelocity = rigidbody.angularVelocity = info.UnscaledAngularVelocity * TimeFactor;

                    info.UnscaledVelocity += acceleration;
                    info.UnscaledAngularVelocity += angularAcceleration;
                }
                else
                {
                    info.PrevVelocity = rigidbody.velocity = info.UnscaledVelocity * TimeFactor;
                    info.PrevAngularVelocity = rigidbody.angularVelocity = info.UnscaledAngularVelocity * TimeFactor;
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var info = new ObjectInfo();
            info.PrevVelocity = null;
            info.UnscaledVelocity = other.attachedRigidbody.velocity;
            info.UnscaledAngularVelocity = other.attachedRigidbody.angularVelocity;
            _bodyInfos.Add(other.attachedRigidbody, info);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (_bodyInfos.ContainsKey(other.attachedRigidbody))
            {
                var info = _bodyInfos[other.attachedRigidbody];
                other.attachedRigidbody.angularVelocity = info.UnscaledAngularVelocity;
                other.attachedRigidbody.velocity = info.UnscaledVelocity;
                _bodyInfos.Remove(other.attachedRigidbody);
            }
        }
    }

    internal class ObjectInfo
    {
        public Vector2 UnscaledVelocity;
        public float UnscaledAngularVelocity;
        public Vector2? PrevVelocity;
        public float? PrevAngularVelocity;
    }
}