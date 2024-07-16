using Events;
using Utils.Scenes;
using Utils.Singleton;

namespace Levels
{
    public class LevelsManager : DontDestroyMonoBehaviour
    {
        // закоментировано для тестов
        //private const string LevelNamePattern = "Level{0}";
        

        private int _currentLevelIndex;


        private void Start()
        {
            // закоментировано для тестов
            //ScenesChanger.GotoScene(string.Format(LevelNamePattern, _currentLevelIndex));
            
            //тест
            ScenesChanger.GotoScene("CameraMovementTestLevel");
            EventsController.Subscribe<EventModels.Game.TargetColorNodesFilled>(this, OnTargetColorNodesFilled);
        }

        private void OnTargetColorNodesFilled(EventModels.Game.TargetColorNodesFilled e)
        {
            // закоментировано для тестов
            //_currentLevelIndex += 1;
            //ScenesChanger.GotoScene(string.Format(LevelNamePattern, _currentLevelIndex));
            
            //тест
            ScenesChanger.GotoScene("CameraMovementTestLevel");
        }
    }
}