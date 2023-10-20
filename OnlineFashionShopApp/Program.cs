namespace OnlineFashionShopApp
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
            //Application.Run(new LoginForm()); 
            //Application.Run(new RegisterForm()); //andrew
            //Application.Run(new HomeFormCustomer());
            //Application.Run(new HomeFormAdmin());
            Application.Run(new SettingsFormCustomer()); //andrew
            //Application.Run(new SettingsFormAdmin()); //andrew
            //Application.Run(new CartForm());
            //Application.Run(new AccessLogForm()); //andrew
            //Application.Run(new AddressForm()); //andrew
            //Application.Run(new AddressListForm()); //andrew
            //Application.Run(new PaymentForm()); //andrew
            //Application.Run(new PaymentSuccessForm());
            //Application.Run(new ProductCustomerForm());
            //Application.Run(new ProductAdminForm());
            //Application.Run(new AddProductForm());
            //Application.Run(new TrackingForm());
            //Application.Run(new OrderFormCustomer());
            //Application.Run(new OrderFormAdmin());
        }
    }
}