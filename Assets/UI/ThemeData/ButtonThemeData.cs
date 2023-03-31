using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonThemeData : UIThemeData
{
    public SelectableThemeData SelectableData;
    public LabelThemeData LabelData;

    public override void Copy(UIThemeData other)
    {
        if(other is ButtonThemeData buttonTheme)
        {
            SelectableData.Copy(buttonTheme.SelectableData);
            LabelData.Copy(buttonTheme.LabelData);
        }
    }
}
