using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using MySqlConnector;

namespace TEST
{
    public class ClientPage : ContentPage
    {
        int Id;
        Label textLabel1, textLabel2;
        StackLayout stackLayout, stackLayout2, stackLayout3, stackLayout4;
        Button button, button2, button3, ok;
        ScrollView scroll; 
        Frame border;
        public ClientPage(int id)
        {
            Id = id;
            Title = "Личный кабинет";
            stackLayout = new StackLayout();
            stackLayout.BackgroundColor = Color.White;
            scroll = new ScrollView();
            stackLayout3 = new StackLayout();
            MySqlConnection conn = new MySqlConnection("server=127.0.0.1;port=3306;database=mydb;user id=root;password=1234;charset=utf8;Pooling=false;SslMode=None;");
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                    MySqlCommand cmd1 = new MySqlCommand("select * from zakaz where client_id = @client_id ORDER BY zakaz_id DESC", conn);
                    cmd1.Parameters.AddWithValue("@client_id", Id);
                    MySqlDataReader reader = cmd1.ExecuteReader();
                    while (reader.Read())
                    {
                        int usluga_id = Convert.ToInt32(reader["usluga_id"]);
                        MySqlConnection conn2 = new MySqlConnection("server=127.0.0.1;port=3306;database=mydb;user id=root;password=1234;charset=utf8;Pooling=false;SslMode=None;");
                        if (conn2.State == ConnectionState.Closed)
                        {
                            conn2.Open();
                            MySqlCommand cmd2 = new MySqlCommand("select name from Uslugi where usluga_id = @usluga_id", conn2);
                            cmd2.Parameters.AddWithValue("@usluga_id", usluga_id);
                            string name_usluga = cmd2.ExecuteScalar().ToString();

                            border = new Frame()
                            {
                                BorderColor = Color.Black,
                                CornerRadius = 10
                            };
                            stackLayout4 = new StackLayout()
                            {
                                Orientation = StackOrientation.Horizontal
                            };

                            textLabel1 = new Label
                            {
                                Text = name_usluga,
                                FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
                                HorizontalOptions = LayoutOptions.StartAndExpand,
                                WidthRequest = 200,
                                TextColor = Color.Black
                            };
                            textLabel2 = new Label
                            {
                                Text = Convert.ToString(reader["status"]),
                                FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
                                HorizontalOptions = LayoutOptions.CenterAndExpand,
                                TextColor = Color.Black
                            };
                            stackLayout4.Children.Add(textLabel1);
                            stackLayout4.Children.Add(textLabel2);
                            if (Convert.ToString(reader["status"]) == "создан")
                            {
                                ok = new Button
                                {
                                    Text = "отменить",
                                    FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Button)),
                                    HorizontalOptions = LayoutOptions.EndAndExpand,
                                    BorderWidth = 1,
                                    TextColor = Color.White,
                                    BackgroundColor = Color.FromHex("#a6075b"),
                                    ClassId = "But" + Convert.ToString(reader["zakaz_id"])
                                };
                                ok.Clicked += ok_Click;
                                stackLayout4.Children.Add(ok);
                            }

                            border.Content = stackLayout4;
                            stackLayout3.Children.Add(border);
                        }
                    }
                    reader.Close();
                }
           

            scroll.Content = stackLayout3;
            stackLayout.Children.Add(scroll);


            stackLayout2 = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.EndAndExpand
            };

            button = new Button
            {
                Text = "Создать заказ",
                FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Button)),
                BorderWidth = 1,
                HorizontalOptions = LayoutOptions.Fill,
                Margin = new Thickness(20),
                TextColor = Color.White,
                BackgroundColor = Color.FromHex("#a6075b")

            };
            button.Clicked += OnButtonClicked;

            button2 = new Button
            {
                Text = "Сообщения",
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
                Text = "Выход",
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
            stackLayout.Children.Add(stackLayout2);
            this.Content = stackLayout;
            }
            catch (MySqlException ex)
            {
                DisplayAlert("Сообщение об ошибке", ex.ToString(), "OK");
            }

        }
        private async void OnButtonClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new NewZakaz(Id));
        }

        private async void OnButton2Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new ClientChats(Id));
        }
        private async void OnButton3Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new MainPage()));
        }

        private async void ok_Click(object sender, System.EventArgs e)
        {
            MySqlConnection conn = new MySqlConnection("server=127.0.0.1;port=3306;database=mydb;user id=root;password=1234;charset=utf8;Pooling=false;SslMode=None;");
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
                Button button = (Button)sender;
                string text = button.ClassId;
                text = text.Replace("But", "");
                int get = Convert.ToInt32(text);

                MySqlCommand cmd = new MySqlCommand("UPDATE zakaz SET status = @status where zakaz_id = @zakaz_id;", conn);
                cmd.Parameters.AddWithValue("@status", "отменен");
                cmd.Parameters.AddWithValue("@zakaz_id", get);
                cmd.ExecuteNonQuery();
                await Navigation.PushModalAsync(new NavigationPage(new ClientPage(Id)));

            }
        }

    }
}