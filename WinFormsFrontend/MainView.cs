using ImTransformBackend.Services;
using WinFormsFrontend.Services;

namespace WinFormsFrontend
{
    public partial class MainView : Form
    {
        public MainView()
        {
            InitializeComponent();
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void MainView_Load(object sender, EventArgs e)
        {
            PopulateUI();
        }

        private void ApplyTransformation()
        {
            if (string.IsNullOrEmpty(cbPickFilter.Text))
            {
                return;
            }

            if (pictureBox.Image is not Bitmap bitmap)
            {
                return;
            }

            string wasmFile = "Native/" + cbPickFilter.Text;
            var (pixels, width, height) = DemoToolkit.ToRgba(bitmap);
            ImageTransformer.TransformImage(pixels, width, height, wasmFile);

            var result = DemoToolkit.ToBitmap(pixels, width, height);
            pictureBox.Image?.Dispose();
            pictureBox.Image = result;
        }

        public void UpdateUI()
        {
            string? path = cbPickRecentFile.SelectedItem is FileItem file
                ? file.FullPath
                : cbPickRecentFile.Text;

            if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
                return;

            pictureBox.Visible = true;

            using var stream = File.OpenRead(path);
            using var image = Image.FromStream(stream);

            var previousImage = pictureBox.Image;
            pictureBox.Image = new Bitmap(image);
            previousImage?.Dispose();

            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
        }


        public void LoadFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filename = openFileDialog.FileName;
                cbPickRecentFile.Text = filename;
                cbPickRecentFile.Items.Clear();
                cbPickRecentFile.Items.Add(filename);

                UpdateUI();
            }
        }

        public void ClearImage()
        {
            pictureBox.Visible = false;
            cbPickRecentFile.Text = "";
            cbPickRecentFile.SelectedItem = null;
            UpdateUI();
        }

        public void PopulateUI()
        {
            var fileRepository = new FileRepository();

            cbPickRecentFile.ComboBox.Items.Clear();
            cbPickFilter.ComboBox.Items.Clear();

            cbPickRecentFile.ComboBox.DisplayMember = nameof(FileItem.Name);
            cbPickFilter.ComboBox.DisplayMember = nameof(FileItem.Name);

            foreach (var file in fileRepository.GetLatestFiles())
                cbPickRecentFile.ComboBox.Items.Add(file);

            foreach (var file in fileRepository.GetWasmFiles())
                cbPickFilter.ComboBox.Items.Add(file);

            if (cbPickRecentFile.ComboBox.Items.Count > 0)
                cbPickRecentFile.ComboBox.SelectedIndex = 0;

            if (cbPickFilter.ComboBox.Items.Count > 0)
                cbPickFilter.ComboBox.SelectedIndex = 0;

            UpdateUI();
        }

        private void tsOpenFile_Click(object sender, EventArgs e)
        {
            LoadFile();
        }

        private void tbClear_Click(object sender, EventArgs e)
        {
            ClearImage();
        }

        private void cbPickRecentFile_DropDownClosed(object sender, EventArgs e)
        {
            UpdateUI();
        }

        private void tbApply_Click(object sender, EventArgs e)
        {
            ApplyTransformation();
        }
    }
}
