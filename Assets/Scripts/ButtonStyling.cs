
using UnityEngine;
using UnityEngine.UI;

public class ButtonStyling {

    static ColorBlock s_ButtonColorBlock = new ColorBlock { normalColor = Color.white, highlightedColor = Color.HSVToRGB(0, 0, 0.8f) };

    static public void StyleAllChildButtons(Transform transform) {
        StyleButton(transform);
        for (int i=0; i<transform.childCount; i++) {
            StyleAllChildButtons(transform.GetChild(i));
        }
    }

    static public void StyleButton(Transform transform) {
        var button = transform.GetComponent<Button>();

        if (button != null) {
            var colors = button.colors;
            colors.highlightedColor = s_ButtonColorBlock.highlightedColor;
            colors.normalColor = s_ButtonColorBlock.normalColor;
            button.colors = colors;
        }
    }

}
