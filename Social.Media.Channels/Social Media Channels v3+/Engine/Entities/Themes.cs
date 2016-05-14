using System.Collections.Generic;

namespace Social_Media_Channels.Entities
{
    public class Themes
    {
        private List<Theme> _themes = new List<Theme>();
        public List<Theme> List { get { return _themes; } }

        public override string ToString()
        {
            return string.Format("Themes [Count: {0}]", _themes.Count);
        }
    }
}