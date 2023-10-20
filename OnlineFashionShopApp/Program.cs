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
            Application.Run(new LoginForm());
            Application.Run(new RegisterForm());
            Application.Run(new HomeFormCustomer());
            Application.Run(new HomeFormAdmin());
            Application.Run(new SettingsFormCustomer());
            Application.Run(new SettingsFormAdmin());
            Application.Run(new CartForm());
            Application.Run(new AccessLogForm());
            Application.Run(new AddressForm());
            Application.Run(new AddressListForm());
            Application.Run(new PaymentForm());
            Application.Run(new PaymentSuccessForm());
            Application.Run(new ProductCustomerForm());
            Application.Run(new ProductAdminForm());
            Application.Run(new AddProductForm());
            Application.Run(new TrackingForm());
            Application.Run(new OrderFormCustomer());
            Application.Run(new OrderFormAdmin());
        }
    }
}