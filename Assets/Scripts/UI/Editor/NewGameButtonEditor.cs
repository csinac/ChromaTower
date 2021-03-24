using UnityEditor;

namespace RectangleTrainer.ChromaTower.Inspector
{
    [CustomEditor(typeof(UI.NewGameButton))]
    public class NewGameButtonEditor: Editor
    {
        private void Awake()
        {
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        }
    }
}