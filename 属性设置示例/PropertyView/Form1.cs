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


        [Category("文档设置"), DefaultValue(true)]
        public bool SaveOnClose
        {
            get { return saveOnClose; }
            set { saveOnClose = value; }
        }

        [Category("文档设置")]
        public Size WindowSize
        {
            get { return windowSize; }
            set { windowSize = value; }
        }

        [Category("文档设置")]
        public Font WindowFont
        {
            get { return windowFont; }
            set { windowFont = value; }
        }

        [Category("全局设置")]
        public Color ToolbarColor
        {
            get { return toolbarColor; }
            set { toolbarColor = value; }
        }

        [Category("全局设置"), ReadOnly(true), DefaultValue("欢迎使用应用程序！")]
        public string GreetingText
        {
            get { return greetingText; }
            set { greetingText = value; }
        }

        [Category("全局设置"), DefaultValue(4)]
        public int ItemsInMRUList
        {
            get { return itemsInMRU; }
            set { itemsInMRU = value; }
        }

        [Description("以毫秒表示的文本重复率。"), Category("全局设置"), DefaultValue(10)]
        public int MaxRepeatRate
        {
            get { return maxRepeatRate; }
            set { maxRepeatRate = value; }
        }

        [Browsable(false), DefaultValue(false)]
        public bool SettingsChanged
        {
            get { return settingsChanged; }
            set { settingsChanged = value; }
        }

        [Category("版本"), DefaultValue("1.0"), ReadOnly(true)]
        public string AppVersion
        {
            get { return appVersion; }
            set { appVersion = value; }
        }

        [Category("拼写检查选项"), DefaultValue("DefaultFileName")]
        public string SpellingOptions
        {
            get { return this.DefaultFileName; }
            set { this.DefaultFileName = value; }
        }
    }

    public class AppSettings
    {
        private string extendDirection = "横向";
        private string extendConfig = "横向";
        private string dataGroup = "不分组";
        private string dataCollection = "合计";
        private string blankLine = "不补充";

        [CategoryAttribute("扩展方向"),
        DefaultValueAttribute("横向"),
         TypeConverter(typeof(ExtendDirection))]
        public string 扩展方向
        {
            get { return extendDirection; }
            set { extendDirection = value; }
        }

        [CategoryAttribute("可扩展性"),
         DefaultValueAttribute("横向"),
         TypeConverter(typeof(ExtendConfig))]
        public string 可扩展性
        {
            get { return extendConfig; }
            set { extendConfig = value; }
        }

        [CategoryAttribute("数据分组设置"),
         DefaultValueAttribute("不分组"),
         TypeConverter(typeof(DataGroup))]
        public string 分组
        {
            get { return dataGroup; }
            set { dataGroup = value; }
        }

        [CategoryAttribute("数据分组设置"),
         DefaultValueAttribute("合计"),
         TypeConverter(typeof(DataCollection))]
        public string 汇总
        {
            get { return dataCollection; }
            set { dataCollection = value; }
        }

        [CategoryAttribute("补充空白行"),
        DefaultValueAttribute("不补充"),
        TypeConverter(typeof(BlankLine))]
        public string 补充空白行
        {
            get { return blankLine; }
            set { blankLine = value; }
        }

    }

    /// <summary>  
    /// 扩展方向配置  
    /// </summary>  
    public class ExtendDirection : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new string[] { "横向", "纵向" });
        }
    }

    /// <summary>  
    /// 可扩展性配置  
    /// </summary>  
    public class ExtendConfig : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new string[] { "横向", "纵向" });
        }
    }

    /// <summary>  
    /// 数据分组设置  
    /// </summary>  
    public class DataGroup : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new string[] { "不分组", "相同分组" });
        }
    }

    /// <summary>  
    /// 数据汇总设置  
    /// </summary>  
    public class DataCollection : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new string[] { "合计", "平均", "最大", "最小", "记录数" });
        }
    }


    /// <summary>  
    /// 补充空白行  
    /// </summary>  
    public class BlankLine : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(new string[] { "不补充", "补满页", "固定行" });
        }
    }
}


