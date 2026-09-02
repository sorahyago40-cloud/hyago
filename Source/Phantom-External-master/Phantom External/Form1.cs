using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using KeyAuth;

namespace Phantom_External
{
    public partial class Form1 : Form
    {
        public static api KeyAuthApp = new api(
            name: "",
            ownerid: "",
            secret: "",
            version: ""
        );

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                KeyAuthApp.init();

                // AutoLogin(); // 🔥 tenta logar automaticamente
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro init: " + ex.Message);
            }
        }

        // 🔑 LOGIN
        private void lczxy7AnimatedButton11_Click(object sender, EventArgs e)
        {
            string username = guna2TextBox2.Text;
            string password = guna2TextBox1.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Preencha usuário e senha!");
                return;
            }

            try
            {
                lczxy7AnimatedButton11.Enabled = false;

                KeyAuthApp.login(username, password);

                if (KeyAuthApp.response.success)
                {
                    MessageBox.Show("Login bem-sucedido!", "Sucesso");

                    SaveLogin(username, password);

                    OpenForm2(username);
                }
                else
                {
                    MessageBox.Show("Login falhou: " + KeyAuthApp.response.message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro login: " + ex.Message);
            }
            finally
            {
                lczxy7AnimatedButton11.Enabled = true;
            }
        }

        // 🚀 ABRIR FORM2
        private void OpenForm2(string username)
        {
            Form2 f2 = new Form2();
            f2.Usuario = username;

            f2.FormClosed += (s, args) => this.Close();

            f2.Show();
            this.Hide();
        }

        // 💾 SALVAR LOGIN (BASE64)
        private void SaveLogin(string user, string pass)
        {
            try
            {
                string raw = user + "\n" + pass;
                string encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(raw));

                File.WriteAllText("login.txt", encoded);
            }
            catch { }
        }

        // 🔓 AUTO LOGIN
        private void AutoLogin()
        {
            try
            {
                if (!File.Exists("login.txt"))
                    return;

                string encoded = File.ReadAllText("login.txt");
                string decoded = Encoding.UTF8.GetString(Convert.FromBase64String(encoded));

                string[] data = decoded.Split('\n');

                if (data.Length < 2)
                    return;

                string user = data[0];
                string pass = data[1];

                KeyAuthApp.login(user, pass);

                if (KeyAuthApp.response.success)
                {
                    OpenForm2(user);
                }
            }
            catch
            {
                // silencioso pra não quebrar UX
            }
        }

        // (EVENTOS VAZIOS - OK MANTER OU REMOVER)
        private void guna2TextBox2_TextChanged(object sender, EventArgs e) { }
        private void guna2TextBox1_TextChanged(object sender, EventArgs e) { }
    }
}