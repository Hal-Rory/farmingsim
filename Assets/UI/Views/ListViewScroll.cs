using UnityEngine;
using UnityEngine.UI;

public class ListViewScroll : ListView
{
    [SerializeField] private ScrollRect Scroll;
    public void GoToBottom()
    {
        SetScrollPos(0);
    }
    public void GoToTop()
    {
        SetScrollPos(1);
    }
    private void SetScrollPos(float pos)
    {
        Scroll.verticalNormalizedPosition = pos;
    }
}
