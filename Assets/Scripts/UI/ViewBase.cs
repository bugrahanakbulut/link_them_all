using Cysharp.Threading.Tasks;
using UnityEngine;

namespace LinkThemAll.UI
{
    public abstract class ViewBase : MonoBehaviour
    {
        public abstract UniTask Show();
        public abstract UniTask Hide();
    }
}