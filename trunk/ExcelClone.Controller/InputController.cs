using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelClone.Controller
{
    public class InputController
    {
        private InputController()
        {
        }

        public static InputController Instance
        {
            get { return ControllerCreator.CreatorInstance; }
        }

        private sealed class ControllerCreator
        {
            private static readonly InputController _instance = new InputController();

            public static InputController CreatorInstance
            {
                get { return _instance; }
            }
        }
    }
}
