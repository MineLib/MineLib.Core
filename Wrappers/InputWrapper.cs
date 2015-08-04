using System;

namespace MineLib.Core.Wrappers
{
    public delegate Action OnKeys(int newValue);

    public interface IInputWrapper
    {
        event OnKeys OnKey;

        void ShowKeyboard();

        void HideKeyboard();
    }

    public static class InputWrapper
    {
        private static IInputWrapper _instance;
        public static IInputWrapper Instance
        {
            private get
            {
                if (_instance == null)
                    throw new NotImplementedException("This instance is not implemented. You need to implement it in your main project");
                return _instance;
            }
            set { _instance = value; }
        }


        public static event OnKeys OnKey { add { Instance.OnKey += value; } remove { Instance.OnKey -= value; } }

        public static void ShowKeyboard() { Instance.ShowKeyboard();}

        public static void HideKeyboard() { Instance.HideKeyboard(); }
    }
}
