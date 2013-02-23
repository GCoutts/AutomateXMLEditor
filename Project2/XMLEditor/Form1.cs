using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


using EditorFunctions;

namespace XMLEditor
{
    /// <summary>
    /// The Form
    /// </summary>
    public partial class Form1 : Form
    {
        File file;
        /// <summary>
        ///  C'tor
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            file = new File();
        }


        
 
        /// <summary>
        /// Event handler for when the user clicks the open button
        /// Opens a File dialog to load an xml file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpen_Click(object sender, EventArgs e)
        {
            string text = "";
            if (file.Open(ref text))
            {
                if (text != "")
                {
                    rtbContent.Text = "";
                    rtbContent.Text = text;
                }
            }
            else
            {
                rtbContent.Text = "SOMETHING HAS GONE WRONG!";
            }
        }

        /// <summary>
        /// Event handler for when the user clicks the Test button
        /// First performs the Gui Tests, then the unit tests
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTest_Click(object sender, EventArgs e)
        {


        }
        /// <summary>
        /// Event handler when the user clicks the close button
        /// Closes the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        /// <summary>
        /// Event handler for when the user clicks the load schema button
        /// Opens a file dialog to load a schema
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSchema_Click(object sender, EventArgs e)
        {
            file.LoadSchema();
        }


        /// <summary>
        /// Event handler for when the user clicks the save button
        /// opens a save dialot to save the contentPOTATTOTO
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            string error = "";
            if (!file.Save(rtbContent.Text, ref error))
            {
                MessageBox.Show(error);
            }
        }







    }
}
