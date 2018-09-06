using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyUtilities : MonoBehaviour {

	public static bool DetectIfOnTablet(Key test) {
        GameObject block = FindUtilities
            .TryFind(test.transform.parent.parent.gameObject, "Block");
        return block != null && block.tag == "Tablet";
    }

    public static bool DetectIfOnGift(Key test) {
        GameObject block = FindUtilities
            .TryFind(test.transform.parent.parent.gameObject, "Block");
        return block != null && block.tag == "Gift";
    }

    public static T ConvertKey<F, T>(ref GameObject keyObject, Color newDefault, string newTag) 
        where F : Key
        where T : Key
    {
        F fromKey = keyObject.GetComponent<F>();
        fromKey.ResetPressed();

        char val = fromKey.GetValue();
        int row = fromKey.GetRow();
        int col = fromKey.GetCol();

        bool left = fromKey.LeftAvailable();
        bool right = fromKey.RightAvailable();
        bool up = fromKey.UpAvailable();
        bool down = fromKey.DownAvailable();

        bool pressed = fromKey.IsPressed();
        Vector3 oldAvail = fromKey.GetAVAIL();
        Vector3 oldUnavail = fromKey.GetUNAVAIL();

        Object.Destroy(fromKey);
        keyObject.AddComponent(typeof(T));
        T toKey = keyObject.GetComponent<T>();
        toKey.SetRow(row);
        toKey.SetCol(col);
        toKey.SetNeighbors(left, right, up, down);
        toKey.SetValue(val);
        toKey.SetAVAIL(oldAvail);
        toKey.SetUNAVAIL(oldAvail);
        toKey.pressed = pressed;
        toKey.defaultColor = newDefault;
        keyObject.tag = newTag;
        toKey.ReturnToDefaultColor();
        return toKey;
    }
	
}
