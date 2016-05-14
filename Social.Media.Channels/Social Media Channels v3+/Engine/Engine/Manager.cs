using System.IO;
using Social_Media_Channels.Engine.Utilities;
using Social_Media_Channels.Entities;
using System.Web.UI.WebControls;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Social_Media_Channels.Engine
{
    [DataContract]
    public class Manager
    {
        #region Manager properties
        public string ThemesFolder { get; set; }
        [DataMember(Name = "CurrentTheme")]
        public string CurrentThemeID { get; set; }
        [DataMember(Name = "Theme")]
        public Theme CurrentTheme { get; set; }
        public Themes Themes { get; set; }
        public int Count { get { return Themes.List.Count; } }
        [DataMember(Name = "ShowLabels")]
        public bool ShowLabels { get; set; }
        #endregion

        #region Constructor
        public Manager(string ThemesFolder = "~/usercontrols/themes/", bool ShowLabels = false, bool isWeb = false)
        {
            this.ThemesFolder = ThemesFolder;
            this.ShowLabels = ShowLabels;
            this.Themes = new Themes();
            Load(this.Themes, this.ThemesFolder, isWeb);
        }
        #endregion

        #region Html Controls Tags
        private TableRow Tr()
        {
            return new TableRow();
        }
        private TableCell Td()
        {
            return new TableCell();
        }
        public static Literal RenderA(Theme theme, Channel channel)
        {
            Literal anchor = Anchor(theme, channel, false);
            return anchor;
        }

        public static string RenderRawA(Theme theme, Channel channel)
        {
            Literal anchor = Anchor(theme, channel, true);
            return anchor.Text;
        }

        public static Literal Anchor(Theme theme, Channel channel, bool isRaw)
        {
            Literal anchor = new Literal();
            string folder;

            if (isRaw) folder = theme.Path;
            else folder = anchor.ResolveClientUrl(@"~\" + theme.Path);

            string[] args = { folder , channel.Image, channel.Name, channel.Image };
            string img = string.Format(@"<img src='{0}{1}' style='border:none;' class='social-icon' data-name='{2}' data-image='{3}' />", args);
            anchor.Text = string.Format("<a href='{0}' title='{1}' target='_blank'>{2}</a>", channel.Url, channel.Name, img);

            return anchor;
        }
        public static TextBox RenderTextBox(Channel channel)
        {
            return new TextBox
            {
                ID = "txt" + channel.ID,
                Text = channel.Url,
                TextMode = TextBoxMode.SingleLine,
                Rows = 1,
                Columns = 40,
                CssClass = "umbEditorTextField"
            };
        }
        #endregion

        #region Html Controls Methods
        public void ThemesDropdownList(ref DropDownList dpList, Themes themes)
        {
            foreach (Theme theme in themes.List)
                dpList.Items.Add(new ListItem(theme.ID, theme.ID));
        }
        public void RenderSocialItems(Table table, Theme theme)
        {
            table.Controls.Clear();


            if (theme.Channels != null)
            {

                foreach (Channel channel in theme.Channels)
                {
                    TableRow tr = Tr();
                    TableCell tdLabel = Td();
                    TableCell tdTextBox = Td();
                    //Add controls to table cell
                    tdLabel.Controls.Add(RenderA(theme, channel));
                    tdTextBox.Controls.Add(RenderTextBox(channel));

                    //add table cells to table row
                    tr.Controls.Add(tdLabel);
                    tr.Controls.Add(tdTextBox);

                    //add row to table
                    table.Controls.Add(tr);
                }
            }
            else
            {
                TableRow tr = Tr();
                TableCell tdNoChannels = Td();
                tdNoChannels.Text = "No Themes have been found, check your Themes Folder";
                tr.Controls.Add(tdNoChannels);
                table.Controls.Add(tr);
            }
        }
        #endregion

        #region Engine Methods
        public Theme LoadSelectedTheme(string CurrentThemeID = "")
        {
            if (CurrentThemeID != null)
                this.CurrentThemeID = CurrentThemeID;

            try
            {
                var dyn = from th in this.Themes.List
                          where th.ID == this.CurrentThemeID
                          select th;
                CurrentTheme = ((IEnumerable<Theme>)dyn).First<Theme>();
            }
            catch
            {
                this.CurrentTheme = new Theme();
            }

            return this.CurrentTheme;
        }

        public void LoadSavedUrls(Theme CurrentTheme, Theme SavedTheme)
        {
            if (this.CurrentTheme.Channels != null)
            {
                foreach (Channel channel in CurrentTheme.Channels)
                    channel.Url = (from ch in SavedTheme.Channels where ch.ID == channel.ID select ch.Url).First();
            }
        }
        private Theme Load(string path)
        {
            Theme theme = new Theme();
            theme = new Serializer<Theme>().DeSerializeObject<Theme>(path);
            string url = path.Replace(HttpContext.Current.Request.PhysicalApplicationPath, string.Empty)
                .Replace("//", @"\");
            string folder = url.Substring(0, url.LastIndexOf(@"\")) + @"\";

            theme.Path = folder;
            return theme;
        }

        private Themes Load(Themes themes, string path, bool isWeb = false)
        {
            if (isWeb) path = HttpContext.Current.Server.MapPath(path);
            DirectoryInfo[] themelist = new DirectoryInfo(path).GetDirectories().ToArray();
            foreach (DirectoryInfo subDir in themelist)
            {
                DrillFolders(subDir, ref themes);
            }
            return themes;
        }
        private void DrillFolders(DirectoryInfo directory, ref Themes themes)
        {
            string themexml = string.Format(@"{0}\{1}.xml", directory.FullName, directory.Name);
            if (File.Exists(themexml))
                themes.List.Add(Load(themexml));
            else
            {
                DirectoryInfo[] subDirs = directory.GetDirectories().ToArray();
                foreach (DirectoryInfo subDir in subDirs)
                    DrillFolders(subDir, ref themes);
            }
        }
        #endregion
    }


}
