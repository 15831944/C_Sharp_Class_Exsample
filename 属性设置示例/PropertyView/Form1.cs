using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace PropertyView
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            AppSettings1 appset = new AppSettings1();
            this.propertyGridControl1.SelectedObject = appset;
        }
    }


    [DefaultPropertyAttribute("SaveOnClose")]
    public class AppSettings1
    {
        private bool saveOnClose = true;
        private string greetingText = "欢迎使用应用程序！";
        private int maxRepeatRate = 10;
        private int itemsInMRU = 4;
        private bool settingsChanged = false;
        private string appVersion = "1.0";
        private string DefaultFileName = "Name";

        private Size windowSize = new Size(100, 100);
        private Font windowFont = new Font("宋体", 9, FontStyle.Regular);
        private Color toolbarColor = SystemColors.Control;
        [CategoryAttribute("文档设置"),
        DefaultValueAttribute(true)]
        public bool SaveOnClose
        {
            get { return saveOnClose; }
            set { saveOnClose = value; }
        }
        [CategoryAttribute("文档设置")]
        public Size WindowSize
        {
            get { return windowSize; }
            set { windowSize = value; }
        }
        [CategoryAttribute("文档设置")]
        public Font WindowFont
        {
            get { return windowFont; }
            set { windowFont = value; }
        }
        [CategoryAttribute("全局设置")]
        public Color ToolbarColor
        {
            get { return toolbarColor; }
            set { toolbarColor = value; }
        }
        [CategoryAttribute("全局设置"),
        ReadOnlyAttribute(true),
        DefaultValueAttribute("欢迎使用应用程序！")]
        public string GreetingText
        {
            get { return greetingText; }
            set { greetingText = value; }
        }
        [CategoryAttribute("全局设置"),
        DefaultValueAttribute(4)]
        public int ItemsInMRUList
        {
            get { return itemsInMRU; }
            set { itemsInMRU = value; }
        }
        [DescriptionAttribute("以毫秒表示的文本重复率。"),
        CategoryAttribute("全局设置"),
        DefaultValueAttribute(10)]
        public int MaxRepeatRate
        {
            get { return maxRepeatRate; }
            set { maxRepeatRate = value; }
        }
        [BrowsableAttribute(false),
        DefaultValueAttribute(false)]
        public bool SettingsChanged
        {
            get { return settingsChanged; }
            set { settingsChanged = value; }
        }
        [CategoryAttribute("版本"),
        DefaultValueAttribute("1.0"),
        ReadOnlyAttribute(true)]
        public string AppVersion
        {
            get { return appVersion; }
            set { appVersion = value; }
        }
        [CategoryAttribute("拼写检查选项"),
        DefaultValueAttribute("DefaultFileName")]
        public string SpellingOptions
        {
            get { return this.DefaultFileName; }
            set { this.DefaultFileName = value; }
        }
    }
}


