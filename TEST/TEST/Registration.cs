using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using MySql.Data.MySqlClient;

namespace TEST
{
    public class Registration : ContentPage
    {
        Label textLabel, textLabel2;
        Entry loginEntry, passwordEntry, passwordEntry2, nameEntry, mailEntry, numberEntry;
        StackLayout stackLayout;
        Button button;
        public Registration()
        {
            Title = "Регистрация";
            stackLayout = new StackLayout();
            stackLayout.BackgroundColor = Color.White;
            nameEntry = new Entry
            {
                Placeholder = "Имя",
                Margin = new Thickness(10),
                TextColor = Color.Black

            };
            numberEntry = new Entry
            {
                Placeholder = "Номер телефона",
                Margin = new Thickness(10),
                TextColor = Color.Black
            };

            mailEntry = new Entry
            {
                Placeholder = "Почта",
                Margin = new Thickness(10),
                TextColor = Color.Black
            };


            loginEntry = new Entry
            {
                Placeholder = "Логин",
                Margin = new Thickness(10),
                TextColor = Color.Black
            };

            passwordEntry = new Entry
            {
                Placeholder = "Пароль",
                IsPassword = true,
                Margin = new Thickness(10),
                TextColor = Color.Black

            };

            passwordEntry2 = new Entry
            {
                Placeholder = "Password",
                IsPassword = true,
                Margin = new Thickness(10),
                TextColor = Color.Black
            };

            textLabel = new Label
            {
                Text = "",
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                TextColor = Color.Black
            };
            passwordEntry.TextChanged += passwordEntry_TextChanged;
            passwordEntry2.TextChanged += passwordEntry_TextChanged;

            button = new Button
            {
                Text = "Регистрация",
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button)),
                BorderWidth = 1,
                HorizontalOptions = LayoutOptions.Center,
                Margin = new Thickness(20),
                TextColor = Color.White,
                BackgroundColor = Color.FromHex("#a6075b")

            };
            button.Clicked += OnButtonClicked;


            textLabel2 = new Label
            {
                Text = "",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center,
                TextColor = Color.Black
            };

            stackLayout.Children.Add(nameEntry);
            stackLayout.Children.Add(numberEntry);
            stackLayout.Children.Add(mailEntry);
            stackLayout.Children.Add(loginEntry);
            stackLayout.Children.Add(passwordEntry);
            stackLayout.Children.Add(passwordEntry2);
            stackLayout.Children.Add(textLabel);
            stackLayout.Children.Add(button);
            stackLayout.Children.Add(textLabel2);
            this.Content = stackLayout;

        }
        void passwordEntry_TextChanged(object sender, EventArgs e)
        {
            if (passwordEntry.Text != passwordEntry2.Text) { textLabel.Text = "Пароли не совпадают"; }
            else { textLabel.Text = ""; }
        }

        private async void OnButtonClicked(object sender, System.EventArgs e)
        {
            MySqlConnection conn = new MySqlConnection("server=127.0.0.1;port=3306;database=mydb;user id=root;password=1234;charset=utf8;Pooling=false;SslMode=None;");
            string name = "";
            try
            {

                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                    if (nameEntry.Text != "" && numberEntry.Text != "" && mailEntry.Text != "" && loginEntry.Text != "" && passwordEntry.Text != "")
                    {
                        MySqlCommand cmd1 = new MySqlCommand("select Client_Id from Client where login = @login or mail = @mail", conn);
                        cmd1.Parameters.AddWithValue("@login", loginEntry.Text);
                        cmd1.Parameters.AddWithValue("@mail", mailEntry.Text);
                        if (cmd1.ExecuteScalar() != null) { name = cmd1.ExecuteScalar().ToString(); }

                        MySqlCommand cmd2 = new MySqlCommand("select Worker_Id from Worker where login = @login or mail = @mail", conn);
                        cmd2.Parameters.AddWithValue("@login", loginEntry.Text);
                        cmd2.Parameters.AddWithValue("@mail", mailEntry.Text);
                        if (cmd2.ExecuteScalar() != null) { name = cmd2.ExecuteScalar().ToString(); }

                        MySqlCommand cmd3 = new MySqlCommand("select Adminis_Id from Adminis where login = @login", conn);
                        cmd3.Parameters.AddWithValue("@login", loginEntry.Text);
                        if (cmd3.ExecuteScalar() != null) { name = cmd3.ExecuteScalar().ToString(); }


                        if (name == "")
                        {
                            MySqlCommand cmd = new MySqlCommand("insert into client (name, number, mail, login, password) values(@name, @number, @mail, @login, @password);", conn);
                            cmd.Parameters.AddWithValue("@name", nameEntry.Text);
                            cmd.Parameters.AddWithValue("@number", numberEntry.Text);
                            cmd.Parameters.AddWithValue("@mail", mailEntry.Text);
                            cmd.Parameters.AddWithValue("@login", loginEntry.Text);
                            cmd.Parameters.AddWithValue("@password", passwordEntry.Text);
                            cmd.ExecuteNonQuery();
                            await DisplayAlert("Успешная регистрация", " ", "OK");
                            await Navigation.PopAsync();
                        }
                        else
                        {
                            await DisplayAlert("Пользователь с таким логином уже существует", " ", "OK");
                        }
                    }
                    else { await DisplayAlert("Пустые поля", "Заполните все поля", "OK"); }
                }


            }
            catch (MySqlException ex)
            {
                await DisplayAlert("Сообщение об ошибке", ex.ToString(), "OK");
            }

        }

    }
}