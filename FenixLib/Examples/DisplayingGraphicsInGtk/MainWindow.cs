using System;
using System.Linq;
using Gtk;
using FenixLib.Core;
using FenixLib.IO;
using FenixLib.Gdk;

public partial class MainWindow : Gtk.Window
{
	private ISpriteAssortment spriteAssorment = null;
	private IBitmapFont BitmapFont = null;

    public MainWindow ()
        : base ( Gtk.WindowType.Toplevel )
    {
        Build ();
    }

    protected void OnDeleteEvent ( object sender, DeleteEventArgs a )
    {
        Application.Quit ();
        a.RetVal = true;
    }

    protected void OnOpenFpgClicked ( object sender, EventArgs e )
    {
        FileChooserDialog dialog = new Gtk.FileChooserDialog ( 
            "Choose any Fpg file",
            this,
            FileChooserAction.Open,
            "Cancel", ResponseType.Cancel,
            "Open", ResponseType.Accept );

		var filter = new FileFilter ();
		filter.AddPattern ("*.fpg");
		filter.Name = "Fpg files";

		dialog.Filter = filter;

        if ( dialog.Run () == ( int ) ResponseType.Accept )
        {
            spriteAssorment = NativeFile.LoadFpg ( dialog.Filename );
        }

		dialog.Destroy ();

		if (spriteAssorment == null) 
		{
			return;
		}

        Gtk.ListStore store = new ListStore ( typeof ( SpriteAssortmentSprite ) );
        foreach ( var sprite in spriteAssorment )
        {
			store.AppendValues ( new FpgTreeViewElement(sprite.Id, sprite.Description) );
        }
			
        TreeViewColumn column = new TreeViewColumn ();
        column.Title = "Sprite Name";
        treeview1.AppendColumn ( column );
        treeview1.Model = store;

        CellRendererText nameCell = new CellRendererText ();
        column.PackStart ( nameCell, false );

        column.AddAttribute ( nameCell, "text", 0 );
    }

	protected void OnOpenMapClick (object sender, EventArgs e)
	{
		FileChooserDialog dialog = new Gtk.FileChooserDialog ( 
			"Choose any Map File",
			this,
			FileChooserAction.Open,
			"Cancel", ResponseType.Cancel,
			"Open", ResponseType.Accept );

		var filter = new FileFilter ();
		filter.AddPattern ("*.map");
		filter.Name = "Map files";

		dialog.Filter = filter;

		if ( dialog.Run () == ( int ) ResponseType.Accept )
		{
			var sprite = NativeFile.LoadMap ( dialog.Filename );
			image1.Pixbuf = sprite.ToPixBuf ();
		}		

		dialog.Destroy ();
	}

	protected void OnFpgTreeRowActivate (object o, RowActivatedArgs args)
	{
		var model = treeview1.Model;
		TreeIter iter;
		model.GetIter (out iter, args.Path);
		var value = (FpgTreeViewElement) model.GetValue (iter, 0);

		if (value != null) 
		{
			var sprite = spriteAssorment [value.Id];
			image1.Pixbuf = sprite.ToPixBuf ();
		}

	}		


	private class FpgTreeElementRenderer : CellRenderer
	{
		protected override void Render (Gdk.Drawable window, Widget widget, Gdk.Rectangle background_area, Gdk.Rectangle cell_area, Gdk.Rectangle expose_area, CellRendererState flags)
		{
			
			base.Render (window, widget, background_area, cell_area, expose_area, flags);
		}
			
	}

		
	private class FpgTreeViewElement
	{
		public int Id { get; }
		public string Name { get; }

		public FpgTreeViewElement(int id, string name)
		{
			this.Id = id;
			this.Name = name;
		}

		public override string ToString ()
		{
			return string.Format ($"{Id}: {Name}");
		}
	}
}
