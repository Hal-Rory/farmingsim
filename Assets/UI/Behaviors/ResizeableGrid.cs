using UnityEngine;
using UnityEngine.UI;

public class ResizeableGrid : MonoBehaviour
{
    internal enum Sizing { Square, Fit, None };
    internal enum SizePreference { Column, Row};
    [SerializeField] private Sizing ChildSizing;
    [SerializeField] private SizePreference Preference;

    public GridLayoutGroup Grid;
    public bool Resize;
    public int MaxColumnCount;
    public int MaxRowCount;
    private void OnValidate()
    {
        if (Resize)
        {
            Resize= false;
            ResizeChildren();
        }   
    }
    public void ResizeChildren()
    {
        if (Grid.constraint != GridLayoutGroup.Constraint.Flexible) return;
        Vector2 resize = Vector2.zero;
        Vector2 gridRect = new Vector2((Grid.transform as RectTransform).rect.width - Grid.padding.left - Grid.padding.right, (Grid.transform as RectTransform).rect.height - Grid.padding.top -Grid.padding.bottom);
        float rowCount = Mathf.Ceil((transform.childCount * 1.0f) / (Mathf.Max(MaxColumnCount, 1)));
        float columnCount = Mathf.Ceil((transform.childCount * 1.0f) / (Mathf.Max(MaxRowCount, 1)));
        gridRect.x -= Grid.spacing.x * (rowCount + 1);
        gridRect.y -= Grid.spacing.y * (columnCount+ 1);
        if (Preference == SizePreference.Column)
        {
            resize.x = gridRect.x / MaxColumnCount;
            if (ChildSizing == Sizing.Fit)
            {
                resize.y = gridRect.y / rowCount;
            }
        } else
        {
            resize.x = gridRect.x / columnCount;
            if (ChildSizing == Sizing.Fit)
            {
                resize.y = gridRect.y / Mathf.Ceil(transform.childCount / columnCount );
            }
        }
        if (ChildSizing == Sizing.Square)
        {
            resize.y = resize.x;
        }
        else if(ChildSizing == Sizing.None)
        {
            resize.y = Grid.cellSize.y;
        }       
        Grid.cellSize= resize;
    }
}
