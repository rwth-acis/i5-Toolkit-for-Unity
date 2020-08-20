using UnityEngine;
#if !UNITY_2019_4_OR_NEWER
using UnityEngine.UI;
#endif

namespace i5.Toolkit.Core.TestUtilities.UIOperator
{
    public static class UiOperator
    {
#if !UNITY_2019_4_OR_NEWER
        public static void PressButton(string buttonName)
        {
            Button btn = FindComponent<Button>(buttonName);
            PressButton(btn);
        }

        public static void PressButton(Button button)
        {
            if (button == null)
            {
                throw new UiElementNotFoundException("The button component could not be found.");
            }
            button.onClick.Invoke();
        }

        public static void ToggleToggle(string toggleName)
        {
            Toggle toggle = FindComponent<Toggle>(toggleName);
            toggle.isOn = !toggle.isOn;
        }

        public static void ToggleToggle(Toggle toggle)
        {
            if (toggle == null)
            {
                throw new UiElementNotFoundException("Toggle component could not be found.");
            }
            toggle.isOn = !toggle.isOn;
        }

        public static void SetToggleValue(string toggleName, bool value)
        {
            Toggle toggle = FindComponent<Toggle>(toggleName);
            SetToggleValue(toggle, value);
        }

        public static void SetToggleValue(Toggle toggle, bool value)
        {
            toggle.isOn = value;
        }

        public static void SetSliderValue(string sliderName, float value)
        {
            Slider slider = FindComponent<Slider>(sliderName);
            SetSliderValue(slider, value);
        }

        public static void SetSliderValue(Slider slider, float value)
        {
            slider.value = value;
        }

        public static void SetDropdownIndex(string dropdownName, int index)
        {
            Dropdown dropdown = FindComponent<Dropdown>(dropdownName);
        }

        public static void SetDropdownIndex(Dropdown dropdown, int index)
        {
            dropdown.value = index;
        }

        public static void SetInputFieldText(string inputFieldName, string text)
        {
            InputField inputField = FindComponent<InputField>(inputFieldName);
            SetInputFieldText(inputField, text);
        }

        public static void SetInputFieldText(InputField inputField, string text)
        {
            inputField.text = text;
        }

        public static void InputFieldEndEdit(string inputFieldName)
        {
            InputField inputField = FindComponent<InputField>(inputFieldName);
            InputFieldEndEdit(inputField);
        }

        public static void InputFieldEndEdit(InputField inputField)
        {
            inputField.onEndEdit.Invoke(inputField.text);
        }

        public static void SetScrollbarValue(string scrollBarName, float value)
        {
            Scrollbar scrollbar = FindComponent<Scrollbar>(scrollBarName);
            SetScrollbarValue(scrollbar, value);
        }

        public static void SetScrollbarValue(Scrollbar scrollbar, float value)
        {
            scrollbar.value = value;
        }

        public static void SetScrollRectValue(string scrollRectName, Vector2 value)
        {
            ScrollRect scrollRect = FindComponent<ScrollRect>(scrollRectName);
            SetScrollRectValue(scrollRect, value);
        }

        public static void SetScrollRectValue(ScrollRect scrollRect, Vector2 value)
        {
            scrollRect.normalizedPosition = value;
        }

        private static T FindComponent<T>(string gameObjectName) where T : MonoBehaviour
        {
            GameObject go = GameObject.Find(gameObjectName);
            if (go == null)
            {
                throw new UiElementNotFoundException("The GameObject with the component " 
                    + typeof(T).ToString() + " could not be found.");
            }
            T component = go.GetComponent<T>();
            if (component == null)
            {
                throw new UiElementNotFoundException
                    ("The GameObject could be found but it does not have a component " + typeof(T));
            }
            return component;
        }
#endif
    }
}
