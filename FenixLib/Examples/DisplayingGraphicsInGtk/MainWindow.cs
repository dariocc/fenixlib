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
    private IBitmapFont bitmapFont = null;

    public MainWindow ()
        : base ( Gtk.WindowType.Toplevel )
    {
        Build ();

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
            typeof ( IGraphic ),         // The graphic itself
            typeof ( int ),             // Id of the sprite
            typeof ( string )           // Description of the sprite
            );
        foreach ( var sprite in spriteAssorment )
        {
            model.AppendValues ( sprite, sprite.Id, sprite.Description );
        }

        spriteAssortmentView.Model = model;
    }


    private void UpdateViewWithBitmapFont ( IBitmapFont bitmapFont )
    {
        if ( bitmapFont == null )
        {
            throw new ArgumentNullException ();
        }

        // The columns of the TreeView
        var characterColumn = new TreeViewColumn ();
        characterColumn.Title = "Character";
        var widthColumn = new TreeViewColumn ();
        widthColumn.Title = "Width";
        var heightColumn = new TreeViewColumn ();
        heightColumn.Title = "Height";
        var xAdvanceColumn = new TreeViewColumn ();
        xAdvanceColumn.Title = "XAdvance";
        var yAdvanceColumn = new TreeViewColumn ();
        yAdvanceColumn.Title = "YAdvance";
        var xOffsetColumn = new TreeViewColumn ();
        xOffsetColumn.Title = "XOffset";
        var yOffsetColumn = new TreeViewColumn ();
        yOffsetColumn.Title = "YOffset";

        bitmapFontView.AppendColumn ( characterColumn );
        bitmapFontView.AppendColumn ( widthColumn );
        bitmapFontView.AppendColumn ( heightColumn );
        bitmapFontView.AppendColumn ( xAdvanceColumn );
        bitmapFontView.AppendColumn ( yAdvanceColumn );
        bitmapFontView.AppendColumn ( xOffsetColumn );
        bitmapFontView.AppendColumn ( yOffsetColumn );

        // A renderer for the data in the model
        var renderer = new CellRendererText ();

        characterColumn.PackStart ( renderer, false );
        characterColumn.AddAttribute ( renderer, "text", 1 );
        widthColumn.PackStart ( renderer, false );
        widthColumn.AddAttribute ( renderer, "text", 2 );
        heightColumn.PackStart ( renderer, false );
        heightColumn.AddAttribute ( renderer, "text", 3 );
        xAdvanceColumn.PackStart ( renderer, false );
        xAdvanceColumn.AddAttribute ( renderer, "text", 4 );
        yAdvanceColumn.PackStart ( renderer, false );
        yAdvanceColumn.AddAttribute ( renderer, "text", 5 );
        xOffsetColumn.PackStart ( renderer, false );
        xOffsetColumn.AddAttribute ( renderer, "text", 6 );
        yOffsetColumn.PackStart ( renderer, false );
        yOffsetColumn.AddAttribute ( renderer, "text", 7 );


        // Model for the TreeView
        // The first column of the model is an IGraphic. We do the same
        // for the BitmapFont TreeView, so as the OnSelectionChanged
        // can be reused.
        var model = new ListStore (
            typeof ( IGraphic ),          // The graphic itself
            typeof ( string ),              // Associated character
            typeof ( int ),               // Width
            typeof ( int ),               // Height
            typeof ( int ),               // XAdvance
            typeof ( int ),               // YAdvance
            typeof ( int ),               // XOffset
            typeof ( int )                // YOffset
            );
        foreach ( var glyph in bitmapFont )
        {
            model.AppendValues ( glyph, 
                glyph.Character.ToString(),
                glyph.Width, 
                glyph.Height,
                glyph.XAdvance, 
                glyph.YAdavance,
                glyph.XOffset, 
                glyph.YOffset );
        }

        bitmapFontView.Model = model;
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

        var openDialog = CreateOpenDialog ();
        openDialog.Filter = null;

        if ( openDialog.Run () == ( int ) ResponseType.Accept )
        {
            spriteAssorment = NativeFile.LoadFpg ( openDialog.Filename );
            UpdateViewWithSpriteAssortment ( spriteAssorment );
        }

        openDialog.Destroy ();
    }

    protected void OnOpenFntClicked ( object sender, EventArgs e )
    {
        var filter = new FileFilter ();
        filter.AddPattern ( "*.fnt" );
        filter.Name = "Fnt files";

        var openDialog = CreateOpenDialog ();
        openDialog.Filter = null;

        if ( openDialog.Run () == ( int ) ResponseType.Accept )
        {
            bitmapFont = NativeFile.LoadFnt ( openDialog.Filename );
            UpdateViewWithBitmapFont ( bitmapFont );
        }

        openDialog.Destroy ();
    }

    protected void OnOpenMapClicked ( object sender, EventArgs e )
    {
        var filter = new FileFilter ();
        filter.Name = "Map files (*.map)";
        filter.AddPattern ( "*.map" );

        var openDialog = CreateOpenDialog ();
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
            var graphic = ( IGraphic ) model.GetValue ( iter, GraphicColum );

            UpdateViewWithGraphic ( graphic );
        }
    }

    private FileChooserDialog CreateOpenDialog ()
    {
        var openDialog = new Gtk.FileChooserDialog (
            "Open file",
            this,
            FileChooserAction.Open,
            "Cancel", ResponseType.Cancel,
            "Open", ResponseType.Accept );

        return openDialog;
    }
}
