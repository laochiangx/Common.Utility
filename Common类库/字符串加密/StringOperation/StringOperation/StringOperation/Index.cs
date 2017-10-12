namespace StringOperation
{
    using System;
    using System.Web.UI;
    using System.Web.UI.WebControls;

    public class Index : Page
    {
        protected Button Button1;
        protected Button Button10;
        protected Button Button11;
        protected Button Button12;
        protected Button Button13;
        protected Button Button14;
        protected Button Button15;
        protected Button Button16;
        protected Button Button17;
        protected Button Button18;
        protected Button Button2;
        protected Button Button3;
        protected Button Button4;
        protected Button Button5;
        protected Button Button6;
        protected Button Button7;
        protected Button Button8;
        protected TextBox TextBox1;

        private void Button1_Click(object sender, EventArgs e)
        {
            this.TextBox1.Text = StringOperation.StringOperation.Encode(this.TextBox1.Text);
        }

        private void Button10_Click(object sender, EventArgs e)
        {
            this.TextBox1.Text = StringOperation.StringOperation.DeTransform3(this.TextBox1.Text);
        }

        private void Button11_Click(object sender, EventArgs e)
        {
            this.TextBox1.Text = StringOperation.StringOperation.Transform3(this.TextBox1.Text);
        }

        private void Button12_Click(object sender, EventArgs e)
        {
            this.TextBox1.Text = StringOperation.StringOperation.Reverse(this.TextBox1.Text);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.TextBox1.Text = StringOperation.StringOperation.Decode(this.TextBox1.Text);
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            this.TextBox1.Text = StringOperation.StringOperation.Encrypt(this.TextBox1.Text);
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            this.TextBox1.Text = StringOperation.StringOperation.Encrypt(this.TextBox1.Text, 0);
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            this.TextBox1.Text = StringOperation.StringOperation.Encrypt(this.TextBox1.Text, 1);
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            this.TextBox1.Text = StringOperation.StringOperation.Decrypt(this.TextBox1.Text);
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            this.TextBox1.Text = StringOperation.StringOperation.DeTransform1(this.TextBox1.Text);
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            this.TextBox1.Text = StringOperation.StringOperation.Transform1(this.TextBox1.Text);
        }

        private void InitializeComponent()
        {
            this.Button1.Click += new EventHandler(this.Button1_Click);
            this.Button2.Click += new EventHandler(this.Button2_Click);
            this.Button3.Click += new EventHandler(this.Button3_Click);
            this.Button4.Click += new EventHandler(this.Button4_Click);
            this.Button5.Click += new EventHandler(this.Button5_Click);
            this.Button6.Click += new EventHandler(this.Button6_Click);
            this.Button7.Click += new EventHandler(this.Button7_Click);
            this.Button8.Click += new EventHandler(this.Button8_Click);
            this.Button10.Click += new EventHandler(this.Button10_Click);
            this.Button11.Click += new EventHandler(this.Button11_Click);
            this.Button12.Click += new EventHandler(this.Button12_Click);
            base.Load += new EventHandler(this.Page_Load);
        }

        protected override void OnInit(EventArgs e)
        {
            this.InitializeComponent();
            base.OnInit(e);
        }

        private void Page_Load(object sender, EventArgs e)
        {
        }
    }
}

