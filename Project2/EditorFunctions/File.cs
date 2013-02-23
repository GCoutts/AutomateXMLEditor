using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Schema;

namespace EditorFunctions
{
    /// <summary>
    /// A class that will perform the functionality the XMLEditor needs.
    /// </summary>
    public class File
    {
        public string schemaLoc;

        /// <summary>
        /// C'tor
        /// </summary>
        public File()
        {
            schemaLoc = "";
        }


        /// <summary>
        /// Return the content of an xml file if one is chosen
        /// </summary>
        /// <param name="ret">A string that will contain the contents of the file being loaded</param>
        /// <returns>Will return true if the file was opened, and false otherwise</returns>
        public bool Open(ref string ret)
        {
            Stream myStream = null;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            
            openFileDialog1.InitialDirectory = "./bin";
            openFileDialog1.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                   
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        XmlTextReader reader = new XmlTextReader(openFileDialog1.FileName);

                        while (reader.Read())
                        {
                            switch (reader.NodeType)
                            {
                                case XmlNodeType.Element: // next node is the beining of an element
                                    {
                                        
                                        ret += "<" + reader.Name;
                                        bool isEmpty = reader.IsEmptyElement;

                                        while (reader.MoveToNextAttribute()) // Read the attributes.
                                            ret +=" " + reader.Name + "='" + reader.Value + "'";

                                        if (isEmpty)// if the element is empty
                                        {
                                            ret += "/>\n";
                                        }
                                        else // if the element contains something
                                        {
                                            ret += ">\n";
                                        }
                                        break;
                                    }

                                case XmlNodeType.Text:  // the content of an element
                                    {
                                        ret += "\t" + reader.Value + "\n";
                                        break;
                                    }
                                case XmlNodeType.EndElement:// closing tag.
                                    {
                                        ret += "</" + reader.Name + ">\n";
                                        break;
                                    }
                            }
                        }


                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                    return false;
                }
                
            }



            return true;
        }
        /// <summary>
        /// Accepts a string to be converted into a byte array
        /// </summary>
        /// <param name="str">The string that will be converted to a byte array</param>
        /// <returns>The a byte array version of the string passed in</returns>
        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }
        /// <summary>
        /// Accepts a byte array and converst it to a string
        /// </summary>
        /// <param name="bytes">Byte array to be converted into a string.</param>
        /// <returns>The byte aray converted to a string.</returns>
        static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }
        /// <summary>
        /// Accepts the text box content to be validated and saved to an xml file
        /// </summary>
        /// <param name="content">The text the user wishes to save to file.</param>
        /// <param name="error">A string that will contain information about an error if one occurs.</param>
        /// <returns></returns>
        public bool Save(string content, ref string error)
        {
            byte[] write = GetBytes(content);
            Stream myStream = null;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            //XmlTextWriter writer = new XmlTextWriter();

            if (schemaLoc != "" && !validate(content))// validates the richtextbox content if a schema has been loaded
            {
                error = "XML not valid based on schema!";
                return false;
            }




            // sets up the settins for the savefiledialog
            saveFileDialog.InitialDirectory = "../";
            saveFileDialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 2;
            saveFileDialog.RestoreDirectory = true;
            //saveFileDialog.Title = "Save";

            // Did the user click ok?
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                // can the file be opened>
                if((myStream = saveFileDialog.OpenFile()) != null)
                {
                    // write the content of the text box to the file
                    myStream.Write(write,0,write.Length);
                    myStream.Close();
                    
                }


                
            }

            return true;
        }



        /// <summary>
        /// Handler for the validation call NEEds changing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ValidationCallBack(object sender, ValidationEventArgs e)
        {
            throw new Exception();
        }
        /// <summary>
        /// Accepts Xml as a string to be validated against the schema the user selected earlier
        /// </summary>
        /// <param name="sxml">The XML to be validated against the schema</param>
        /// <returns>Returns true if the XML is valid, and returns false otherwise.</returns>
        public bool validate(string sxml)
        {
            try
            {
                XmlDocument xmld = new XmlDocument();
                xmld.LoadXml(sxml);
                xmld.Schemas.Add(null, @schemaLoc);
                xmld.Validate(ValidationCallBack);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// after the user selects a xsd file in the file dialog, the path to the chosen file
        /// is stored to be used to validate the xml at a later time.
        /// </summary>
        /// <returns>Returns true if the schema is loaded.</returns>
        public bool LoadSchema()
        {
            // creates the file dialog
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // sets the settings of the dialog
            openFileDialog.InitialDirectory = "../";
            openFileDialog.Filter = "XSD files (*.xsd)|*.xsd|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;


            // Has the user pressed OK
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {

                    if (openFileDialog.CheckFileExists)
                    {
                        // stores the path of the selected schema
                        schemaLoc = openFileDialog.FileName;
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                    return false;
                }
            }
            return false;
        }






    }
}
