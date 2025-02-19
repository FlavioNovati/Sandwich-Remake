using UnityEngine;

namespace Input_System.Extentions
{
    public static class SwipeDirectionExtentions
    {
        //Convert swipe direction to Vector2Int
        public static Vector2Int ToVector2Int(this SwipeDirection swipeDirection)
        {
            switch(swipeDirection)
            {
                case SwipeDirection.UP:
                    return Vector2Int.up;
                case SwipeDirection.DOWN:
                    return Vector2Int.down;
                case SwipeDirection.LEFT:
                    return Vector2Int.left;
                case SwipeDirection.RIGHT:
                    return Vector2Int.right;

                default: return Vector2Int.zero;
            }
        }

        //Convert swipe direction to Vector3
        public static Vector3 ToVector3(this SwipeDirection swipeDirection)
        {
            switch (swipeDirection)
            {
                case SwipeDirection.UP:
                    return Vector3.forward;
                case SwipeDirection.DOWN:
                    return Vector3.back;
                case SwipeDirection.LEFT:
                    return Vector3.left;
                case SwipeDirection.RIGHT:
                    return Vector3.right;

                default: return Vector3.zero;
            }
        }

        //Reverse swipe direction
        public static SwipeDirection Reverse(this SwipeDirection swipeDirection)
        {
            switch (swipeDirection)
            {
                case SwipeDirection.UP:
                    return SwipeDirection.DOWN;
                case SwipeDirection.DOWN:
                    return SwipeDirection.UP;
                case SwipeDirection.LEFT:
                    return SwipeDirection.RIGHT;
                case SwipeDirection.RIGHT:
                    return SwipeDirection.LEFT;

                default: return SwipeDirection.INVALID;
            }
        }

        //Convert Vector2 to swipe direction
        public static SwipeDirection ToSwipeDirection(this Vector2 direction)
        {
            switch (direction.x)
            {
                case > 0:
                    return SwipeDirection.RIGHT;
                case < 0:
                    return SwipeDirection.LEFT;
            }

            switch (direction.y)
            {
                case > 0:
                    return SwipeDirection.UP;
                case < 0:
                    return SwipeDirection.DOWN;
            }

            return SwipeDirection.INVALID;
        }
    }
}
