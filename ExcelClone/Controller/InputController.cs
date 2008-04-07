using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelClone.InputController
{
    class InputController
    {
        public static InputController Instance
        {
            get
            {
                return InputControllerCreator.InputControllerInstance;
            }

        }

        private sealed class InputControllerCreator
        {
            private static readonly InputController _instance = new InputController();
            public static InputController InputControllerInstance
            {
                get
                {
                    return _instance;
                }
            }
        }
    }
}
