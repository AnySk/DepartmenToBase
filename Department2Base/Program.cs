using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Department2Base
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if ((int)dataBase.ToCount("SELECT count(*) FROM [dbo].[ProgVer]") != 0)
            {
                if (Convert.ToInt64(dataBase.ToCount("SELECT TOP 1 [Version] FROM [dbo].[ProgVer] ORDER BY [dbo].[ProgVer].[Version] DESC").ToString().Replace(".", string.Empty)) > Convert.ToInt64(Application.ProductVersion.Replace(".", string.Empty)))
                {
                    if (MessageBoxTi.Show("Ваша версия БД - " + Application.ProductVersion + ", акуальная версия - " + (string)dataBase.ToCount("SELECT TOP 1 [Version] FROM [dbo].[ProgVer] ORDER BY [dbo].[ProgVer].[Version] DESC") + ". Обновить?", "Обновление") == MessageResult.Yes)
                    {
                        int num = 0;

                        while (!Path.HasExtension(Application.ExecutablePath + num))
                            num++;
                        
                        DataTable ProgVer = dataBase.SimpleData("ProgVer");
                        byte[] my = (byte[])ProgVer.Rows.Cast<DataRow>().OrderByDescending(x => x["Version"]).FirstOrDefault()["Data"];
                        FileStream fs = new FileStream(Application.ExecutablePath + num, FileMode.Create);
                        fs.Write(my, 0, my.Length);
                        fs.Close();
                        Application.Exit();

                        ProcessStartInfo info = new ProcessStartInfo();
                        info.Arguments = "/C choice /C Y /N /D Y /T 3 & Del \"" + Application.ExecutablePath + "\" && ren \"" + Application.ExecutablePath + num + "\" \"" + AppDomain.CurrentDomain.FriendlyName + "\" && \"" + AppDomain.CurrentDomain.FriendlyName + "\"";
                        info.WindowStyle = ProcessWindowStyle.Hidden;
                        info.CreateNoWindow = true;
                        info.FileName = "cmd.exe";
                        Process.Start(info);

                        return;
                    }
                }
            }
            

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginForm());

        }
    }
}
