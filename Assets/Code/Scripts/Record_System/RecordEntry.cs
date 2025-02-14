using Grid_System;
using Input_System;

namespace Record_System
{
    public struct RecordEntry
    {
        public PlateCell PlateCell => _plateCell;
        private PlateCell _plateCell;

        public SwipeDirection SwipeDirection => _swipeDirection;
        private SwipeDirection _swipeDirection;

        public RecordEntry(PlateCell plateCell, SwipeDirection swipeDirection)
        {
            _plateCell = plateCell;
            _swipeDirection = swipeDirection;
        }
    }
}
