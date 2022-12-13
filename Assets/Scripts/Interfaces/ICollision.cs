using UnityEngine;

namespace Interfaces
{
    public interface ICollision
    {
        public void OnCollisionEnter2D(Collision2D other);
        public void OnCollisionExit2D(Collision2D other);
    }
}