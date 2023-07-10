namespace MCManager
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            try
            {
                Form1 form1;
                Application.Run(form1 = new Form1());
                form1.p.Close();
            }
            catch (Exception ex) { }
        }
    }
}