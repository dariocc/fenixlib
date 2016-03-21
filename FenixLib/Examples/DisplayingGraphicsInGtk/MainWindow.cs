using System;
using System.Linq;
using Gtk;
using FenixLib.Core;
using FenixLib.IO;
using FenixLib.Gdk;

public partial class MainWindow : Gtk.Window
{
    private const int GraphicColum = 0;
    private ISpriteAssortment spriteAssorment = null;
    private IBitmapFont BitmapFont = null;
    private FileChooserDialog openDialog;

    public MainWindow ()
        : base ( Gtk.WindowType.Toplevel )
    {
        Build ();

        openDialog = new Gtk.FileChooserDialog ( 
            "Open file",
            this,
            FileChooserAction.Open,
            "Cancel", ResponseType.Cancel,
            "Open", ResponseType.Accept );

        // Define tree views as single-selection and connect the 
        // Changed signal. Notice the signal handler is the same
        // in both cases

        spriteAssortmentView.Selection.Mode = SelectionMode.Single;
        spriteAssortmentView.Selection.Changed += OnSelectionChanged;

        bitmapFontView.Selection.Mode = SelectionMode.Single;
        bitmapFontView.Selection.Changed += OnSelectionChanged;

    }

    void UpdateViewWithGraphic ( IGraphic graphic )
    {
        previewImage.Pixbuf = graphic.ToPixBuf ();
    }

    void UpdateViewWithSpriteAssortment ( ISpriteAssortment spriteAssortment )
    {
        if ( spriteAssorment == null )
        {
            throw new ArgumentNullException ();
        }

        // The columns of the TreeView
        var descriptionColumn = new TreeViewColumn ();
        descriptionColumn.Title = "Description";
        var idColumn = new TreeViewColumn ();
        idColumn.Title = "Id";

        spriteAssortmentView.AppendColumn ( idColumn );
        spriteAssortmentView.AppendColumn ( descriptionColumn );

        // A renderer for the data in the model
        var renderer = new CellRendererText ();

        idColumn.PackStart ( renderer, false );
        idColumn.AddAttribute ( renderer, "text", 1 );
        descriptionColumn.PackStart ( renderer, true );
        descriptionColumn.AddAttribute ( renderer, "text", 2 );

        // Model for the TreeView
        // The first column of the model is an IGraphic. We do the same
        // for the BitmapFont TreeView, so as the OnSelectionChanged
        // can be reused.
        var model = new ListStore ( 
            typeof( IGraphic ),         // The graphic itself
            typeof ( int ),             // Id of the sprite
            typeof ( string )           // Description of the sprite
            );
        foreach ( var sprite in spriteAssorment )
        {
            model.AppendValues ( sprite, sprite.Id, sprite.Description );
        }
            
        spriteAssortmentView.Model = model;
    }

    protected void OnDeleteEvent ( object sender, DeleteEventArgs a )
    {
        Application.Quit ();
        a.RetVal = true;
    }

    protected void OnOpenFpgClicked ( object sender, EventArgs e )
    {
        var filter = new FileFilter ();
        filter.AddPattern ( "*.fpg" );
        filter.Name = "Fpg files";

        openDialog.Filter = null;

        if ( openDialog.Run () == ( int ) ResponseType.Accept )
        {
            spriteAssorment = NativeFile.LoadFpg ( openDialog.Filename );
            UpdateViewWithSpriteAssortment ( spriteAssorment );
        }

        openDialog.Destroy ();
    }

    protected void OnOpenMapClicked ( object sender, EventArgs e )
    {
        var filter = new FileFilter ();
        filter.Name = "Map files (*.map)";
        filter.AddPattern ( "*.map" );

        openDialog.Filter = filter;

        if ( openDialog.Run () == ( int ) ResponseType.Accept )
        {
            var sprite = NativeFile.LoadMap ( openDialog.Filename );
            UpdateViewWithGraphic ( sprite );
        }		

        openDialog.Destroy ();
    }

    // This event handler is used for both the sprite assortment and bitmap
    // font tree views
    protected void OnSelectionChanged ( object sender, EventArgs e )
    {   
        var treeSelection = sender as TreeSelection;
        var selectedRows = treeSelection.GetSelectedRows ();

        if ( !( selectedRows.Length > 0 ) )
        {
            return;
        }
            
        var treeView = treeSelection.TreeView;
        var model = treeView.Model;
        var iterPath = selectedRows[0];

        TreeIter iter;
        if ( model.GetIter ( out iter, iterPath ) )
        {
            var graphic = (IGraphic) model.GetValue ( iter, GraphicColum);

            UpdateViewWithGraphic ( graphic );
        }
    }
}
