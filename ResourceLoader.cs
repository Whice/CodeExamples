using Cysharp.Threading.Tasks;
using Data;
using Utility;
using Zenject;

namespace Loading
{
    public class ResourceLoader : MonoBehaviourLogger
    {
        [Inject] private GlobalContainer globalContainer = null;
        private SceneLoader sceneLoader => globalContainer.sceneLoader;
        public bool isGlobalResourcesLoaded { get; private set; } = false;
        /// <summary>
        /// Загрузка общих ресурсов, которые используются во всей игре.
        /// Проводится только один раз при входе в игру.
        /// </summary>
        private async UniTask LoadGlobalResources()
        {
            globalContainer.savingService.Load();//ToDo: временно, чтобы загрузка считалась пройденой.
            if (!globalContainer.isGlobalResourcesLoaded)
            {
                SettingsValuesData data = globalContainer.savingService.LoadSettings();
                globalContainer.settingsValueContainer.SetData(data);

                globalContainer.isGlobalResourcesLoaded = true;
            }
            await globalContainer.localizationKeeper.LoadLocalization();
            isGlobalResourcesLoaded = true;
        }
        private async UniTask LoadBattleScene()
        {
            await UniTask.WaitForSeconds(0.1f);
            sceneLoader.LoadScene(SceneType.BATTLE);
        }
        private async UniTask LoadMainMenuScene()
        {
            await UniTask.WaitForSeconds(0.1f);
            sceneLoader.LoadScene(SceneType.MAIN_MENU);
        }
        private async UniTaskVoid LoadResurcesForScene()
        {
            if (!isGlobalResourcesLoaded)
            {
                await LoadGlobalResources();
            }

            switch (sceneLoader.sceneRequiredForLoading)
            {
                case SceneType.BATTLE:
                    {
                        await LoadBattleScene();
                        break;
                    }
                case SceneType.MAIN_MENU:
                    {
                        await LoadMainMenuScene();
                        break;
                    }
                default:
                    {
                        LogError($"Scene {sceneLoader.sceneRequiredForLoading.ToString()} unknown!");
                        break;
                    }
            }
        }
        private void Start()
        {
            LoadResurcesForScene().Forget();
        }
    }
}