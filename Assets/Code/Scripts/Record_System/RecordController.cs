using System.Collections.Generic;

using Grid_System;
using Input_System;

namespace Record_System
{
    //This class is used to store all the Record Entry into a Stack
    //A class has been created to organize better the architecture
    public class RecordController
    {
        public Stack<RecordEntry> RecordStack => _recordStack;
        private Stack<RecordEntry> _recordStack;

        public RecordController()
        {
            _recordStack = new Stack<RecordEntry>();
        }

        public void AddEntry(PlateCell cell, SwipeDirection direction)
        {
            //Add a new RecordEntry to the stack
            _recordStack.Push(new RecordEntry(cell, direction));
        }
    }
}
