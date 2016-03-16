using System;
using Gtk;
using FenixLib.Core;
using FenixLib.IO;

public partial class MainWindow : Gtk.Window
{
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

    protected void OnGtkButtonClick ( object sender, EventArgs e )
    {
        FileChooserDialog dialog = new Gtk.FileChooserDialog ( 
            "Choose the file to open",
            this,
            FileChooserAction.Open,
            "Cancel", ResponseType.Cancel,
            "Open", ResponseType.Accept );

        ISpriteAssortment fpg = null;

        if ( dialog.Run () == ( int ) ResponseType.Accept )
        {
            fpg = NativeFile.LoadFpg ( dialog.Filename );
        }

        Gtk.ListStore store = new ListStore ( typeof ( string ) );
        foreach ( Sprite sprite in fpg )
        {
            store.AppendValues ( sprite.Description );
        }

        dialog.Destroy ();

        TreeViewColumn column = new TreeViewColumn ();
        column.Title = "Name";
        treeview1.AppendColumn ( column );
        treeview1.Model = store;

        CellRendererText nameCell = new CellRendererText ();
        column.PackStart ( nameCell, false );

        column.AddAttribute ( nameCell, "text", 0 );
    }
}
