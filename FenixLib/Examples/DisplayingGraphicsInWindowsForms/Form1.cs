using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FenixLib.Core;
using FenixLib.IO;
using FenixLib.BitmapConvert;

namespace DisplayingGraphicsInWindowsForms
{
    public partial class Form1 : Form
    {
        private ISpriteAssortment spriteAssortment;
        private IBitmapFont bitmapFont;

        public Form1 ()
        {
            InitializeComponent ();
        }

        private void button1_Click ( object sender, EventArgs e )
        {
            openFileDialog1.Filter = "Map files (*.map)|*.map";
            var result = openFileDialog1.ShowDialog ();
            if ( result == DialogResult.OK )
            {
                var oldImage = pictureBox1.Image;
                pictureBox1.Image = NativeFile.LoadMap ( openFileDialog1.FileName ).ToBitmap ();

                // Bitmaps implement the IDisposable interface, so we need to dispose them
                // manually
                if ( oldImage != null )
                {
                    oldImage.Dispose ();
                }
            }
        }

        private void button2_Click ( object sender, EventArgs e )
        {
            openFileDialog1.Filter = "Fpg files (*.fpg)|*.fpg";
            var result = openFileDialog1.ShowDialog ();
            if ( result == DialogResult.OK )
            {
                spriteAssortment = NativeFile.LoadFpg ( openFileDialog1.FileName );

                listBox1.Items.Clear ();
                // Fill the list box with the sprite names
                foreach ( var sprite in spriteAssortment )
                {
                    listBox1.Items.Add ( $"{sprite.Id}: {sprite.Description}" );
                }
            }
        }

        private void listBox1_SelectedIndexChanged ( object sender, EventArgs e )
        {
            var index = listBox1.SelectedIndex;

            if ( index  < 0)
            {
                return;
            }

            var oldImage = pictureBox1.Image;
            pictureBox1.Image = spriteAssortment.Sprites.ElementAt(index).ToBitmap ();

            // Bitmaps implement the IDisposable interface
            if ( oldImage != null )
            {
                oldImage.Dispose ();
            }
        }

        private void button3_Click ( object sender, EventArgs e )
        {
            openFileDialog1.Filter = "Fnt files (*.fnt)|*.fnt";
            var result = openFileDialog1.ShowDialog ();
            if ( result == DialogResult.OK )
            {
                bitmapFont = NativeFile.LoadFnt ( openFileDialog1.FileName );

                listBox2.Items.Clear ();
                // Fill the list box with the sprite names
                foreach ( var glyph in bitmapFont )
                {
                    listBox2.Items.Add ( $"{glyph.Character}" );
                }
            }
        }

        private void listBox2_SelectedIndexChanged ( object sender, EventArgs e )
        {
            var index = listBox2.SelectedIndex;
            
            if ( index < 0 )
            {
                return;
            }

            char character = listBox2.Items[index].ToString ().ToCharArray ()[0];

            var oldImage = pictureBox1.Image;
            pictureBox1.Image = bitmapFont[character].ToBitmap ();

            // Bitmaps implement the IDisposable interface
            if ( oldImage != null )
            {
                oldImage.Dispose ();
            }
        }
    }
}
