using RedBjorn.Utils;
using TMPro;
using UnityEngine;

namespace RedBjorn.SuperTiles
{
    public class HealthChangeView : MonoBehaviour
    {
        public TextMeshPro Text;

        public void Init(float delta)
        {
            Text.color = Mathf.Sign(delta) > 0 ? Color.green : Color.red;
            Text.text = Mathf.Abs(delta).ToString();
            Spawner.Despawn(gameObject, 2f);
        }
    }
}
