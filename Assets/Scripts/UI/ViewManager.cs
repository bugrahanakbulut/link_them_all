using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;

namespace LinkThemAll.UI
{
    public static class ViewConstants
    {
        public static readonly string InGameView = "InGameView";
        public static readonly string LevelFailedView = "LevelFailedView";
    }
    
    public class ViewManager : MonoBehaviour
    {
        [SerializeField] private Transform _uiParent;

        private readonly Dictionary<string, ViewBase> _loadedViews = new Dictionary<string, ViewBase>();
        
        public async UniTask LoadView(string path)
        {
            if (_loadedViews.ContainsKey(path))
            {
                Debug.Log($"[UiManager]::View already loaded! {path}");
                return;
            }
            
            try
            {
                GameObject viewObject = await Addressables.InstantiateAsync(path, _uiParent);
                viewObject.gameObject.SetActive(false);
                
                ViewBase view = viewObject.GetComponent<ViewBase>();

                if (view != null)
                {
                    _loadedViews.Add(path, view);
                    await view.Show();
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"{e.Message}\n{e.StackTrace}");
            }
        }

        public async UniTask UnloadView(string path)
        {
            if (!_loadedViews.ContainsKey(path))
            {
                Debug.Log($"[UiManager]::View not loaded yet! {path}");
                return;
            }

            try
            {
                _loadedViews.Remove(path, out ViewBase view);

                await view.Hide();
                
                Addressables.ReleaseInstance(view.gameObject);
            }
            catch (Exception e)
            {
                Debug.LogError($"{e.Message}\n{e.StackTrace}");
            }
        }
    }
}