1.说明

weiFenLuo.winFormsUI.Docking.dll是开源项目DockPanel Suite的一个类库，可以实现像Visual Studio的窗口停靠、拖拽等功能。WeifenLuo.WinFormsUI.Docking是一个很强大的界面布局控件,可以保存自定义的布局为XML文件,可以加载XML配置文件。

该动态库源自开源项目DockPanelSuite，原始下载链接如下：

http://sourceforge.net/projects/dockpanelsuite/files/?source=navbar

但目前，该动态库已经改至以下链接进行下载与维护：

https://github.com/dockpanelsuite/dockpanelsuite

可下载其源码、可直接使用的dll及示例程序，目前已更新至2.8，可在VS2012环境下使用。

2.使用

dockpanel中提供了几个可用的类, 重要的有两个, 一是DockPanel, 一是DockContent：

DockPanel是从panel继承出来的, 用于提供可浮动的dock的子窗口进行浮动和dock的场所,
DockContent是从form类中继承出来的, 用于提供可浮动的窗口基类. 就是说: DockContent对象可以在DockPanel对象中任意贴边, 浮动, TAB化等.  

  添加引用：
     1）引用—>添加引用—>浏览—>weiFenLuo.winFormsUI.Docking.dll。
     2）窗体属性IsMdiContainer:True。
     3）工具箱—>右键—>选择项—>.net组件—>浏览—>weiFenLuo.winFormsUI.Docking.dll—>在工具箱出现dockPanel。
     4）将dockPanel拖到窗体Form1上，设置Dock属性为：Fill。
  停靠窗体：
     1.新建一个WinForm窗体Form2。
     2.在代码中修改窗体继承于DockContent。
     public partial class Form2 : DockContent
     3.在主窗体Form1中显示停靠窗体。
     private void Form1_Load(object sender, EventArgs e)
     {
　　     Form2 form2 = new Form2();
　　     form2.Show(this.dockPanel1);
　　     form2.DockTo(this.dockPanel1, DockStyle.Left);
     }