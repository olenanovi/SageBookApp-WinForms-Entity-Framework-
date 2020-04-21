using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SageBook.Models;
using SageBook.SageBookContexts;

namespace SageBook
{
    public partial class AddBook : Form
    {
        private readonly SageBookContext context;

        public AddBook(SageBookContext context)
        {
            this.context = context;

            InitializeComponent();

            //listBoxSages.Items.AddRange(context.Sages.Select(x => x.Name).ToArray());
            
            listBoxSages.DataSource = context.Sages.ToList();
            listBoxSages.ValueMember = "IdSage";
            listBoxSages.DisplayMember = "Name";
        }
    }
}