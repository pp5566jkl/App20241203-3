namespace App20241203_3
{
    public partial class Form1 : Form
    {
        private Point leftPoint = Point.Empty;
        private Point rightPoint = Point.Empty;
        private bool isLeftPointSelected = false;
        private bool isRightPointSelected = false;
        private const double FocalLength = 12.07; // mm
        private const double PixelSize = 0.0033450704225352; // mm (�C�����j�p)

        public Form1()
        {
            InitializeComponent();

            // �]�m PictureBox �� Click �ƥ�
            this.pictureBox1.MouseClick += pictureBox1_MouseClick;
            this.pictureBox2.MouseClick += pictureBox2_MouseClick;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Filter = "�Ϲ����(JPeg, Gif, Bmp, etc.)|.jpg;*jpeg;*.gif;*.bmp;*.tif;*.tiff;*.png|�Ҧ����(*.*)|*.*";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    Bitmap MyBitmap = new Bitmap(openFileDialog1.FileName);
                    this.pictureBox1.Image = MyBitmap;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "�T�����");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Filter = "�Ϲ����(JPeg, Gif, Bmp, etc.)|.jpg;*jpeg;*.gif;*.bmp;*.tif;*.tiff;*.png|�Ҧ����(*.*)|*.*";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    Bitmap MyBitmap = new Bitmap(openFileDialog1.FileName);
                    this.pictureBox2.Image = MyBitmap;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "�T�����");
            }
        }
        private void pictureBox2_MouseClick(object? sender, MouseEventArgs e)
        {
            Point translatedPoint = TranslateMouseClickToImagePoint(pictureBox2, e.Location);
            if (translatedPoint == Point.Empty)
            {
                MessageBox.Show("���I���Ϥ����������I�ϰ�I");
                return;
            }

            rightPoint = translatedPoint;
            isRightPointSelected = true;
            label2.Text = $" ({rightPoint.X}, {rightPoint.Y})";
        }

        private void pictureBox1_MouseClick(object? sender, MouseEventArgs e)
        {
            Point translatedPoint = TranslateMouseClickToImagePoint(pictureBox1, e.Location);
            if (translatedPoint == Point.Empty)
            {
                MessageBox.Show("���I���Ϥ����������I�ϰ�I");
                return;
            }

            leftPoint = translatedPoint;
            isLeftPointSelected = true;
            label1.Text = $" ({leftPoint.X}, {leftPoint.Y})";
        }



        private Point TranslateMouseClickToImagePoint(PictureBox pictureBox, Point mouseClick)
        {
            if (pictureBox.Image == null) return Point.Empty;

            int imgWidth = pictureBox.Image.Width;
            int imgHeight = pictureBox.Image.Height;
            
            int pbWidth = pictureBox.Width;
            int pbHeight = pictureBox.Height;

            float ratioWidth = (float)pbWidth / imgWidth;
            float ratioHeight = (float)pbHeight / imgHeight;
            float scale = (pictureBox.SizeMode == PictureBoxSizeMode.Zoom) ? Math.Min(ratioWidth, ratioHeight) : Math.Max(ratioWidth, ratioHeight);

            int displayWidth = (int)(imgWidth * scale);
            int displayHeight = (int)(imgHeight * scale);

            int offsetX = (pbWidth - displayWidth) / 2;
            int offsetY = (pbHeight - displayHeight) / 2;

            if (mouseClick.X < offsetX || mouseClick.X > offsetX + displayWidth ||
                mouseClick.Y < offsetY || mouseClick.Y > offsetY + displayHeight)
            {
                return Point.Empty;
            }

            int imgX = (int)((mouseClick.X - offsetX) / scale);
            int imgY = (int)((mouseClick.Y - offsetY) / scale);

            return new Point(imgX, imgY);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (isLeftPointSelected && isRightPointSelected)
                {
                    if (double.TryParse(textBox1.Text, out double baselineCm))
                    {
                        
                        double baselineMm = baselineCm * 10;
                        double disparityPixels = Math.Abs(leftPoint.X - rightPoint.X);
                        double disparityMm = disparityPixels * PixelSize;

                        if (disparityMm == 0)
                        {
                            MessageBox.Show("���t���s�A�L�k�p��`�סC���ˬd���I����O�_���T�C", "���~", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                                               
                        double depthMm = (FocalLength * baselineMm) / disparityMm;
                        double depthCm = depthMm / 10;
                        
                        label3.Text = $"{depthCm:F2}";
                    }
                    else
                    {
                        MessageBox.Show("�п�J���Ī��۾����ʶZ�� (Baseline)�C", "���~", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("�Цb��i�Ϥ���������I������m�C", "���~", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "���~");
            }
        }
    }
}