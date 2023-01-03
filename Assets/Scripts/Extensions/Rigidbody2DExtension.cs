using Enums;
using UnityEngine;

namespace Extensions
{
    public static class Rigidbody2DExtension
    {
        // Modified for convenience, if we only want to set the x axis of the velocity
        public static void SetVelocity(this Rigidbody2D rigidbody2D, Axis axis, float value)
        {
            var oldVelocity = rigidbody2D.velocity;
            
            var newVelocity = new Vector2
            (
                axis == Axis.X ? value : oldVelocity.x,
                axis == Axis.Y ? value : oldVelocity.y
            );

            rigidbody2D.velocity = newVelocity;
        }
    }
}