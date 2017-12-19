using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MefDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Init();
        }
        [ImportMany]
        public IEnumerable<IPlugin> plugins;
        private void Form1_Load(object sender, EventArgs e)
        {
            
            ToolStripMenuItem item = new ToolStripMenuItem("插件");
            ms.Items.Add(item);
            foreach (IPlugin plugin in plugins)
            {
                ToolStripMenuItem subItem = new ToolStripMenuItem(plugin.Text);
                subItem.Click += (s, arg) => { plugin.Do(); };
                item.DropDownItems.Add(subItem);
            }
        }

        private CompositionContainer _container;
        private void Init()
        {
          
            //设置目录，让引擎能自动去发现新的扩展
            var catalog = new AggregateCatalog();
            catalog.Catalogs.Add(new DirectoryCatalog(AppDomain.CurrentDomain.BaseDirectory+"plugin\\"));

            //创建一个容器，相当于是生产车间
            _container = new CompositionContainer(catalog);

            //调用车间的ComposeParts把各个部件组合到一起
            try
            {
                this._container.ComposeParts(this);//这里只需要传入当前应用程序实例就可以了，其它部分会自动发现并组装
            }
            catch (CompositionException compositionException)
            {
                Console.WriteLine(compositionException.ToString());
            }
        }
    }
}
