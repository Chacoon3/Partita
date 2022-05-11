using UnityEngine.UI;

namespace Partita.ExtComponent {
    public enum LayoutStyle {
        Vertical, Horizontal,
    }
    public class Pager : HorizontalOrVerticalLayoutGroup {

        public LayoutStyle style;

        public override void CalculateLayoutInputHorizontal() {
            base.CalculateLayoutInputHorizontal();
            CalcAlongAxis(0, isVertical: true);
        }

        public override void CalculateLayoutInputVertical() {
            CalcAlongAxis(1, isVertical: true);
        }

        public override void SetLayoutHorizontal() {
            SetChildrenAlongAxis(0, isVertical: true);
        }

        public override void SetLayoutVertical() {
            SetChildrenAlongAxis(1, isVertical: true);
        }
    }
}
