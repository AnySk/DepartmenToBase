using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Department2Base
{
    public class NoteForOperators : Panel
    {
        TextBox operatorNoteTextBox = new TextBox() { Dock = DockStyle.Fill, Multiline = true, Font = new System.Drawing.Font("Times New Roman", 12f)};
        TextBox NSNoteTextBox = new TextBox() { Dock = DockStyle.Fill, Multiline = true, Font = new System.Drawing.Font("Times New Roman", 12f) };
        Button refresh = new Button() { Text = "Обновить", Dock = DockStyle.Bottom };
        Button update = new Button() { Text = "Сохранить изменения", Dock = DockStyle.Bottom };

        public NoteForOperators()
        {
            try
            {
                Dock = DockStyle.Fill;
                Name = "NoteForOperators";

                Controls.Add(new TabControlTi(false)
                {
                    Name = "TabControlTi",
                    Dock = DockStyle.Fill,
                });

                Controls.Add(refresh);
                Controls.Add(update);

                (Controls.Find("TabControlTi", false).FirstOrDefault() as TabControlTi).TabPages.Add(new TabPage() { Text = "Личные записи", Name = "operatorNote" });
                (Controls.Find("TabControlTi", false).FirstOrDefault() as TabControlTi).TabPages.Add(new TabPage() { Text = "От начальника смены", Name = "NSNote" });

                (Controls.Find("TabControlTi", false).FirstOrDefault() as TabControlTi).TabPages["operatorNote"].Controls.Add(operatorNoteTextBox);
                (Controls.Find("TabControlTi", false).FirstOrDefault() as TabControlTi).TabPages["NSNote"].Controls.Add(NSNoteTextBox);


                update.Click += (s, e) =>
                {
                    dataBase.SimpleRequest("UPDATE [Login] SET [PrivateNote] = '" + operatorNoteTextBox.Text + "' WHERE [Login] = '" + Profile.userLogin + "'");
                    dataBase.SimpleRequest("UPDATE [Login] SET [NSNote] = '" + NSNoteTextBox.Text + "'' WHERE [Login] = '" + Profile.userLogin + "'");
                };

                refresh.Click += (s, e) =>
                {
                    operatorNoteTextBox.Text = dataBase.ToCount("SELECT [PrivateNote] FROM [Login] WHERE [Login] = '" + Profile.userLogin + "'").ToString();
                    NSNoteTextBox.Text = dataBase.ToCount("SELECT [NSNote] FROM [Login] WHERE [Login] = '" + Profile.userLogin + "'").ToString();

                };

                operatorNoteTextBox.Text = dataBase.ToCount("SELECT [PrivateNote] FROM [Login] WHERE [Login] = '" + Profile.userLogin + "'").ToString();
                NSNoteTextBox.Text = dataBase.ToCount("SELECT [NSNote] FROM [Login] WHERE [Login] = '" + Profile.userLogin + "'").ToString();
            }
            catch (Exception e)
            {
               // MessageBoxTi.Show("NoteForOperators " + e.Message);
            }


        }
    }
}
 