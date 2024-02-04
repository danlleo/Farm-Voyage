namespace FarmTools
{
    public abstract class FarmTool
    {
        private const int DefaultToolLevel = 1;
        private int _toolLevel;

        protected FarmTool(int toolLevel)
        {
            SetToolLevel(toolLevel);
        }
        
        private void SetToolLevel(int toolLevel)
        {
            _toolLevel = toolLevel == 0 ? DefaultToolLevel : toolLevel;
        }
    }
}
