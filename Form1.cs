using AxWMPLib;
using Testproject;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.IO;

namespace Testproject
{
    public partial class Form1 : Form
    {
        private DoubleLinkedList DLL = new DoubleLinkedList();   //Khởi tạo danh sách liên kết
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            btnRewind15s.Click += btnRewind15s_Click;
            btnForward15s.Click += btnForward15s_Click;
        }
        OpenFileDialog openFileDialog;    //Khởi tạo biến để mở hộp thoại file cho phép chọn file
        string[] filePaths, fileNames;    //Mảng chứa đường dẫn và tên file của bài hát
        private void btn_Add_Click(object sender, EventArgs e)   
        {
            openFileDialog = new OpenFileDialog();   //Khởi tạo hộp thoại mở file
            openFileDialog.Filter = "Audio and Video Files (*.mp3; *.mp4)|*.mp3; *.mp4";   //Chỉ cho phép chọn file âm thanh và video
            openFileDialog.Multiselect = true;   //Cho phép chọn nhiều file
            openFileDialog.CheckFileExists = true;   //Kiểm tra file tồn tại
            openFileDialog.CheckPathExists = true;   //Kiểm tra đường dẫn tồn tại
            openFileDialog.Title = "Select Audio or Video Files";   //Tiêu đề của hộp thoại
            if (openFileDialog.ShowDialog() == DialogResult.OK)   //Nếu chọn file thành công
            {
                string[] filePaths = openFileDialog.FileNames;   //Lấy đường dẫn của các file
                string[] fileNames = openFileDialog.SafeFileNames;   //Lấy tên của các file
                for (int i = 0; i < filePaths.Length; i++)   //Duyệt qua các file đã chọn
                {
                    bool isExist = false;   //Biến kiểm tra file đã tồn tại trong danh sách chưa
                    foreach (string item in ListSong.Items)   //Duyệt qua các item trong danh sách
                    {
                        if (item.ToString().Equals(fileNames[i], StringComparison.OrdinalIgnoreCase))   //Nếu file đã tồn tại trong danh sách (không phân biệt hoa thường)
                        {
                            isExist = true;   //Gán biến kiểm tra bằng true
                            break;   //Thoát khỏi vòng lặp
                        }
                    }
                    if (!isExist)   //Nếu file chưa tồn tại trong danh sách
                    {
                        DLL.AddSong(filePaths[i], fileNames[i]);   //Thêm file vào danh sách
                        ListSong.Items.Add(fileNames[i]);   //Hiện thị tên file trong danh sách
                    }
                    else   //Nếu file đã tồn tại trong danh sách
                        MessageBox.Show($"File \"{fileNames[i]}\" đã tồn tại trong danh sách!", "Trùng lặp", MessageBoxButtons.OK, MessageBoxIcon.Warning);   //Thông báo file đã tồn tại
                }
            }
        }
        private void btn_Remove_Click(object sender, EventArgs e)
        {
            if (ListSong.SelectedIndex != -1)   // Nếu đã chọn bài hát
            {
                int selectedIndex = ListSong.SelectedIndex;   // Lấy vị trí bài hát được chọn
                DLL.RemoveSong(selectedIndex);   // Xóa bài hát khỏi danh sách
                ListSong.Items.RemoveAt(selectedIndex);   // Xóa bài hát khỏi ListBox
                MessageBox.Show("Đã xóa bài hát thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);   // Thông báo xóa thành công
            }
            else MessageBox.Show("Vui lòng chọn một bài hát để xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);   // Thông báo chưa chọn bài hát
        }
        private void btn_Swap_Click(object sender, EventArgs e)
        {
            if(ListSong.SelectedIndices.Count != 2)   // Nếu chưa chọn đúng 2 dòng(bài hát)
            {
                MessageBox.Show("Vui lòng chọn 2 bài hát để hoán đổi vị trí!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);   // Thông báo chưa chọn 2 bài hát
                return;   // Thoát khỏi phương thức
            }
            int index1 = ListSong.SelectedIndices[0];   // Lấy vị trí bài hát đầu tiên
            int index2 = ListSong.SelectedIndices[1];   // Lấy vị trí bài hát thứ hai
            DLL.SwapSong(index1, index2);   // Hoán đổi vị trí 2 bài hát
            RefreshListSong(index1, index2);   // Cập nhật danh sách
            MessageBox.Show("Đã hoán đổi vị trí thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);   // Thông báo hoán đổi thành công
        }
        private void ListSong_DoubleClick(object sender, EventArgs e)
        {
            if (ListSong.SelectedIndex != -1)   //   Nếu đã chọn bài hát
            {
                int selectedIndex = ListSong.SelectedIndex;   // Lấy vị trí bài hát được chọn
                DLL.Current = DLL.Head;   // Khởi tạo Node hiện tại là Node đầu tiên
                for (int i = 0; i < selectedIndex; i++)   // Duyệt qua danh sách để tìm bài hát được chọn
                    DLL.Current = DLL.Current.Next;   // Chuyển đến Node tiếp theo
                axWindowsMediaPlayer.URL = DLL.Current.FilePath;   // Đường dẫn bài hát
                btnPlay.BackgroundImage = Properties.Resources.pause;   // Đổi biểu tượng sang Pause
                timer1.Start();   // Bắt đầu đếm thời gian
                trackBar1.Value = axWindowsMediaPlayer.settings.volume;   // Đặt giá trị âm lượng cho thanh trượt
                lbl_volume.Text = trackBar1.Value.ToString() + "%";   // Hiển thị âm lượng
            }
        }
        private bool isButtonClick = false;   // Biến kiểm tra trạng thái nút nhấn
        private void btnPlay_Click(object sender, EventArgs e)
        {
            isButtonClick = true; // Đánh dấu trạng thái được thay đổi bởi nút nhấn

            if (axWindowsMediaPlayer.playState == WMPLib.WMPPlayState.wmppsPlaying)   // Nếu đang phát nhạc
            {
                axWindowsMediaPlayer.Ctlcontrols.pause();   // Tạm dừng phát nhạc
                btnPlay.BackgroundImage = Properties.Resources.play;   // Đổi biểu tượng sang Play
            }
            else   // Nếu không đang phát nhạc
            {
                axWindowsMediaPlayer.Ctlcontrols.play();   // Phát nhạc
                btnPlay.BackgroundImage = Properties.Resources.pause;   // Đổi biểu tượng sang Pause
            }
            btnPlay.Refresh(); // Cập nhật giao diện
            isButtonClick = false; // Hoàn thành xử lý nút nhấn
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (DLL.Current != null)   // Nếu bài hiện tại không phải là null
            {
                DLL.MoveNext();   // Chuyển đến bài hát tiếp theo
                if (DLL.Current == null)   // Nếu bài hát là bài cuối cùng 
                    DLL.Current = DLL.Head;   // Quay lại bài hát đầu tiên
                if (DLL.Current != null)   // Nếu bài hát hiện tại không phải là null
                {
                    axWindowsMediaPlayer.URL = DLL.Current.FilePath;   // Đường dẫn bài hát
                    ListSong.SelectedIndices.Clear();   // Xóa tất cả các chỉ mục đã chọn
                    ListSong.SelectedIndex = ListSong.Items.IndexOf(DLL.Current.FileName);   // Đánh dấu chọn bài hát hiện tại trong danh sách
                }
            }
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (DLL.Current != null)   // Nếu bài hiện tại không phải là null
            {
                DLL.MovePrev();   // Chuyển đến bài hát tiếp theo
                if (DLL.Current == null)   // Nếu bài hát là bài cuối cùng 
                    DLL.Current = DLL.Tail;   // Quay lại bài hát đầu tiên
                if (DLL.Current != null)   // Nếu bài hát hiện tại không phải là null
                {
                    axWindowsMediaPlayer.URL = DLL.Current.FilePath;   // Đường dẫn bài hát
                    ListSong.SelectedIndices.Clear();   // Xóa tất cả các chỉ mục đã chọn
                    ListSong.SelectedIndex = ListSong.Items.IndexOf(DLL.Current.FileName);   // Đánh dấu chọn bài hát hiện tại trong danh sách
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (axWindowsMediaPlayer.playState == WMPLib.WMPPlayState.wmppsMediaEnded)
            {
                if (chkRepeat.Checked)
                {
                    axWindowsMediaPlayer.Ctlcontrols.play();
                }
                else
                {
                    btnNext.PerformClick(); // Phát bài kế tiếp
                }
            }
            if (axWindowsMediaPlayer.playState == WMPLib.WMPPlayState.wmppsPlaying)   // Nếu đang phát nhạc
            {
                progressBar1.Maximum = (int)axWindowsMediaPlayer.Ctlcontrols.currentItem.duration;   // Đặt giá trị tối đa cho thanh tiến trình
                progressBar1.Value = (int)axWindowsMediaPlayer.Ctlcontrols.currentPosition;   // Đặt giá trị cho thanh tiến trình
            }
            lbl_track_start.Text = axWindowsMediaPlayer.Ctlcontrols.currentPositionString;   // Hiển thị thời gian bắt đầu
            lbl_track_.Text = axWindowsMediaPlayer.Ctlcontrols.currentItem.durationString.ToString();   // Hiển thị thời gian kết thúc
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            axWindowsMediaPlayer.settings.volume = trackBar1.Value;   // Đặt âm lượng cho bài hát
            lbl_volume.Text = trackBar1.Value.ToString();   // Hiển thị âm lượng
        }

        private void cbPlaybackSpeed_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbPlaybackSpeed.SelectedItem.ToString())
            {
                case "1x":
                    axWindowsMediaPlayer.settings.rate = 1.0;
                    break;
                case "1.5x":
                    axWindowsMediaPlayer.settings.rate = 1.5;
                    break;
                case "2x":
                    axWindowsMediaPlayer.settings.rate = 2.0;
                    break;
            }
        }

        private void btnRandom_Click(object sender, EventArgs e)
        {
            if (ListSong.Items.Count == 0) return;

            Random rand = new Random();
            int randomIndex = rand.Next(ListSong.Items.Count);

            DLL.Current = DLL.Head;
            for (int i = 0; i < randomIndex; i++)
                DLL.Current = DLL.Current.Next;

            axWindowsMediaPlayer.URL = DLL.Current.FilePath;
            ListSong.SelectedIndex = randomIndex;
            btnPlay.BackgroundImage = Properties.Resources.pause;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.ToLower();  // Lấy giá trị tìm kiếm từ TextBox
            ListSong.Items.Clear();   // Xóa danh sách hiện tại

            SongNode current = DLL.Head;  // Khởi tạo Node hiện tại là Node đầu tiên
            while (current != null)
            {
                if (current.FileName.ToLower().Contains(searchTerm))  // Kiểm tra nếu tên bài hát chứa từ khóa tìm kiếm
                {
                    ListSong.Items.Add(current.FileName);  // Thêm bài hát vào danh sách
                }
                current = current.Next;  // Chuyển đến Node tiếp theo
            }
        }

        private void btnSort_Click(object sender, EventArgs e)
        {
            DLL.SortSongs();  // Gọi hàm sắp xếp danh sách bài hát
            RefreshListSong();  // Cập nhật lại danh sách trong ListBox
            // Thông báo sau khi sắp xếp
            MessageBox.Show("Đã sắp xếp xong!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void RefreshListSong()
        {
            ListSong.Items.Clear();   // Xóa danh sách hiện tại
            SongNode current = DLL.Head;   // Khởi tạo Node hiện tại là Node đầu tiên
            while (current != null)   // Duyệt qua các Node trong danh sách
            {
                ListSong.Items.Add(current.FileName);   // Thêm tên bài hát vào danh sách
                current = current.Next;   // Chuyển sang Node tiếp theo
            }
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnRewind15s_Click(object sender, EventArgs e)
        {
            double newPosition = axWindowsMediaPlayer.Ctlcontrols.currentPosition - 15;
            if (newPosition < 0) newPosition = 0;
            axWindowsMediaPlayer.Ctlcontrols.currentPosition = newPosition;
        }

        private void btnForward15s_Click(object sender, EventArgs e)
        {
            double duration = axWindowsMediaPlayer.currentMedia?.duration ?? 0;
    double newPosition = axWindowsMediaPlayer.Ctlcontrols.currentPosition + 15;
    if (newPosition > duration) newPosition = duration;
    axWindowsMediaPlayer.Ctlcontrols.currentPosition = newPosition;
        }

        private void RefreshListSong(int index1, int index2)   
        {
            ListSong.Items.Clear();   // Xóa danh sách hiện tại
            SongNode current = DLL.Head;   // Khởi tạo Node hiện tại là Node đầu tiên
            while (current != null)   // Duyệt qua các Node trong danh sách
            {
                ListSong.Items.Add(current.FileName);   // Thêm tên bài hát vào danh sách
                current = current.Next;   // Chuyển sang Node tiếp theo
            }
            ListSong.SelectedIndices.Add(index1);   //Đánh dấu chọn bài hát đầu tiên
            ListSong.SelectedIndices.Add(index2);   //Đánh dấu chọn bài hát thứ hai
        }
    }
}
