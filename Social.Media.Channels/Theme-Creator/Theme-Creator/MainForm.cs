using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.IO;
using System.Xml.Schema;
using System.Xml;

namespace Theme_Creator
{
    #region Form
    public partial class MainForm : Form
    {
        Theme theme = null;
        string folderName = null;

        public MainForm()
        {
            InitializeComponent();
        }

        #region Engine
        public void SerializeObject<T>(T serializableObject, string fileName)
        {
            if (serializableObject == null) { return; }
            string results = string.Empty;

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(serializableObject.GetType());
                string themeID = new DirectoryInfo(folderName).Name;
                string path = string.Format(@"{0}\{1}.xml", folderName, themeID);
                using (MemoryStream stream = new MemoryStream())
                {
                    serializer.Serialize(stream, serializableObject);
                    stream.Position = 0;
                    xmlDocument.Load(stream);
                    xmlDocument.Save(path);
                    stream.Close();
                    results = "Theme generated sucessfully";
                    lblResults.ForeColor = Color.LightSeaGreen;
                    lnkBrowse.Text = "Browse Theme Folder";
                    lnkBrowse.Links.Clear();
                    lnkBrowse.Links.Add(0, 19, folderName);
                }
            }
            catch (Exception e)
            {
                results = "Error Creating Theme-See Log";
                string log = folderName + @"\error-Log.txt";
                //String FolderPath = Environment.ExpandEnvironmentVariables("C:\\User\\LogFile.txt");
                FileStream fs = null;
                if (!File.Exists(log))
                {
                    using (fs = File.Create(log)) { }
                }

                try
                {
                    if (!string.IsNullOrEmpty(e.Message))
                    {
                        using (FileStream file = new FileStream(log, FileMode.OpenOrCreate, FileAccess.Write))
                        {
                            StreamWriter streamWriter = new StreamWriter(file);
                            streamWriter.WriteLine(System.DateTime.Now.ToString() + " : " + fileName + ".xml");
                            streamWriter.WriteLine(e.Message);
                            streamWriter.WriteLine("-------");
                            streamWriter.WriteLine(e.StackTrace);
                            streamWriter.WriteLine();
                            streamWriter.Close();
                        }
                    }
                }
                catch
                { //ignore
                }

                lblResults.ForeColor = Color.IndianRed;
                lnkBrowse.Text = "Error Log";
                lnkBrowse.Links.Clear();
                lnkBrowse.Links.Add(0, 9, log);
            }
            finally
            {
                lblResults.Text = results;
            }
        }

        private void SetDetails(ref Channel channel)
        {
            string id = channel.ID;
            string name = channel.Name;
            string image = channel.Image.Substring(0, channel.Image.LastIndexOf('.'));

            if (name == null) name = image;
            if (id == null) id = image;
            //set first letter to caps
            name = name.Substring(0, 1).ToUpper() + name.Substring(1);

            channel.ID = id;
            channel.Name = name;

            txtID.Text = id;
            txtName.Text = name;
        }
        #endregion

        #region Form Functionality
        private void btnOpenFolder_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            DialogResult = folderBrowserDialog.ShowDialog();
            if (DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                theme = new Theme();
                folderName = folderBrowserDialog.SelectedPath;
                string[] icons = Directory.GetFiles(folderName).ToArray();
                for (int i = 0; i < icons.Length; i++)
                {
                    FileInfo image = new FileInfo(icons[i]);
                    Channel channel = new Channel { Image = image.Name };
                    theme.Channels.Add(channel);
                }
                lstIcons.DataSource = theme.Channels;
                lstIcons.Refresh();
            }
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                theme.ID = txtThemeID.Text;
                theme.Description = txtThemeDescription.Text;
                theme.CreatedDate = txtCreationDate.Text;
                theme.Creator = txtCreatedBy.Text;
                theme.Url = txtUrl.Text;

                SerializeObject<Theme>(theme, theme.ID);
            }
        }
        private void btnSet_Click(object sender, EventArgs e)
        {
            if (theme != null)
            {
                IEnumerable<Channel> channel = from ch in theme.Channels
                                               where ch.Image == lstIcons.SelectedValue.ToString()
                                               select ch;

                channel.FirstOrDefault().Name = txtName.Text;
                channel.FirstOrDefault().ID = txtID.Text;
            }
        }

        private void lstIcons_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtImg.Text = ((ListBox)sender).SelectedValue.ToString();
            IEnumerable<Channel> channel = from ch in theme.Channels
                                           where ch.Image == lstIcons.SelectedValue.ToString()
                                           select ch;
            Channel chx = channel.FirstOrDefault();
            SetDetails(ref chx);
        }

        private void btnAutoSet_Click(object sender, EventArgs e)
        {
            if (theme != null)
            {
                for (int i = 0; i < theme.Channels.Count; i++)
                {
                    Channel chx = theme.Channels[i];
                    SetDetails(ref chx);

                    theme.Channels[i] = chx;
                    lstIcons.SelectedIndex = i;
                    txtImg.Text = lstIcons.SelectedValue.ToString();
                }
            }
        }
        private bool ValidateForm()
        {
            if (txtThemeID.Text == string.Empty)
            {
                errMsg.SetError(txtThemeID, "Theme needs an ID");
                return false;
            }
            else
            {
                errMsg.Clear();
                return true;
            }
        }
        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.BackColor = Color.DarkOrange;
            this.TransparencyKey = Color.DarkOrange;
        }

        private void btnMinimize_MouseHover(object sender, EventArgs e)
        {
            btnMinimize.Image = Properties.Resources.Minus_hover;
        }

        private void btnClose_MouseHover(object sender, EventArgs e)
        {
            btnClose.Image = Properties.Resources.Close_hover;
        }

        private void btnMinimize_MouseLeave(object sender, EventArgs e)
        {
            btnMinimize.Image = Properties.Resources.Minus;
        }

        private void btnClose_MouseLeave(object sender, EventArgs e)
        {
            btnClose.Image = Properties.Resources.Close;

        }

        private void lnkBrowse_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            lnkBrowse.LinkVisited = true;
            System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
        }
        #endregion
    }
    #endregion

    #region Classes
    [DataContract]
    [XmlRoot("channel")]
    public class Channel
    {
        [XmlAttribute("id")]
        public string ID { get; set; }
        [XmlAttribute("name")]
        public string Name { get; set; }
        [XmlAttribute("image")]
        public string Image { get; set; }

        public override string ToString()
        {
            return Image;
        }
    }

    [DataContract]
    [XmlRoot(Namespace = "http://zueuz.com/Theme-Schema.xsd", ElementName = "theme", IsNullable = false)]
    public class Theme
    {
        private List<Channel> _channels = new List<Channel>();
        [XmlElement("channel")]
        public List<Channel> Channels { get { return _channels; } }

        [XmlAttribute("id")]
        public string ID { get; set; }
        [XmlAttribute("description")]
        public string Description { get; set; }
        [XmlAttribute("created-date")]
        public string CreatedDate { get; set; }
        [XmlAttribute("created-by")]
        public string Creator { get; set; }
        [XmlAttribute("url-reference")]
        public string Url { get; set; }
    }
    #endregion
}
