using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * A grid component for UI that emulates the way the Unreal Engine 4 Grid Layout
 * Written by Nikhil Ghosh '24
 */

namespace WSoft.UI
{
    public class FlexibleGridComponent : LayoutGroup
    {
        public enum FitType
        {
            Uniform,
            Width,
            Height,
            FixedRows,
            FixedColumns,
            FixedRowsAndColumns
        }

        [Tooltip("The type of fit. Uniform will make an equal number of rows and columns, width is a fixed number of columns, and height is a fixed number of rows")]
        public FitType fitType;

        public int rows;
        public int columns;

        public Vector2 cellSize;

        public Vector2 spacing;

        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();

            if (fitType == FitType.Uniform || fitType == FitType.Width || fitType == FitType.Height)
            {
                float sqrtChildren = Mathf.Sqrt(transform.childCount);
                rows = Mathf.CeilToInt(sqrtChildren);
                columns = Mathf.CeilToInt(sqrtChildren);
            }

            if (fitType == FitType.Width || fitType == FitType.FixedColumns)
            {
                if (columns != 0)
                {
                    rows = Mathf.CeilToInt(transform.childCount / (float)columns);
                }
            }
            else if (fitType == FitType.Height || fitType == FitType.FixedRows)
            {
                if (rows != 0)
                {
                    columns = Mathf.CeilToInt(transform.childCount / (float)rows);
                }
            }

            float parentWidth = rectTransform.rect.width;
            float parentHeight = rectTransform.rect.height;

            float cellWidth = parentWidth / (float)columns - spacing.x - (padding.left / (float)columns) - (padding.right / (float)columns);
            float cellHeight = parentHeight / (float)rows - spacing.y - (padding.top / (float)rows) - (padding.bottom / (float)rows);
            cellSize = new Vector2(Mathf.Abs(cellWidth), Mathf.Abs(cellHeight));

            int columnCount = 0;
            int rowCount = 0;

            for (int i = 0; i < rectChildren.Count; i++)
            {
                rowCount = i / columns;
                columnCount = i % columns;

                float positionX = (cellSize.x + 2 * spacing.x) * columnCount + padding.left;
                float positionY = (cellSize.y + 2 * spacing.y) * rowCount + padding.top;

                RectTransform child = rectChildren[i];
                SetChildAlongAxis(child, 0, positionX, cellSize.x);
                SetChildAlongAxis(child, 1, positionY, cellSize.y);
            }
        }

        public override void CalculateLayoutInputVertical()
        {
            // Required for LayoutGroup component, not necessary to implement
        }

        public override void SetLayoutHorizontal()
        {
            // Required for LayoutGroup component, not necessary to implement
        }

        public override void SetLayoutVertical()
        {
            // Required for LayoutGroup component, not necessary to implement
        }
    }
}