using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace com.Momentum.Net.PostItNoteSystem
{
    public partial class Form1 : Form
    {
        NoteStorage NoteManager = new NoteStorage();
        string LocalWorkingFile = Directory.GetCurrentDirectory() + "/Notes.dat";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            if (File.Exists(LocalWorkingFile))
            {
                FileStream FileLoader = new FileStream(LocalWorkingFile, FileMode.Open);

                try
                {
                    BinaryFormatter SerializationManager = new BinaryFormatter();
                    NoteManager = (NoteStorage)SerializationManager.Deserialize(FileLoader);

                    foreach (KeyValuePair<string, string> Note in NoteManager.Notes)
                    {
                        listBox1.Items.Add(Note.Key);
                    }

                    FileLoader.Close();
                    FileLoader.Dispose();
                }
                catch (Exception ex)
                {
                    FileLoader.Close();
                    FileLoader.Dispose();
                    ex = null;
                }
            }
            else //If the save file does not exist create a save file.
            {
                FileStream FileLoader = new FileStream(LocalWorkingFile, FileMode.OpenOrCreate);
                FileLoader.Close();
                FileLoader.Dispose();
            }
        }

        //Delete the selected note
        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
            {

            }
            else
            {
                NoteManager.Notes.Remove(listBox1.SelectedItem.ToString());
                listBox1.Items.Remove(listBox1.SelectedItem);
                textBox1.Text = "";
                textBox2.Text = "";
            }
        }

        //Save the current note
        private void button1_Click(object sender, EventArgs e)
        {
            if (!listBox1.Items.Contains(textBox2.Text))
            {
                NoteManager.Notes.Add(textBox2.Text, textBox1.Text);
                listBox1.Items.Add(textBox2.Text);
                textBox2.Text = "";
                textBox1.Text = "";
            }
            else
            {
                NoteManager.Notes[listBox1.SelectedItem.ToString()] = textBox1.Text;
                textBox2.Text = "";
                textBox1.Text = "";
            }
        }

        //When the list box selection is changed update the note.
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem == null)
            {
                
            }
            else
            {
                textBox1.Text = NoteManager.Notes[listBox1.SelectedItem.ToString()].ToString();
                textBox2.Text = listBox1.SelectedItem.ToString();
            }
        }

        //Serialize the notes file.
        private void button3_Click(object sender, EventArgs e)
        {
            if (File.Exists(LocalWorkingFile))
            {
                try
                {
                    BinaryFormatter SerializationManager = new BinaryFormatter();
                    FileStream FileLoader = new FileStream(LocalWorkingFile, FileMode.Open);

                    SerializationManager.Serialize(FileLoader, NoteManager);

                    FileLoader.Close();
                    FileLoader.Dispose();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    ex = null;
                }
            }
            else //If the save file does not exist create a save file.
            {
                try
                {
                    BinaryFormatter SerializationManager = new BinaryFormatter();
                    FileStream FileLoader = new FileStream(LocalWorkingFile, FileMode.OpenOrCreate);

                    SerializationManager.Serialize(FileLoader, NoteManager);

                    FileLoader.Close();
                    FileLoader.Dispose();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    ex = null;
                }
            }
        }
    }

    [Serializable]
    public class NoteStorage
    {
        public Dictionary<string, string> Notes = new Dictionary<string, string>();
    }
}
