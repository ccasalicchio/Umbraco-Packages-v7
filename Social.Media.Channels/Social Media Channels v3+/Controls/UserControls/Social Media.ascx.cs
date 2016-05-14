using Social_Media_Channels.Engine;
using Social_Media_Channels.Engine.Utilities;
using Social_Media_Channels.Entities;
using System;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControls_Social_Media : System.Web.UI.UserControl, umbraco.editorControls.userControlGrapper.IUsercontrolDataEditor
{
    private Manager _manager = null;
    static Manager _tmp = null;

    public string Name { get; set; }
    public string Description { get; set; }
    public string Creator { get; set; }
    public string Created { get; set;}
    public string Url { get; set; }

    protected override void OnInit(EventArgs e)
    {
        string ThemesFolder = ConfigurationManager.AppSettings["SocialMediaThemeFolder"] == null ? "~/usercontrols/themes/" : ConfigurationManager.AppSettings["SocialMediaThemeFolder"].ToString();

        _manager = new Manager(ThemesFolder, false, true);
        _tmp = new Manager(ThemesFolder, false, true);

        if (!Page.IsPostBack)
        {
            _manager.ThemesDropdownList(ref dpStyles, _manager.Themes);
        }
        
        base.OnInit(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        _manager.LoadSelectedTheme(dpStyles.SelectedValue);
        _manager.ShowLabels = chkShowLabel.Checked;
        _manager.RenderSocialItems(tableLinks, _manager.CurrentTheme);

        //Theme details
        Name = _manager.CurrentTheme.ID;
        Description = _manager.CurrentTheme.Description;
        Creator = _manager.CurrentTheme.Creator;
        Created = _manager.CurrentTheme.CreatedDate;
        Url = _manager.CurrentTheme.Url;
    }
    public object value
    {
        get
        {
            foreach (Channel channel in _manager.CurrentTheme.Channels)
            {
                string ctrl = "txt" + channel.ID;
                channel.Url = ((TextBox)tableLinks.FindControl(ctrl)).Text;
            }

            string json = JsonHelper.JsonSerializer<Manager>(_manager);
            return json;
        }
        set
        {
            if (value.ToString() != string.Empty)
            {
                _tmp = (Manager)JsonHelper.JsonDeserialize<Manager>(value.ToString());
                _manager.ShowLabels = _tmp.ShowLabels;
                _manager.CurrentThemeID = _tmp.CurrentThemeID;

                dpStyles.SelectedValue = _manager.CurrentThemeID;
                chkShowLabel.Checked = _manager.ShowLabels;
                LoadTheme();
            }
        }
    }

    protected void chkShowLabel_CheckedChanged(object sender, EventArgs e)
    {
        _manager.ShowLabels = (chkShowLabel.Checked);
    }
    
    private void LoadTheme()
    {
        _manager.LoadSelectedTheme(_tmp.CurrentThemeID);
        _manager.LoadSavedUrls(_manager.CurrentTheme, _tmp.CurrentTheme);
    }
}