using UnityEngine;

namespace RedBjorn.SuperTiles.Utils
{
    public class BarSprite : MonoBehaviour
    {
        public SpriteRenderer Current;
        public SpriteRenderer Background;

        public float WidthMax;

        public float WidthCurrent
        {
            get
            {
                return Current.size.x;
            }
            set
            {
                Current.size = new Vector2(value, Current.size.y);
            }
        }

        void Awake()
        {
            WidthMax = WidthCurrent;
        }
    }
}
