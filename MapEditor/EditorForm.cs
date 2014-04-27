using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace MapEditor
{
    using GdiColor = System.Drawing.Color;
    using XnaColor = Microsoft.Xna.Framework.Color;

    public partial class EditorForm : Form
    {
        public EditorForm()
        {
            InitializeComponent();
        }

        private void loadLevelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog theDialog = new OpenFileDialog();
            theDialog.Title = "Open Level File";
            theDialog.Filter = "XML files|*.xml";
            theDialog.InitialDirectory = @"../../../HackyRPG/Content/";
            if (theDialog.ShowDialog() == DialogResult.OK)
            {
                // Load level here
                editor1.LoadLevel(@"Levels\" + theDialog.SafeFileName.ToString());
            }
        }

        private void loadTilesetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog theDialog = new OpenFileDialog();
            theDialog.Title = "Open Tileset File";
            theDialog.Filter = "XNB files|*.xnb";
            theDialog.InitialDirectory = @"../../../HackyRPG/Content/";
            if (theDialog.ShowDialog() == DialogResult.OK)
            {
                // Strip the extension off the filename and pass it to the Sprite Panel
                string filename = Path.GetFileNameWithoutExtension(theDialog.SafeFileName.ToString());
                spriteSheetDisplay1.LoadSpriteSheet(filename);
            }
        }
    }
}
