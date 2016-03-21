
// This file has been generated by the GUI designer. Do not modify.

public partial class MainWindow
{
	private global::Gtk.VBox vbox1;
	
	private global::Gtk.VBox vbox5;
	
	private global::Gtk.Label label1;
	
	private global::Gtk.Label label2;
	
	private global::Gtk.HBox hbox2;
	
	private global::Gtk.VBox vbox4;
	
	private global::Gtk.Image image1;
	
	private global::Gtk.Button button4;
	
	private global::Gtk.VBox vbox2;
	
	private global::Gtk.ScrolledWindow GtkScrolledWindow;
	
	private global::Gtk.TreeView treeview1;
	
	private global::Gtk.Button Load;
	
	private global::Gtk.VBox vbox3;
	
	private global::Gtk.ScrolledWindow GtkScrolledWindow1;
	
	private global::Gtk.TreeView treeview2;
	
	private global::Gtk.Button Load1;

	protected virtual void Build ()
	{
		global::Stetic.Gui.Initialize (this);
		// Widget MainWindow
		this.Name = "MainWindow";
		this.Title = global::Mono.Unix.Catalog.GetString ("MainWindow");
		this.WindowPosition = ((global::Gtk.WindowPosition)(4));
		this.AllowShrink = true;
		// Container child MainWindow.Gtk.Container+ContainerChild
		this.vbox1 = new global::Gtk.VBox ();
		this.vbox1.Name = "vbox1";
		this.vbox1.Spacing = 6;
		// Container child vbox1.Gtk.Box+BoxChild
		this.vbox5 = new global::Gtk.VBox ();
		this.vbox5.Name = "vbox5";
		this.vbox5.Spacing = 6;
		// Container child vbox5.Gtk.Box+BoxChild
		this.label1 = new global::Gtk.Label ();
		this.label1.Name = "label1";
		this.label1.LabelProp = global::Mono.Unix.Catalog.GetString ("<b>Example of FenixLib use with Gtk</b>");
		this.label1.UseMarkup = true;
		this.vbox5.Add (this.label1);
		global::Gtk.Box.BoxChild w1 = ((global::Gtk.Box.BoxChild)(this.vbox5 [this.label1]));
		w1.Position = 0;
		w1.Expand = false;
		w1.Fill = false;
		// Container child vbox5.Gtk.Box+BoxChild
		this.label2 = new global::Gtk.Label ();
		this.label2.Name = "label2";
		this.label2.LabelProp = global::Mono.Unix.Catalog.GetString ("Use one of the \"Open\" buttons below to open a PixTudio / BennuGD / Div resource.");
		this.vbox5.Add (this.label2);
		global::Gtk.Box.BoxChild w2 = ((global::Gtk.Box.BoxChild)(this.vbox5 [this.label2]));
		w2.Position = 1;
		w2.Expand = false;
		w2.Fill = false;
		this.vbox1.Add (this.vbox5);
		global::Gtk.Box.BoxChild w3 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.vbox5]));
		w3.Position = 0;
		w3.Expand = false;
		w3.Fill = false;
		// Container child vbox1.Gtk.Box+BoxChild
		this.hbox2 = new global::Gtk.HBox ();
		this.hbox2.Name = "hbox2";
		this.hbox2.Spacing = 6;
		// Container child hbox2.Gtk.Box+BoxChild
		this.vbox4 = new global::Gtk.VBox ();
		this.vbox4.Name = "vbox4";
		this.vbox4.Spacing = 6;
		// Container child vbox4.Gtk.Box+BoxChild
		this.image1 = new global::Gtk.Image ();
		this.image1.WidthRequest = 400;
		this.image1.HeightRequest = 400;
		this.image1.Name = "image1";
		this.vbox4.Add (this.image1);
		global::Gtk.Box.BoxChild w4 = ((global::Gtk.Box.BoxChild)(this.vbox4 [this.image1]));
		w4.Position = 0;
		w4.Expand = false;
		w4.Fill = false;
		// Container child vbox4.Gtk.Box+BoxChild
		this.button4 = new global::Gtk.Button ();
		this.button4.CanFocus = true;
		this.button4.Name = "button4";
		this.button4.UseUnderline = true;
		this.button4.Label = global::Mono.Unix.Catalog.GetString ("Open map");
		this.vbox4.Add (this.button4);
		global::Gtk.Box.BoxChild w5 = ((global::Gtk.Box.BoxChild)(this.vbox4 [this.button4]));
		w5.Position = 1;
		w5.Expand = false;
		w5.Fill = false;
		this.hbox2.Add (this.vbox4);
		global::Gtk.Box.BoxChild w6 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.vbox4]));
		w6.Position = 0;
		w6.Expand = false;
		w6.Fill = false;
		// Container child hbox2.Gtk.Box+BoxChild
		this.vbox2 = new global::Gtk.VBox ();
		this.vbox2.Name = "vbox2";
		this.vbox2.Spacing = 6;
		// Container child vbox2.Gtk.Box+BoxChild
		this.GtkScrolledWindow = new global::Gtk.ScrolledWindow ();
		this.GtkScrolledWindow.WidthRequest = 200;
		this.GtkScrolledWindow.Name = "GtkScrolledWindow";
		this.GtkScrolledWindow.ShadowType = ((global::Gtk.ShadowType)(1));
		// Container child GtkScrolledWindow.Gtk.Container+ContainerChild
		this.treeview1 = new global::Gtk.TreeView ();
		this.treeview1.CanFocus = true;
		this.treeview1.Name = "treeview1";
		this.GtkScrolledWindow.Add (this.treeview1);
		this.vbox2.Add (this.GtkScrolledWindow);
		global::Gtk.Box.BoxChild w8 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.GtkScrolledWindow]));
		w8.Position = 0;
		// Container child vbox2.Gtk.Box+BoxChild
		this.Load = new global::Gtk.Button ();
		this.Load.CanFocus = true;
		this.Load.Name = "Load";
		this.Load.UseUnderline = true;
		this.Load.Label = global::Mono.Unix.Catalog.GetString ("Open Fpg");
		this.vbox2.Add (this.Load);
		global::Gtk.Box.BoxChild w9 = ((global::Gtk.Box.BoxChild)(this.vbox2 [this.Load]));
		w9.Position = 1;
		w9.Expand = false;
		w9.Fill = false;
		this.hbox2.Add (this.vbox2);
		global::Gtk.Box.BoxChild w10 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.vbox2]));
		w10.Position = 1;
		w10.Expand = false;
		w10.Fill = false;
		// Container child hbox2.Gtk.Box+BoxChild
		this.vbox3 = new global::Gtk.VBox ();
		this.vbox3.Name = "vbox3";
		this.vbox3.Spacing = 6;
		// Container child vbox3.Gtk.Box+BoxChild
		this.GtkScrolledWindow1 = new global::Gtk.ScrolledWindow ();
		this.GtkScrolledWindow1.WidthRequest = 200;
		this.GtkScrolledWindow1.Name = "GtkScrolledWindow1";
		this.GtkScrolledWindow1.ShadowType = ((global::Gtk.ShadowType)(1));
		// Container child GtkScrolledWindow1.Gtk.Container+ContainerChild
		this.treeview2 = new global::Gtk.TreeView ();
		this.treeview2.CanFocus = true;
		this.treeview2.Name = "treeview2";
		this.GtkScrolledWindow1.Add (this.treeview2);
		this.vbox3.Add (this.GtkScrolledWindow1);
		global::Gtk.Box.BoxChild w12 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.GtkScrolledWindow1]));
		w12.Position = 0;
		// Container child vbox3.Gtk.Box+BoxChild
		this.Load1 = new global::Gtk.Button ();
		this.Load1.CanFocus = true;
		this.Load1.Name = "Load1";
		this.Load1.UseUnderline = true;
		this.Load1.Label = global::Mono.Unix.Catalog.GetString ("Open Fnt");
		this.vbox3.Add (this.Load1);
		global::Gtk.Box.BoxChild w13 = ((global::Gtk.Box.BoxChild)(this.vbox3 [this.Load1]));
		w13.Position = 1;
		w13.Expand = false;
		w13.Fill = false;
		this.hbox2.Add (this.vbox3);
		global::Gtk.Box.BoxChild w14 = ((global::Gtk.Box.BoxChild)(this.hbox2 [this.vbox3]));
		w14.Position = 2;
		w14.Expand = false;
		w14.Fill = false;
		this.vbox1.Add (this.hbox2);
		global::Gtk.Box.BoxChild w15 = ((global::Gtk.Box.BoxChild)(this.vbox1 [this.hbox2]));
		w15.Position = 1;
		w15.Expand = false;
		w15.Fill = false;
		this.Add (this.vbox1);
		if ((this.Child != null)) {
			this.Child.ShowAll ();
		}
		this.DefaultWidth = 834;
		this.DefaultHeight = 540;
		this.Show ();
		this.DeleteEvent += new global::Gtk.DeleteEventHandler (this.OnDeleteEvent);
		this.button4.Clicked += new global::System.EventHandler (this.OnOpenMapClick);
		this.treeview1.RowActivated += new global::Gtk.RowActivatedHandler (this.OnFpgTreeRowActivate);
		this.Load.Clicked += new global::System.EventHandler (this.OnOpenFpgClicked);
	}
}
