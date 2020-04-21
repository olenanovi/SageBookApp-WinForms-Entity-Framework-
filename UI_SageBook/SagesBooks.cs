using System;
using System.Windows.Forms;
using SageBook.Models;
using System.Data.Entity;
using SageBook.SageBookContexts;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace SageBook
{
    public partial class SagesBooks : Form
    {
        SageBookContext context;

        public SagesBooks()
        {
            InitializeComponent();
            
            tabControl.SelectTab("tabSage");
            context = new SageBookContext();

            context.Sages.Load();
            dataGridViewSages.DataSource = context.Sages.Local.ToBindingList();
            //dataGridViewSages.Columns[0].Visible = false;
            dataGridViewSages.Columns[5].Visible = false;

            context.Books.Load();
            dataGridViewBooks.DataSource = context.Books.Local.ToBindingList();
            //dataGridViewBooks.Columns[0].Visible = false;
            dataGridViewBooks.Columns[3].Visible = false;

            dataGridViewBooks.DataBindingComplete += DataGridViewBooks_DataBindingComplete;

            dataGridViewBooks.Columns.Add("SagesString", "Sages");
        }

        private void DataGridViewBooks_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            DataGridViewBooksSagesRefresh();
        }

        private void DataGridViewBooksSagesRefresh()
        {
            if (dataGridViewBooks.Columns.Contains("Sages"))
            {
                for (int i = 0; i < dataGridViewBooks.Rows.Count; i++)
                {
                    var sages = string.Join(", ", (dataGridViewBooks.Rows[i].Cells["Sages"].Value as ICollection<Sage>).Select(x => x.Name));
                    dataGridViewBooks["SagesString", i].Value = sages;
                }
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            DeleteTab formDelete = new DeleteTab();

            DialogResult result = formDelete.ShowDialog(this);

            if (result == DialogResult.Cancel)
                return;
            if (result == DialogResult.OK)
            {
                if (tabControl.SelectedIndex == 0)
                {
                    if (dataGridViewSages.SelectedRows.Count > 0)
                    {
                        int index = dataGridViewSages.SelectedRows[0].Index;
                        bool converted = Int32.TryParse(dataGridViewSages["IdSage", index].Value.ToString(), out int id);
                        if (converted == false)
                            return;

                        Sage sageInTrash = context.Sages.Find(id);
                        context.Sages.Remove(sageInTrash);
                        context.SaveChanges();

                        MessageBox.Show("Sage Deleted");
                        dataGridViewSages.Refresh();
                    }
                }
                else
                {
                    int index = dataGridViewBooks.SelectedRows[0].Index;
                    bool converted = Int32.TryParse(dataGridViewBooks["IdBook", index].Value.ToString(), out int id);
                    if (converted == false)
                        return;

                    Book bookInTrash = context.Books.Find(id);
                    context.Books.Remove(bookInTrash);
                    context.SaveChanges();

                    MessageBox.Show("Book Deleted");
                    dataGridViewBooks.Refresh();

                }
            }


        }

        private byte[] ConvertFileToByte(string sPath)
        {
            byte[] data = null;
            FileInfo fInfo = new FileInfo(sPath);
            long numBytes = fInfo.Length;
            FileStream fStream = new FileStream(sPath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fStream);
            data = br.ReadBytes((int) numBytes);
            return data;
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedIndex == 0)
            {
                AddSage formAddSage = new AddSage();

                DialogResult result = formAddSage.ShowDialog(this);

                if (result == DialogResult.Cancel)
                    return;
                if (result == DialogResult.OK)
                {
                    Sage newSage = new Sage();
                    newSage.Name = formAddSage.textBoxName.Text;
                    newSage.Age = (int)formAddSage.numericUpDownAge.Value;
                    
                    if (formAddSage.textBoxImage.Text != String.Empty)
                        newSage.Image = ConvertFileToByte(formAddSage.textBoxImage.Text);

                    newSage.City = formAddSage.textBoxCity.Text;
                    context.Sages.Add(newSage);
                    context.SaveChanges();
                    MessageBox.Show("New Sage Added");
                }

            }
            else
            {
                AddBook formAddBook = new AddBook(context);

                formAddBook.listBoxSages.SelectedItems.Clear();

                DialogResult result = formAddBook.ShowDialog(this);

                if (result == DialogResult.Cancel)
                    return;
                if (result == DialogResult.OK)
                {
                    Book newBook = new Book();
                    newBook.Name = formAddBook.textBoxName.Text;
                    
                    foreach (var item in formAddBook.listBoxSages.SelectedItems)
                    {
                        if (item is Sage sage)
                        {
                            newBook.Sages.Add(sage);
                        }
                    }

                    newBook.Description = formAddBook.textBoxDescription.Text;
                    context.Books.Add(newBook);
                    context.SaveChanges();
                    DataGridViewBooksSagesRefresh();
                    MessageBox.Show("New Book Added");
                }

            } 
            
        }


        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            SageBookContext context = new SageBookContext();
            if (tabControl.SelectedIndex == 0)
            {
                dataGridViewSages.Refresh();
                
            }
            else
            {
                dataGridViewBooks.Refresh();
            }
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedIndex == 0)
            {
                if (dataGridViewSages.SelectedRows.Count > 0)
                {
                    int index = dataGridViewSages.SelectedRows[0].Index;
                    bool converted = Int32.TryParse(dataGridViewSages["IdSage", index].Value.ToString(), out int id);
                    if (converted == false)
                        return;

                    Sage editingSage = context.Sages.Find(id);

                    AddSage formEditSage = new AddSage();

                    formEditSage.textBoxName.Text = editingSage.Name;
                    formEditSage.numericUpDownAge.Value = editingSage.Age;

                    formEditSage.textBoxCity.Text = editingSage.City;

                    DialogResult result = formEditSage.ShowDialog(this);

                    if (result == DialogResult.Cancel) return;

                    if (result == DialogResult.OK)
                    {
                        editingSage.Age = (int)formEditSage.numericUpDownAge.Value;
                        editingSage.Name = formEditSage.textBoxName.Text;

                        if (formEditSage.textBoxImage.Text != String.Empty)
                            editingSage.Image = ConvertFileToByte(formEditSage.textBoxImage.Text);

                        editingSage.City = formEditSage.textBoxCity.Text;

                        context.SaveChanges();
                        dataGridViewSages.Refresh();
                        MessageBox.Show("Information about sage refreshed");
                    }
                }
            }
            else
            {
                if (dataGridViewBooks.SelectedRows.Count > 0)
                {
                    int index = dataGridViewBooks.SelectedRows[0].Index;
                    bool converted = Int32.TryParse(dataGridViewBooks["IdBook", index].Value.ToString(), out int id);
                    if (converted == false) return;

                    Book editingBook = context.Books.Find(id);

                    AddBook formEditBook = new AddBook(context);

                    formEditBook.textBoxName.Text = editingBook.Name;

                    formEditBook.listBoxSages.SelectedItems.Clear();

                    foreach (Sage t in editingBook.Sages)
                        formEditBook.listBoxSages.SelectedItem = t;


                    formEditBook.textBoxDescription.Text = editingBook.Description;

                    DialogResult result = formEditBook.ShowDialog(this);

                    if (result == DialogResult.Cancel) return;

                    if (result == DialogResult.OK)
                    {
                        editingBook.Name = formEditBook.textBoxName.Text;

                        editingBook.Sages.Clear();

                        foreach (var item in formEditBook.listBoxSages.SelectedItems)
                        {
                            if (item is Sage sage)
                            {
                                editingBook.Sages.Add(sage);
                            }
                        }

                        editingBook.Description = formEditBook.textBoxDescription.Text;

                        context.Entry(editingBook).State = EntityState.Modified;
                        context.SaveChanges();
                        dataGridViewBooks.Refresh();
                        DataGridViewBooksSagesRefresh();
                        MessageBox.Show("Information about book refreshed");
                    }
                }
            }
        }
    }
}
