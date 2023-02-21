using Xamarin.Forms;
using System.Data;
using MySqlConnector;

namespace TEST
{
    public partial class MainPage : ContentPage
    {
        Label textLabel;
        Entry loginEntry, passwordEntry;
        StackLayout stackLayout, stackLayout2;
        Button button, button2, button3;
        public MainPage()
        {
            MySqlConnection conn = new MySqlConnection("server=127.0.0.1;port=3306;database=mydb;user id=root;password=1234;charset=utf8;Pooling=false;SslMode=None;");
            stackLayout = new StackLayout();
            stackLayout.BackgroundColor = Color.White;
            stackLayout2 = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.EndAndExpand,
                BackgroundColor = Color.White
            };


            loginEntry = new Entry
            {
                Placeholder = "Логин",
                Text = "",
                Margin = new Thickness(10),
                TextColor = Color.Black
            };

            passwordEntry = new Entry
            {
                Placeholder = "Пароль",
                Text = "",
                IsPassword = true,
                Margin = new Thickness(10),
                TextColor = Color.Black

            };

            textLabel = new Label
            {
                Text = "",
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                TextColor = Color.Black,
                HorizontalOptions = LayoutOptions.Center
            };

            button = new Button
            {
                Text = "Регистрация",
                FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Button)),
                BorderWidth = 1,
                HorizontalOptions = LayoutOptions.Fill,
                Margin = new Thickness(20),
                TextColor = Color.White,
                BackgroundColor = Color.FromHex("#a6075b"),

            };
            button.Clicked += OnButtonClicked;

            button2 = new Button
            {
                Text = "Вход",
                FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Button)),
                BorderWidth = 1,
                HorizontalOptions = LayoutOptions.Fill,
                Margin = new Thickness(20),
                TextColor = Color.White,
                BackgroundColor = Color.FromHex("#a6075b")
            };

            button2.Clicked += OnButton2Clicked;

            button3 = new Button
            {
                Text = "Забыли пароль?",
                FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Button)),
                BorderWidth = 1,
                HorizontalOptions = LayoutOptions.Fill,
                Margin = new Thickness(20),
                TextColor = Color.White,
                BackgroundColor = Color.FromHex("#a6075b")
            };

            button3.Clicked += OnButton3Clicked;

            stackLayout2.Children.Add(button2);
            stackLayout2.Children.Add(button);
            stackLayout2.Children.Add(button3);

            stackLayout.Children.Add(loginEntry);
            stackLayout.Children.Add(passwordEntry);
            stackLayout.Children.Add(stackLayout2);
            stackLayout.Children.Add(textLabel);
            this.Content = stackLayout;
        }
        private async void OnButtonClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new Registration());
        }
        private async void OnButton2Clicked(object sender, System.EventArgs e)
        {
            MySqlConnection conn = new MySqlConnection("server=127.0.0.1;port=3306;database=mydb;user id=root;password=1234;charset=utf8;Pooling=false;SslMode=None;");
            string name = "";
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                    if (loginEntry.Text != "" && passwordEntry.Text != "")
                    {
                        if (name == "")
                        {
                            MySqlCommand cmd1 = new MySqlCommand("select Client_Id from Client where login = @login", conn);
                            cmd1.Parameters.AddWithValue("@login", loginEntry.Text);
                            if (cmd1.ExecuteScalar() != null)
                            {
                                name = cmd1.ExecuteScalar().ToString();
                                await Navigation.PushModalAsync(new NavigationPage(new ClientPage(1)));
                            }
                        }
                        if (name == "")
                        {
                            MySqlCommand cmd2 = new MySqlCommand("select Worker_Id from Worker where login = @login", conn);
                            cmd2.Parameters.AddWithValue("@login", loginEntry.Text);
                            if (cmd2.ExecuteScalar() != null)
                            {
                                name = cmd2.ExecuteScalar().ToString();
                                await Navigation.PushModalAsync(new NavigationPage(new WorkerPage(1)));
                            }
                        }

                        if (name == "")
                        {
                            await DisplayAlert("Пользователя не существует", "Зарегистрируйтесь по кнопке", "OK");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Пустые поля", "Заполните все поля", "OK");
                    }
                }

            }
            catch (MySqlException ex)
            {
                await DisplayAlert("Сообщение об ошибке", ex.ToString(), "OK");
            }

        }
        private async void OnButton3Clicked(object sender, System.EventArgs e)
        {

            await Navigation.PushAsync(new RecoverAcc());

        }
    }
}
